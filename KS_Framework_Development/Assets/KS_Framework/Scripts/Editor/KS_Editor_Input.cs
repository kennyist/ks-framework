using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using KS_Core.Input;

namespace KS_Core.Editor
{
    /// <summary>
    /// KS input manager editor window: Create, edit or delete inputs.
    /// <see cref="KS_Scriptable_Input"/>
    /// </summary>
    public class KS_Editor_Input : EditorWindow
    {

        private static string ConfigSaveFile = "Assets/KS_Framework/KS_Data/Input.asset";
        private bool configFound = false;

        private KS_Scriptable_Input inputConfig;
        private bool addInput = false;

        string searchString = "";
        string lastSearchString = "";
        bool isSearching = false;
        string addbox_string = "";
        bool addbox = false;
        bool addbox_error = false;

        [MenuItem("KS: Framework/Input Manager", false, 23)]
        static void Init()
        {
            KS_Editor_Input window = (KS_Editor_Input)EditorWindow.GetWindow(typeof(KS_Editor_Input));
            window.titleContent.text = "Input Manager";
            window.Show();
        }

        private void OnEnable()
        {
            inputConfig = FindGameConfig();
            if (inputConfig == null) configFound = false;
            else configFound = true;
        }

        private static KS_Scriptable_Input FindGameConfig()
        {
            KS_Scriptable_Input gameConfig = AssetDatabase.LoadAssetAtPath(ConfigSaveFile, typeof(KS_Scriptable_Input)) as KS_Scriptable_Input;

            if (gameConfig) return gameConfig;
            return null;
        }

        void OpenDatabase()
        {
            string absPath = EditorUtility.OpenFilePanel("Select Input Database", "", "");
            if (absPath.StartsWith(Application.dataPath))
            {
                string relPath = absPath.Substring(Application.dataPath.Length - "Assets".Length);
                inputConfig = AssetDatabase.LoadAssetAtPath(relPath, typeof(KS_Scriptable_Input)) as KS_Scriptable_Input;

                if (inputConfig)
                {
                    EditorPrefs.SetString("ObjectPath", relPath);
                }
            }

            selected = null;
        }

        private void CreateDatabase()
        {
            string absPath = EditorUtility.SaveFilePanel("Create Translation Database", "", "Input", "asset");
            KS_Scriptable_Input gameConfig;
            if (absPath.StartsWith(Application.dataPath))
            {
                gameConfig = ScriptableObject.CreateInstance<KS_Scriptable_Input>();

                absPath = absPath.Replace(Application.dataPath, "");
                absPath = "Assets" + absPath;

                AssetDatabase.CreateAsset(gameConfig, absPath);
                AssetDatabase.SaveAssets();
                gameConfig = AssetDatabase.LoadAssetAtPath(absPath, typeof(KS_Scriptable_Input)) as KS_Scriptable_Input;
            }

            selected = null;
        }

        private void SaveDatabase()
        {
            if (inputConfig)
            {
                EditorUtility.SetDirty(inputConfig);
                AssetDatabase.SaveAssets();
            }
        }

        // GUI

        private void OnGUI()
        {
            if(searchString != lastSearchString)
            {
                lastSearchString = searchString;

                if (string.IsNullOrEmpty(searchString))
                {
                    isSearching = false;
                } 
                else if (!isSearching)
                {
                    isSearching = true;
                }
            }


            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            DrawToolBar();
            GUILayout.EndHorizontal();

            if (!configFound)
            {
                return;
            }

            EditorStyles.miniButtonLeft.alignment = TextAnchor.MiddleLeft;

            GUILayout.BeginHorizontal();
            GUIDrawInputMenu();

            if (addbox)
            {
                GUIDrawAddBox();
            }
            else
            {
                GUIDrawConfigEditor();
            }
            GUILayout.EndHorizontal();
        }

        void DrawToolBar()
        {
            if (!inputConfig)
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

                if (inputConfig)
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


                if (GUILayout.Button("Add input", EditorStyles.toolbarButton))
                {
                    addbox = true;
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


        private Vector2 keyScroll = new Vector2();

        private void GUIDrawInputMenu()
        {
            GUILayout.BeginVertical(GUILayout.Width(200));

            GUILayout.Label("Inputs:");

            keyScroll = GUILayout.BeginScrollView(keyScroll);

            string[] keys = GetAllKeys();

            for (int i = 0; i < keys.Length; i++)
            {
                if (GUILayout.Button(keys[i], EditorStyles.miniButtonLeft))
                {
                    ChangeSelectedInput(keys[i]);
                }
            }

            GUILayout.EndScrollView();

            GUILayout.EndVertical();
        }

        private string[] GetAllKeys()
        {
            List<string> keys = new List<string>();

            if(inputConfig.Inputs.Count <= 0)
            {
                return keys.ToArray();
            }

            for(int i = 0; i < inputConfig.Inputs.Count; i++)
            {
                keys.Add(inputConfig.Inputs[i].ID);
            }

            if (isSearching)
            {
                keys = keys.FindAll(s => s.Contains(searchString));
            }

            keys.Sort();

            return keys.ToArray();
        }

        private void ChangeSelectedInput(string ID)
        {
            if (inputConfig.Inputs.Count <= 0 || ID == null)
            {
                selected = null;
                return;
            }

            foreach (KS_Scriptable_Input_object input in inputConfig.Inputs)
            {
                Debug.Log(ID + " - " + input.ID);
                if (input.ID == ID)
                {
                    selected = input;
                    return;
                }
            }

            selected = null;
        }

        private KS_Scriptable_Input_object selected = null;

        private void GUIDrawConfigEditor()
        {
            GUILayout.BeginVertical();

            if (selected == null)
            {
                GUILayout.Label("No input selected, Select one of the left side menu or create new one");
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Current Input: " + selected.ID);
                EditorGUILayout.Separator();
                if (GUILayout.Button("Delete Input", EditorStyles.miniButton))
                {
                    Delete(selected.ID);
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(20);

                GUILayout.BeginHorizontal();
                GUILayout.Label("Input type:", GUIStyleLabel());
                selected.type = (KS_Scriptable_input_type)EditorGUILayout.EnumPopup(selected.type);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Editable:", GUIStyleLabel());
                selected.EditableInGame = GUILayout.Toggle(selected.EditableInGame, "Is editable in game and save to config (false = always use default)");
                GUILayout.EndHorizontal();

                if (selected.EditableInGame)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Config Help Text:", GUIStyleLabel());
                    selected.ConfigHelpText = EditorGUILayout.TextArea(selected.ConfigHelpText);
                    GUILayout.EndHorizontal();
                }

                switch (selected.type)
                {
                    case KS_Scriptable_input_type.Keyboard:
                        GUIDrawKeyboardItem(selected);
                        break;

                    case KS_Scriptable_input_type.Mouse:
                        GUIDrawMouseItem(selected);
                        break;

                    case KS_Scriptable_input_type.Axis:
                        GUIDrawAxisItem(selected);
                        break;
                }
            }
            GUILayout.EndVertical();
        }

        GUIStyle GUIStyleLabel()
        {
            GUIStyle style = EditorStyles.label;
            style.fixedWidth = 100;

            return style;
        }

        void GUIDrawKeyboardItem(KS_Scriptable_Input_object input)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Input key:", GUIStyleLabel());
            selected.DefaultKey = (KeyCode)EditorGUILayout.EnumPopup(selected.DefaultKey);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Use DS4:", GUIStyleLabel());
            selected.UseDS4 = GUILayout.Toggle(selected.UseDS4, " Enable Dual Shock 4 support on this input");
            GUILayout.EndHorizontal();

            if (selected.UseDS4)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("DS4 Button:", GUIStyleLabel());
                selected.DefaultDS4 = (DS4KeyCode)EditorGUILayout.EnumPopup(selected.DefaultDS4);
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label("Use Xbox:", GUIStyleLabel());
            selected.UseXbox = GUILayout.Toggle(selected.UseXbox, " Enable xbox controller support on this input");
            GUILayout.EndHorizontal();

            if (selected.UseXbox)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Xbox Button:", GUIStyleLabel());
                selected.DefaultXbox = (XboxKeyCode)EditorGUILayout.EnumPopup(selected.DefaultXbox);
                GUILayout.EndHorizontal();
            }
        }

        void GUIDrawMouseItem(KS_Scriptable_Input_object input)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Mouse button:", GUIStyleLabel());
            selected.MouseButton = EditorGUILayout.IntField(selected.MouseButton);
            GUILayout.EndHorizontal();
        }

        void GUIDrawAxisItem(KS_Scriptable_Input_object input)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Positive Key:", GUIStyleLabel());
            selected.positive = (KeyCode)EditorGUILayout.EnumPopup(selected.positive);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Negative Key:", GUIStyleLabel());
            selected.negitive = (KeyCode)EditorGUILayout.EnumPopup(selected.negitive);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Mouse X:", GUIStyleLabel());
            selected.mouseX = GUILayout.Toggle(selected.mouseX, " Read Mouse X axis");
            GUILayout.EndHorizontal();

            if (selected.mouseX) selected.mouseY = false;

            GUILayout.BeginHorizontal();
            GUILayout.Label("Mouse Y:", GUIStyleLabel());
            selected.mouseY = GUILayout.Toggle(selected.mouseY, " Read Mouse Y axis");
            GUILayout.EndHorizontal();

            if (selected.mouseY) selected.mouseX = false;

            GUILayout.BeginHorizontal();
            GUILayout.Label("Use DS4:", GUIStyleLabel());
            selected.UseDS4 = GUILayout.Toggle(selected.UseDS4, " Enable Dual Shock 4 support on this input");
            GUILayout.EndHorizontal();

            if (selected.UseDS4)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("DS4 Axis:", GUIStyleLabel());
                selected.DS4Axis = (DS4Axis)EditorGUILayout.EnumPopup(selected.DS4Axis);
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label("Use Xbox:", GUIStyleLabel());
            selected.UseXbox = GUILayout.Toggle(selected.UseXbox, " Enable xbox controller support on this input");
            GUILayout.EndHorizontal();

            if (selected.UseXbox)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Xbox Axis:", GUIStyleLabel());
                selected.XboxAxis = (XboxAxis)EditorGUILayout.EnumPopup(selected.XboxAxis);
                GUILayout.EndHorizontal();
            }

            if (selected.UseDS4 || selected.UseXbox)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Controller Axis Deadzone:", GUIStyleLabel());
                selected.deadZone = EditorGUILayout.FloatField(selected.deadZone);
                GUILayout.EndHorizontal();
            }
        }

        bool DoesKeyExist(string key)
        {
            string[] keys = GetAllKeys();

            for(int i = 0; i < keys.Length; i++)
            {
                if (keys[i].Equals(key))
                {
                    return true;
                }
            }

            return false;
        }

        private void GUIDrawAddBox()
        {
            GUILayout.BeginVertical();
            GUILayout.Label("Add new input:");
            addbox_string = GUILayout.TextField(addbox_string);
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Add"))
            {
                if (DoesKeyExist(addbox_string))
                {
                    addbox_error = true;
                }
                else
                {
                    AddInput(addbox_string);
                    addbox_error = false;
                    addbox_string = "";
                    addbox = false;
                }
            }
            if (GUILayout.Button("Cancle"))
            {
                addbox_error = false;
                addbox_string = "";
                addbox = false;
            }
            GUILayout.EndHorizontal();

            if (addbox_error)
            {
                GUILayout.Label("Input ID already exists in the database.");
            }

            GUILayout.EndVertical();
        }

        void AddInput(string ID)
        {
            if (DoesKeyExist(ID)) { ChangeSelectedInput(ID); return; }

            KS_Scriptable_Input_object input = new KS_Scriptable_Input_object();
            input.ID = ID.ToLower();

            inputConfig.Inputs.Add(input);

            ChangeSelectedInput(ID);
        }

        void Delete(string ID)
        {
            for(int i = 0; i < inputConfig.Inputs.Count; i++)
            {
                if(inputConfig.Inputs[i].ID == ID)
                {
                    inputConfig.Inputs.RemoveAt(i);
                    selected = null;
                    return;
                }
            }
        }
    }
}
