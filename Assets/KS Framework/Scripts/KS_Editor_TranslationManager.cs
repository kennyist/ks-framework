using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using System;

public class KS_Editor_TranslationManager : EditorWindow {

    KS_FileHelper IO;
    KS_Localisation translationManager;
    string[] files;

    [SerializeField] TreeViewState treeViewState;
    KS_Edtior_TranslationsTreeView fileTreeView;

    bool AddFile = false;

    // Use this for initialization
    void OnEnable () { 
        IO = new KS_FileHelper("KSFW Example", KS_FileHelper.DataLocation.MyDocuments, "KS_Data");
        files = IO.GetApplicationDataContents(KS_FileHelper.GameDataFolders.Translate);

        for(int i = 0; i < files.Length; i++)
        {
            files[i] = files[i].Replace(".txt", "");
        }

        translationManager = new KS_Localisation();

        if (treeViewState == null)
            treeViewState = new TreeViewState();

        LoadLanguages();

        fileTreeView = new KS_Edtior_TranslationsTreeView(treeViewState, files, keys.ToArray());

    }

    void ReloadData()
    {
        files = IO.GetApplicationDataContents(KS_FileHelper.GameDataFolders.Translate);

        keys.Clear();
        languages.Clear();

        for (int i = 0; i < files.Length; i++)
        {
            files[i] = files[i].Replace(".txt", "");
        }

        translationManager = new KS_Localisation();

        if (treeViewState == null)
            treeViewState = new TreeViewState();

        LoadLanguages();

        fileTreeView = new KS_Edtior_TranslationsTreeView(treeViewState, files, keys.ToArray());
    }

    [MenuItem("KS: Framework/Translation Manager")]
    static void Init()
    {
        KS_Editor_TranslationManager window = (KS_Editor_TranslationManager)EditorWindow.GetWindow(typeof(KS_Editor_TranslationManager));
        window.titleContent.text = "Translation Manager";
        window.Show();
    }

    string searchString = "";
    Vector2 scrollPos = new Vector2();

    private Dictionary<string, Dictionary<string, string>> languages = new Dictionary<string, Dictionary<string, string>>();
    private List<string> keys;

    private void LoadLanguages()
    {
        keys.Clear();

        foreach(string s in files)
        {
            string replaced = s.Replace(".txt", "");

            Debug.Log(replaced);

            languages.Add(s, parseTranslationFile(replaced));
        }

        Dictionary<string, string> lang = languages[files[0]];
        

        foreach(KeyValuePair<string,string> kvp in lang)
        {
            keys.Add(kvp.Key);
        }

    }

    private Dictionary<string,string> parseTranslationFile(string name)
    {
        string file = IO.LoadGameFile(KS_FileHelper.GameDataFolders.Translate, name + ".txt");

        Debug.Log("file: " + file);

        Dictionary<string, string> loadedLines = new Dictionary<string, string>();

        if (!string.IsNullOrEmpty(file))
        {

            string[] lines = file.Split(
                        new[] { Environment.NewLine },
                        StringSplitOptions.None
                    );

            foreach (string s in lines)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    string[] line = s.Split(new[] { '=' }, 2);

                    loadedLines.Add(line[0], line[1]);
                }
            }

            Debug.Log(loadedLines.Count + " Lines loaded");
        }

        return loadedLines;
    }


    // GUI

    void OnGUI()
    {
        // Menu
        GUILayout.BeginHorizontal(EditorStyles.toolbar);
        DrawToolStrip();
        GUILayout.EndHorizontal();

        if (AddFile)
        {
            DrawAddFile();
            return;
        } 

        GUILayout.BeginScrollView(scrollPos);
        DrawMain();
        GUILayout.EndScrollView();
    }

    string addFileName = "";

    void DrawAddFile()
    {
        GUILayout.BeginVertical();
        GUILayout.Label("New Translation Name:");

        GUILayout.BeginHorizontal();
        addFileName = GUILayout.TextField(addFileName);
        GUILayout.Label(".txt");
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Add File"))
        {
            CreateNewTranslationFile();
            AddFile = false;
        }

        GUILayout.EndVertical();
    }

    void CreateNewTranslationFile()
    {

        if (string.IsNullOrEmpty(addFileName)) return;

        IO.SaveGameFile(KS_FileHelper.GameDataFolders.Translate, addFileName + ".txt", "");

        ReloadData();

        addFileName = "";
    }

    void DrawMain()
    {
        Rect test = new Rect(0, 0, 300, 300);

        fileTreeView.OnGUI(test);
    }

    void DrawToolStrip()
    {
        if (GUILayout.Button("Add New", EditorStyles.toolbarDropDown))
        {
            Debug.Log("testsetsetset");
            GenericMenu toolsMenu = new GenericMenu();
            toolsMenu.AddItem(new GUIContent("Language"), false, one);
            toolsMenu.AddItem(new GUIContent("string"), false, two);
            toolsMenu.DropDown(new Rect(5, 0, 0, 16));
            EditorGUIUtility.ExitGUI();
        }

        GUILayout.Space(5);

        searchString = GUILayout.TextField(searchString, GUI.skin.FindStyle("ToolbarSeachTextField"), GUILayout.MinWidth(100));
        if (GUILayout.Button("", GUI.skin.FindStyle("ToolbarSeachCancelButton")))
        {
            // Remove focus if cleared
            searchString = "";
            GUI.FocusControl(null);
        }

        GUILayout.Space(5);

        GUILayout.Label(keys.Count + " Strings found", EditorStyles.miniLabel);

        GUILayout.FlexibleSpace();
    }

    void one() {
        AddFile = true;
    }
    void two() { }

    // Update is called once per frame
    void Update () {
		
	}
}

class KS_Edtior_TranslationsTreeView : TreeView
{
    string[] files;
    string[] keys;

    public KS_Edtior_TranslationsTreeView(TreeViewState treeViewState, string[] files, string[] keys)
        : base(treeViewState)
    {
        this.files = files;
        this.keys = keys;
        Reload();
    }

    protected override TreeViewItem BuildRoot()
    {
        // BuildRoot is called every time Reload is called to ensure that TreeViewItems 
        // are created from data. Here we create a fixed set of items. In a real world example,
        // a data model should be passed into the TreeView and the items created from the model.

        // This section illustrates that IDs should be unique. The root item is required to 
        // have a depth of -1, and the rest of the items increment from that.
        var root = new TreeViewItem { id = 0, depth = -1, displayName = "Translations" };
        var allItems = new List<TreeViewItem>();
        int i = 1;

        allItems.Add(new TreeViewItem { id = i, depth = 0, displayName = "Translations" });
        i++;

        foreach (string s in files)
        {
            allItems.Add(new TreeViewItem { id = i, depth = 1, displayName = s });
            i++;
        }

        allItems.Add(new TreeViewItem { id = i, depth = 0, displayName = "Strings" });

        foreach (string s in keys)
        {
            i++;
            allItems.Add(new TreeViewItem { id = i, depth = 1, displayName = s });
        }


        // Utility method that initializes the TreeViewItem.children and .parent for all items.
        SetupParentsAndChildrenFromDepths(root, allItems);

        // Return root of the tree
        return root;
    }
}
