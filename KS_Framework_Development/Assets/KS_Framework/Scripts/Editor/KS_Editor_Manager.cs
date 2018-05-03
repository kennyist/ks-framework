using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using KS_Core.Settings;
using KS_Core.Input;
using KS_Core.Localisation;
using KS_Core.Pooling;
using KS_Core.IO;
using KS_Utility;

namespace KS_Core.Editor
{
    /// <summary>
    /// KS framework mconfig editor window
    /// <see cref="KS_Scriptable_GameConfig"/>
    /// </summary>
    public class KS_Editor_Manager : EditorWindow
    {
        private static string ConfigSaveFile = "Assets/KS_Framework/KS_Data/GameConfig.asset";

        private float labelWidth = 200;


        private bool configFound = false;
        private KS_Scriptable_GameConfig gameConfig;

        [MenuItem("KS: Framework/Manager", false, 20)]
        static void Init()
        {
            KS_Editor_Manager window = (KS_Editor_Manager)EditorWindow.GetWindow(typeof(KS_Editor_Manager));
            window.titleContent.text = "KS: Framework Manager";
            window.Show();
        }

        [MenuItem("KS: Framework/Setup", false, 1)]
        private static void CreateManager()
        {
            GameObject obj = new GameObject("KS: Framework");
            GameObject pooling = new GameObject("KS: Pool Container");
            obj.transform.SetSiblingIndex(0);
            pooling.transform.SetParent(obj.transform);
            Selection.activeGameObject = obj;

            KS_Scriptable_GameConfig config = null;

            string absPath = EditorUtility.SaveFilePanel("Create Game Config", "", "GameConfig", "asset");
            if (absPath.StartsWith(Application.dataPath))
            {
                config = ScriptableObject.CreateInstance<KS_Scriptable_GameConfig>();

                absPath = absPath.Replace(Application.dataPath, "");
                absPath = "Assets" + absPath;

                AssetDatabase.CreateAsset(config, absPath);
                AssetDatabase.SaveAssets();
            }

            obj.AddComponent<KS_Manager>();
            obj.GetComponent<KS_Manager>().gameConfig = config;

            obj.AddComponent<KS_Localisation>();
            obj.GetComponent<KS_Localisation>().gameConfig = config;

            obj.AddComponent<KS_Settings>();
            obj.GetComponent<KS_Settings>().gameConfig = config;

            obj.AddComponent<KS_Input>();
            obj.GetComponent<KS_Input>().gameConfig = config;

            GameObject obj2 = new GameObject("Load Screen Container");
            obj2.transform.parent = obj.transform;

            obj.AddComponent<KS_LoadScreen>();
            obj.GetComponent<KS_LoadScreen>().LoadScreenContainer = obj2;

            obj.AddComponent<KS_PoolManager>();
            obj.GetComponent<KS_PoolManager>().gameConfig = config;
            obj.GetComponent<KS_PoolManager>().pooledObjectsContainer = pooling;

            obj.AddComponent<KS_Subtitle>();

            //GameObject obj3 = Instantiate(AssetDatabase.LoadAssetAtPath("Assets/KS Framework/Prefabs/UI/KS_prefab_console.prefab", typeof(GameObject))) as GameObject;
            //obj3.transform.parent = obj.transform;


            Init();
        }

        private void OnEnable()
        {
            configFound = false;
        }

        void OpenDatabase()
        {
            string absPath = EditorUtility.OpenFilePanel("Select Game Config", "", "");
            if (absPath.StartsWith(Application.dataPath))
            {
                string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
                gameConfig = AssetDatabase.LoadAssetAtPath(relPath, typeof(KS_Scriptable_GameConfig)) as KS_Scriptable_GameConfig;

                if (gameConfig)
                {
                    EditorPrefs.SetString("ObjectPath", relPath);
                    configFound = true;
                }
            }
        }

        private void CreateDatabase()
        {
            string absPath = EditorUtility.SaveFilePanel("Create Game Config", "", "GameConfig", "asset");
            KS_Scriptable_GameConfig config;
            if (absPath.StartsWith(Application.dataPath))
            {
                config = ScriptableObject.CreateInstance<KS_Scriptable_GameConfig>();

                absPath = absPath.Replace(Application.dataPath, "");
                absPath = "Assets" + absPath;

                AssetDatabase.CreateAsset(config, absPath);
                AssetDatabase.SaveAssets();
                gameConfig = AssetDatabase.LoadAssetAtPath(absPath, typeof(KS_Scriptable_GameConfig)) as KS_Scriptable_GameConfig;
                configFound = true;
            }
        }

        private void SaveDatabase()
        {
            if (gameConfig)
            {
                EditorUtility.SetDirty(gameConfig);
                AssetDatabase.SaveAssets();
            }
        }

        // GUI

        private void OnGUI()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            DrawToolBar();
            GUILayout.EndHorizontal();

            if (!configFound)
            {
                return;
            }

            GUIDrawConfigEditor();
        }

        void DrawToolBar()
        {
            if (!gameConfig)
            {
                if (GUILayout.Button("Open config", EditorStyles.toolbarButton))
                {
                    OpenDatabase();
                }

                if (GUILayout.Button("Create config", EditorStyles.toolbarButton))
                {
                    CreateDatabase();
                }
            }
            else
            {
                if (GUILayout.Button("Open config", EditorStyles.toolbarButton))
                {
                    OpenDatabase();
                }

                if (gameConfig)
                {
                    if (GUILayout.Button("Save config", EditorStyles.toolbarButton))
                    {
                        SaveDatabase();
                    }
                }

                GUILayout.Space(5);
            }
            GUILayout.FlexibleSpace();
        }

        private Vector2 scrollPos = new Vector2();

        private void GUIDrawConfigEditor()
        {
            GUILayout.BeginVertical();
            scrollPos = GUILayout.BeginScrollView(scrollPos);

            GUIDrawHeader("General:");

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

            GUIDrawHeader("IO:");

            GUILayout.BeginHorizontal();
            GUILayout.Label("Game Folder Name:", GUILayout.Width(labelWidth));
            gameConfig.gameFolderName = EditorGUILayout.TextField(gameConfig.gameFolderName);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Windows Application data folder:", GUILayout.Width(labelWidth));
            gameConfig.windowsDataLocation = (WindowsDataLocation)EditorGUILayout.EnumPopup(gameConfig.windowsDataLocation);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Mac Application data folder:", GUILayout.Width(labelWidth));
            gameConfig.macDataLocation = (OSXDataLocation)EditorGUILayout.EnumPopup(gameConfig.macDataLocation);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Linux Application data folder:", GUILayout.Width(labelWidth));
            gameConfig.linuxDataLocation = (LinuxDataLocation)EditorGUILayout.EnumPopup(gameConfig.linuxDataLocation);
            GUILayout.EndHorizontal();

            /*GUILayout.BeginHorizontal();
            GUILayout.Label("Screenshot Save location:", GUILayout.Width(labelWidth));
            gameConfig.ScreenShotSaveLocation = (KS_FileHelper.ScreenShotSaveLocation)EditorGUILayout.EnumPopup(gameConfig.ScreenShotSaveLocation);
            GUILayout.EndHorizontal();*/

            GUIDrawHeader("Configs:");

            GUILayout.BeginHorizontal();
            GUILayout.Label("Settings Config Name", GUILayout.Width(labelWidth));
            gameConfig.SettingsConfigName = EditorGUILayout.TextField(gameConfig.SettingsConfigName);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Input Config Name", GUILayout.Width(labelWidth));
            gameConfig.i_configName = EditorGUILayout.TextField(gameConfig.i_configName);
            GUILayout.EndHorizontal();

            GUIDrawHeader("Localisation:");

            GUILayout.BeginHorizontal();
            GUILayout.Label("Return Not found:", GUILayout.Width(labelWidth));
            gameConfig.loc_returnNotFound = EditorGUILayout.Toggle(gameConfig.loc_returnNotFound);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Not found line:", GUILayout.Width(labelWidth));
            gameConfig.loc_NotFoundLine = EditorGUILayout.TextField(gameConfig.loc_NotFoundLine);
            GUILayout.EndHorizontal();

            GUIDrawHeader("Pool Manager:");
            GUILayout.BeginHorizontal();
            GUILayout.Label("Clear pool on new level:", GUILayout.Width(labelWidth));
            gameConfig.pool_ClearOnLoadLevel = EditorGUILayout.Toggle(gameConfig.pool_ClearOnLoadLevel);
            GUILayout.EndHorizontal();

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        private void GUIDrawHeader(string name)
        {
            EditorStyles.label.fontStyle = FontStyle.Bold;
            GUILayout.Space(10);
            EditorGUILayout.LabelField(name);
            GUILayout.Space(15);
            EditorStyles.label.fontStyle = FontStyle.Normal;
        }
    }
}