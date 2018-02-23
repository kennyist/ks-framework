using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using System;
using System.Linq;

public class KS_Editor_TranslationManager : EditorWindow
{

    public KS_Storage_Translations translations;

    [SerializeField] TreeViewState treeViewState;
    KS_Edtior_TranslationsTreeView fileTreeView;

    private string searchString = "";
    private string lastSearchString = "";

    private bool addBox = false;
    private bool addBox_isString = false;
    private string addBox_String = "";

    [MenuItem("KS: Framework/Translation Manager")]
    static void Init()
    {
        KS_Editor_TranslationManager window = (KS_Editor_TranslationManager)EditorWindow.GetWindow(typeof(KS_Editor_TranslationManager));
        window.titleContent.text = "Translation Manager";
        window.Show();
    }

    void OnEnable()
    {
        if (EditorPrefs.HasKey("ObjectPath"))
        {
            string objectPath = EditorPrefs.GetString("ObjectPath");
            translations = AssetDatabase.LoadAssetAtPath(objectPath, typeof(KS_Storage_Translations)) as KS_Storage_Translations;
            ReloadData();
        }

        if (treeViewState == null)
            treeViewState = new TreeViewState();

    }

    private void OnGUI()
    {

        GUILayout.BeginHorizontal(EditorStyles.toolbar);

        if (!translations)
        {
            if (GUILayout.Button("Open Database", EditorStyles.toolbarButton))
            {
                OpenDatabase();
            }

            if (GUILayout.Button("Create Database", EditorStyles.toolbarButton))
            {
                CreateDatabase();
            }
        }
        else
        {
            if (GUILayout.Button("Close Database", EditorStyles.toolbarButton))
            {
                CloseDatabase();
            }

            if (GUILayout.Button("Add", EditorStyles.toolbarDropDown))
            {
                GenericMenu toolsMenu = new GenericMenu();
                toolsMenu.AddItem(new GUIContent("Language"), false, AddLanguage);
                toolsMenu.AddItem(new GUIContent("string"), false, AddString);
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

            if (searchString != lastSearchString)
            {
                lastSearchString = searchString;

                fileTreeView.searchString = searchString;
            }

            GUILayout.Space(5);
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        if (!translations) return; //

        GUILayout.BeginHorizontal();

        if (addBox)
        {
            GUILayout.Label("Add new " + ((addBox_isString) ? " string key:" : " language name:"));
            addBox_String = GUILayout.TextField(addBox_String);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add"))
            {
                if (addBox_isString)
                {
                    AddString(addBox_String);
                }
                else
                {
                    AddLanguage(addBox_String);
                }

                addBox_String = "";
                addBox = false;
            }
            if (GUILayout.Button("Cancle"))
            {
                addBox = false;
            }
            GUILayout.EndHorizontal();
        }
        else
        {
            Rect test = new Rect(0, 20, 150, 300);
            GUILayout.BeginArea(test);
            fileTreeView.OnGUI(test);
            GUILayout.EndArea();

            GUILayout.BeginVertical();
            GUILayout.Label("string: ");
            


            GUILayout.EndVertical();
        }

        GUILayout.EndHorizontal();

    }

    void OpenDatabase()
    {
        string absPath = EditorUtility.OpenFilePanel("Select Translation Database", "", "");
        if (absPath.StartsWith(Application.dataPath))
        {
            string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
            translations = AssetDatabase.LoadAssetAtPath(relPath, typeof(KS_Storage_Translations)) as KS_Storage_Translations;

            if (translations)
            {
                EditorPrefs.SetString("ObjectPath", relPath);
            }
        }

        ReloadData();
    }

    void ReloadData()
    {
        fileTreeView = new KS_Edtior_TranslationsTreeView(treeViewState, GetLanguages(), GetStrings());

        fileTreeView.OnSelectedChange += LoadString;
    }

    void CreateDatabase()
    {
        string absPath = EditorUtility.SaveFilePanel("Create Translation Database", "", "Translations", "asset");
        if (absPath.StartsWith(Application.dataPath))
        {
            translations = ScriptableObject.CreateInstance<KS_Storage_Translations>();

            Debug.Log(absPath);
            absPath = absPath.Replace(Application.dataPath, "");
            Debug.Log(absPath);

            absPath = "Assets" + absPath;

            AssetDatabase.CreateAsset(translations, absPath);
            AssetDatabase.SaveAssets();
        }

        ReloadData();
    }

    void CloseDatabase()
    {
        translations = null;
    }

    string[] GetStrings()
    {
        List<string> keys = new List<string>();

        foreach (KS_Storage_Translations.Language l in translations.languages)
        {
            foreach (KS_Storage_Translations.Language.TranslationString s in l.strings)
            {
                if (!keys.Contains(s.lineID))
                {
                    keys.Add(s.lineID);
                }
            }
        }

        return keys.ToArray();
    }

    string[] GetLanguages()
    {
        List<string> ret = new List<string>();

        foreach (KS_Storage_Translations.Language l in translations.languages)
        {
            ret.Add(l.language);
        }

        return ret.ToArray();
    }

    void AddLanguage()
    {
        addBox_isString = false;
        addBox = true;
    }

    void AddLanguage(string language)
    {
        KS_Storage_Translations.Language l = new KS_Storage_Translations.Language();
        l.language = language;

        foreach (string s in GetStrings())
        {
            KS_Storage_Translations.Language.TranslationString ts = new KS_Storage_Translations.Language.TranslationString();
            ts.lineID = s;
            ts.lineText = "# Line not set #";
        }

        translations.languages.Add(l);

        ReloadData();
    }

    void AddString()
    {
        addBox_isString = true;
        addBox = true;
    }

    void AddString(string key)
    {
        foreach (KS_Storage_Translations.Language l in translations.languages)
        {
            KS_Storage_Translations.Language.TranslationString ts = new KS_Storage_Translations.Language.TranslationString();
            ts.lineID = key;
            l.strings.Add(ts);
        }

        ReloadData();
    }

    KeyValuePair<string, string>[] LoadString(string key)
    {
    }

    void DeleteString()
    {

    }

    void EditString()
    {

    }

}

class KS_Edtior_TranslationsTreeView : TreeView
{
    string[] files;
    string[] keys;

    public delegate void SelectedChange(string key);
    public event SelectedChange OnSelectedChange;

    public KS_Edtior_TranslationsTreeView(TreeViewState treeViewState, string[] files, string[] keys)
        : base(treeViewState)
    {
        this.files = files;
        this.keys = keys;
        Reload();
    }

    protected override void SelectionChanged(IList<int> selectedIds)
    {

        if (selectedIds[0] > 999)
        {

            if (OnSelectedChange != null)
            {
                OnSelectedChange(this.keys[selectedIds[0] - 999]);
            }
        }

        base.SelectionChanged(selectedIds);
    }

    protected override TreeViewItem BuildRoot()
    {
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

        i = 999;
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