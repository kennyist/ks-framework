using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class KS_Editor_Manager : EditorWindow
{
    private static string ConfigSaveFile = "Assets/KS_Data/GameConfig.asset";

    private float labelWidth = 200;


    private bool configFound = false;
    private KS_Scriptable_GameConfig gameConfig;

    [MenuItem("KS: OWGF/Manager", false, 20)]
    static void Init()
    {
        KS_Editor_Manager window = (KS_Editor_Manager)EditorWindow.GetWindow(typeof(KS_Editor_Manager));
        window.titleContent.text = "KS_OWGF Manager";
        window.Show();
    }

    [MenuItem("KS: OWGF/Setup", false, 1)]
    private static void CreateManager()
    {
        GameObject obj = new GameObject("KS: Open World Framework");

        KS_Scriptable_GameConfig config = FindGameConfig();

        if (config == null) config = CreateConfig();

        obj.AddComponent<KS_Manager>();
        obj.GetComponent<KS_Manager>().gameConfig = config;

        obj.AddComponent<KS_Localisation>();
        obj.GetComponent<KS_Localisation>().gameConfig = config;

        obj.AddComponent<KS_Settings>();
        obj.GetComponent<KS_Settings>().gameConfig = config;

        Init();
    }

    private void OnEnable()
    {
        gameConfig = FindGameConfig();
        if (gameConfig == null) configFound = false;
        else configFound = true;
    }

    private static KS_Scriptable_GameConfig FindGameConfig()
    {
        KS_Scriptable_GameConfig gameConfig = AssetDatabase.LoadAssetAtPath(ConfigSaveFile, typeof(KS_Scriptable_GameConfig)) as KS_Scriptable_GameConfig;

        if (gameConfig) return gameConfig;
        return null;
    }

    private static KS_Scriptable_GameConfig CreateConfig()
    {
        KS_Scriptable_GameConfig gameConfig = ScriptableObject.CreateInstance<KS_Scriptable_GameConfig>();

        AssetDatabase.CreateAsset(gameConfig, ConfigSaveFile);
        AssetDatabase.SaveAssets();
        gameConfig = AssetDatabase.LoadAssetAtPath(ConfigSaveFile, typeof(KS_Scriptable_GameConfig)) as KS_Scriptable_GameConfig;

        return gameConfig;
    }

    // GUI

    private void OnGUI()
    {
        if (!configFound)
        {
            GUIDrawCreateConfig();
            return;
        }

        GUIDrawConfigEditor();
    }

    private void GUIDrawCreateConfig()
    {
        GUILayout.BeginVertical();
        GUILayout.Label("Game config not found");
        if(GUILayout.Button("Create Config"))
        {
            CreateConfig();
        }
        GUILayout.EndVertical();
    }

    private void GUIDrawConfigEditor()
    {
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Game name:", GUILayout.Width(labelWidth));
        gameConfig.gameName = GUILayout.TextField(gameConfig.gameName);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Game version:", GUILayout.Width(labelWidth));
        gameConfig.version = GUILayout.TextField(gameConfig.version);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Build Number:", GUILayout.Width(labelWidth));
        gameConfig.buildNumber = EditorGUILayout.IntField(gameConfig.buildNumber);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Windows Application data folder:",GUILayout.Width(labelWidth));
        gameConfig.windowsDataLocation = (KS_FileHelper.DataLocation)EditorGUILayout.EnumPopup(gameConfig.windowsDataLocation);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Mac Application data folder:", GUILayout.Width(labelWidth));
        gameConfig.macDataLocation = (KS_FileHelper.DataLocation)EditorGUILayout.EnumPopup(gameConfig.macDataLocation);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Linux Application data folder:", GUILayout.Width(labelWidth));
        gameConfig.linuxDataLocation = (KS_FileHelper.DataLocation)EditorGUILayout.EnumPopup(gameConfig.linuxDataLocation);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Screenshot Save location:", GUILayout.Width(labelWidth));
        gameConfig.ScreenShotSaveLocation = (KS_FileHelper.ScreenShotSaveLocation)EditorGUILayout.EnumPopup(gameConfig.ScreenShotSaveLocation);
        GUILayout.EndHorizontal();



        GUILayout.BeginHorizontal();
        GUILayout.Label("Return Not found:", GUILayout.Width(labelWidth));
        gameConfig.loc_returnNotFound = EditorGUILayout.Toggle(gameConfig.loc_returnNotFound);
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
    }
}
