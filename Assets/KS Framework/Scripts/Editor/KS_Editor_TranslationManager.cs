using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using System;
using System.Linq;

public class KS_Editor_TranslationManager : EditorWindow
{

    public KS_Scriptable_Translations translations;

    private string searchString = "";
    private string lastSearchString = "";
    private bool IsSearching = false;

    private bool addBox = false;
    private bool addBox_isString = false;
    private string addBox_String = "";
    private bool addBox_Error = false;

    private string[] keys;
    private string[] languages;
    
    private KeyValuePair<string, KS_Scriptable_Translations_Language.TranslationString>[] loadedLines = null;
    private string loadedString = null;

    [MenuItem("KS: OWGF/Translation Manager", false, 21)]
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
            translations = AssetDatabase.LoadAssetAtPath(objectPath, typeof(KS_Scriptable_Translations)) as KS_Scriptable_Translations;
        }

        GUISetup();

        if (translations)
        {
            UpdateDisplayData();
        }
    }

    private void OnGUI()
    {
        if(searchString != lastSearchString)
        {
            lastSearchString = searchString;

            if (string.IsNullOrEmpty(searchString))
            {
                IsSearching = false;
            }
            else if (!IsSearching)
            {
                IsSearching = true;
            }

            Debug.Log("Search string: " + searchString);

            UpdateDisplayData();
        }

        GUILayout.BeginHorizontal(EditorStyles.toolbar);
        DrawToolBar();
        GUILayout.EndHorizontal();

        if (!translations) return; //

        GUILayout.BeginHorizontal();
        DrawKeyMenu();
        if (!addBox)
        {
            DrawMainArea();
        }
        else
        {
            DrawAddBox();
        }
        GUILayout.EndHorizontal();

    }

    /*GUIStyle keyLabel = new GUIStyle(EditorStyles.label);*/

    void GUISetup()
    {
        /*keyLabel.fixedWidth = 100f;
        keyLabel.onActive.textColor = Color.blue;
        keyLabel.alignment = TextAnchor.MiddleLeft;
        keyLabel.fixedHeight = 15f;
        keyLabel.margin = new RectOffset(5, 0, 0, 0);*/
    }


    void OpenDatabase()
    {
        string absPath = EditorUtility.OpenFilePanel("Select Translation Database", "", "");
        if (absPath.StartsWith(Application.dataPath))
        {
            string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
            translations = AssetDatabase.LoadAssetAtPath(relPath, typeof(KS_Scriptable_Translations)) as KS_Scriptable_Translations;

            if (translations)
            {
                EditorPrefs.SetString("ObjectPath", relPath);
            }
        }

        UpdateDisplayData();
    }

    void CreateDatabase()
    {
        string absPath = EditorUtility.SaveFilePanel("Create Translation Database", "", "Translations", "asset");
        if (absPath.StartsWith(Application.dataPath))
        {
            translations = ScriptableObject.CreateInstance<KS_Scriptable_Translations>();

            Debug.Log(absPath);
            absPath = absPath.Replace(Application.dataPath, "");
            Debug.Log(absPath);

            absPath = "Assets" + absPath;

            AssetDatabase.CreateAsset(translations, absPath);
            AssetDatabase.SaveAssets();
        }
        
        UpdateDisplayData();
    }

    void SaveDatabase()
    {
        UpdateDisplayData();
    }

    // Update display data

    void UpdateDisplayData()
    {
        if (translations)
        {
            EditorUtility.SetDirty(translations);
            AssetDatabase.SaveAssets();
        }

        keys = GetAllKeys();
        languages = GetAllLanguages();
        Debug.Log("");
    }

    // GUI

    void DrawToolBar()
    {
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
            if (GUILayout.Button("Open Database", EditorStyles.toolbarButton))
            {
                OpenDatabase();
            }

            if (translations)
            {
                if (GUILayout.Button("Save Database", EditorStyles.toolbarButton))
                {
                    SaveDatabase();
                }
            }

            if (GUILayout.Button("Create Database", EditorStyles.toolbarButton))
            {
                CreateDatabase();
            }


            if (GUILayout.Button("Add", EditorStyles.toolbarDropDown))
            {
                GenericMenu toolsMenu = new GenericMenu();
                toolsMenu.AddItem(new GUIContent("Language"), false, AddBoxLanguage);
                toolsMenu.AddItem(new GUIContent("string"), false, AddBoxString);
                toolsMenu.DropDown(new Rect(280, 0, 0, 16));
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
        }
        GUILayout.FlexibleSpace();
    }

    Vector2 scrollPos = new Vector2();

    void DrawKeyMenu()
    {
        GUILayout.BeginVertical(GUILayout.Width(200));
        GUILayout.Label("Languages");

        if(languages != null || languages.Length > 0)
        {
            for(int i = 0; i < languages.Length; i++)
            {
                if(GUILayout.Button(languages[i], EditorStyles.miniButton))
                {

                }
            }
        }

        GUILayout.Space(10);

        GUILayout.Label("Keys");

        scrollPos = GUILayout.BeginScrollView(scrollPos);


        if(keys != null || keys.Length > 0)
        {
            for(int i = 0; i < keys.Length; i++)
            {
                EditorStyles.miniButton.alignment = TextAnchor.MiddleLeft;
                if (GUILayout.Button(keys[i], EditorStyles.miniButton))
                {
                    loadedLines = null;
                    loadedLines = GetKeyData(i);
                    loadedString = GetKeyData(i)[0].Value.lineID;
                }
            }
        }

        GUILayout.EndScrollView();
        GUILayout.EndVertical();
    }

    void DrawMainArea()
    {
        GUILayout.BeginVertical();
        if (loadedString != null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("LineID: " + loadedString);
            EditorGUILayout.Separator();
            if (GUILayout.Button("Delete Line", EditorStyles.miniButton))
            {
                DeleteString(loadedString);
            }
            GUILayout.EndHorizontal();

            if (loadedLines != null && loadedLines.Length > 0)
            {
                for (int i = 0; i < loadedLines.Length; i++)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(loadedLines[i].Key, GUILayout.Width(100));
                    loadedLines[i].Value.lineText = EditorGUILayout.TextArea(loadedLines[i].Value.lineText, GUILayout.ExpandWidth(true));
                    GUILayout.EndHorizontal();
                }
            }
        }
        else
        {
            GUILayout.Label("No lineID selected, Select one from te left menu.");
        }

        GUILayout.EndVertical();
    }

    void DrawAddBox()
    {
        GUILayout.BeginVertical();
        string postfix = (addBox_isString) ? "line ID" : "language";

        GUILayout.Label("Add new " + postfix);
        addBox_String = GUILayout.TextField(addBox_String);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add"))
        {
            if (addBox_isString)
            {
                if (DoesKeyExist(addBox_String))
                {
                    addBox_Error = true;
                }
                else
                {
                    AddString(addBox_String);
                    addBox = false;
                    addBox_String = "";
                    addBox_Error = false;
                }
            }
            else
            {
                if (DoesLanguageExist(addBox_String))
                {
                    addBox_Error = true;
                }
                else
                {
                    AddLanguage(addBox_String);
                    addBox = false;
                    addBox_String = "";
                    addBox_Error = false;
                }
            }
        }
        if (GUILayout.Button("Cancle"))
        {
            addBox = false;
            addBox_String = "";
            addBox_Error = false;
        }
        GUILayout.EndHorizontal();

        if (addBox_Error)
        {
            GUILayout.Label("Already exists in database");
        }
        GUILayout.EndVertical();
    }

    // Get

    string[] GetAllKeys()
    {
        List<string> keys = new List<string>();

        if (translations.languages.Count <= 0)
        {
            return keys.ToArray();
        }

        for(int i = 0; i < translations.languages.Count; i++)
        {
            if(translations.languages[i].strings.Count > 0)
            {
                for(int j = 0; j < translations.languages[i].strings.Count; j++)
                {
                    if (!keys.Contains(translations.languages[i].strings[j].lineID))
                    {
                        keys.Add(translations.languages[i].strings[j].lineID);
                    }
                }
            }
        }

        if (IsSearching)
        {
            keys = keys.FindAll(s => s.Contains(searchString));
        }

        keys.Sort();

        return keys.ToArray();
    }

    string[] GetAllLanguages()
    {
        List<string> languages = new List<string>();

        if(translations.languages.Count <= 0)
        {
            return languages.ToArray();
        }

        foreach (KS_Scriptable_Translations_Language l in translations.languages)
        {
            languages.Add(l.language);
        }

        return languages.ToArray();
    }

    KeyValuePair<string, KS_Scriptable_Translations_Language.TranslationString>[] GetKeyData(int index)
    {
        Debug.Log(index);

        List<KeyValuePair<string, KS_Scriptable_Translations_Language.TranslationString>> lines = new List<KeyValuePair<string, KS_Scriptable_Translations_Language.TranslationString>>();

        if (translations.languages.Count <= 0) return null;

        for(int i = 0; i < translations.languages.Count; i++)
        {
            if(translations.languages[i].strings.Count > 0)
            {
                for(int j = 0; j < translations.languages[i].strings.Count; j++)
                {
                    if(translations.languages[i].strings[j].lineID.Equals(GetAllKeys()[index]))
                    {
                        lines.Add(new KeyValuePair<string, KS_Scriptable_Translations_Language.TranslationString>(translations.languages[i].language, translations.languages[i].strings[j]));
                    }
                }
            }
        }

        return lines.ToArray();
    }

    // check

    bool DoesKeyExist(string key)
    {
        List<string> keys = GetAllKeys().ToList<string>();

        if (keys.Contains(key))
        {
            return true;
        }

        return false;
    }

    bool DoesLanguageExist(string language)
    {
        Debug.Log("Got language: " + language);
        if(translations.languages.Count <= 0)
        {
            return false;
        }

        foreach(KS_Scriptable_Translations_Language l in translations.languages)
        {
            Debug.Log(l.language + " : " + language);
            if (l.language == language) return true;
        }

        return false;
    }

    // Add box buttons

    void AddBoxString()
    {
        addBox_isString = true;
        addBox = true;
        addBox_Error = false;
        addBox_String = "";
    }

    void AddBoxLanguage()
    {
        addBox_isString = false;
        addBox = true;
        addBox_Error = false;
        addBox_String = "";
    }

    // Add

    void AddLanguage(string name)
    {
        KS_Scriptable_Translations_Language l = new KS_Scriptable_Translations_Language();
        l.language = name;
        translations.languages.Add(l);

        if(GetAllKeys().Length > 0)
        {
            for(int i = 0; i < GetAllKeys().Length; i++)
            {
                KS_Scriptable_Translations_Language.TranslationString s = new KS_Scriptable_Translations_Language.TranslationString();
                s.lineID = GetAllKeys()[i];

                l.strings.Add(s);
            }
        }

        UpdateDisplayData();
    }

    void AddString(string ID)
    {
        ID = ID.ToLower();

        if(translations.languages.Count > 0)
        {
            for(int i = 0; i < translations.languages.Count; i++)
            {
                KS_Scriptable_Translations_Language.TranslationString s = new KS_Scriptable_Translations_Language.TranslationString();
                s.lineID = ID;
                s.lineText = "# Not Set #";

                translations.languages[i].strings.Add(s);
            }
        }

        UpdateDisplayData();

        loadedLines = null;
        loadedLines = GetKeyData(keys.Length - 1);
        loadedString = ID;
    }

    // Delete

    private void DeleteString(string lineID)
    {
        if (translations.languages.Count > 0)
        {
            for (int i = 0; i < translations.languages.Count; i++)
            {
                Debug.Log("DELSTR: Language - " + translations.languages[i].language);
                for(int j = 0; j < translations.languages[i].strings.Count; j++)
                {
                    Debug.Log("DELSTR: Line - " + translations.languages[i].strings[j].lineID);
                    if (translations.languages[i].strings[j].lineID == lineID)
                    {
                        Debug.Log("DELSTR: DELETED");
                        translations.languages[i].strings.RemoveAt(j);
                    }
                }
            }
        }

        loadedLines = null;
        loadedString = null;

        UpdateDisplayData();
    }
}