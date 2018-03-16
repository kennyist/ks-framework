using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class KS_Editor_Input : EditorWindow {

    private static string ConfigSaveFile = "Assets/KS_Data/Input.asset";
    private bool configFound = false;

    private KS_Scriptable_Input inputConfig;

    [MenuItem("KS: OWGF/Input Manager", false, 23)]
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

    private static KS_Scriptable_Input CreateConfig()
    {
        KS_Scriptable_Input gameConfig = ScriptableObject.CreateInstance<KS_Scriptable_Input>();

        AssetDatabase.CreateAsset(gameConfig, ConfigSaveFile);
        AssetDatabase.SaveAssets();
        gameConfig = AssetDatabase.LoadAssetAtPath(ConfigSaveFile, typeof(KS_Scriptable_Input)) as KS_Scriptable_Input;

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

        EditorStyles.miniButtonLeft.alignment = TextAnchor.MiddleLeft;

        GUILayout.BeginHorizontal();
        GUIDrawInputMenu();
        GUIDrawConfigEditor();
        GUILayout.EndHorizontal();
    }

    private void GUIDrawCreateConfig()
    {
        GUILayout.BeginVertical();
        GUILayout.Label("Game config not found");
        if (GUILayout.Button("Create Config"))
        {
            CreateConfig();
        }
        GUILayout.EndVertical();
    }

    private Vector2 keyScroll = new Vector2();

    private void GUIDrawInputMenu()
    {
        GUILayout.BeginVertical(GUILayout.Width(200));

        GUILayout.Label("Inputs:");

        keyScroll = GUILayout.BeginScrollView(keyScroll);

        for(int i = 0; i < inputConfig.Inputs.Count; i++)
        {
            if (GUILayout.Button(inputConfig.Inputs[i].ID, EditorStyles.miniButtonLeft))
            {
                ChangeSelectedInput(inputConfig.Inputs[i].ID);
            }
        }

        GUILayout.EndScrollView();

        if (GUILayout.Button("Add"))
        {

        }

        GUILayout.EndVertical();
    }

    private void ChangeSelectedInput(string ID)
    {
        if (inputConfig.Inputs.Count <= 0) return;

        foreach(KS_Scriptable_Input_object input in inputConfig.Inputs)
        {
            Debug.Log(ID + " - " + input.ID);
            if(input.ID == ID)
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

        if(selected == null)
        {
            GUILayout.Label("No input selected, Select one of the left side menu or create new one");
        }
        else
        {
            GUILayout.Label("Current Input: "+ selected.ID);
            GUILayout.Space(20);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Input type:", GUIStyleLabel());
            selected.type = (KS_Scriptable_input_type)EditorGUILayout.EnumPopup(selected.type);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Editable:", GUIStyleLabel());
            selected.EditableInGame = GUILayout.Toggle(selected.EditableInGame, "Is editable in game and save to config (false = always use default)");
            GUILayout.EndHorizontal();

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
            selected.DefaultXbox = (XboxKeyCode) EditorGUILayout.EnumPopup(selected.DefaultXbox);
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
        selected.positive = (KeyCode) EditorGUILayout.EnumPopup(selected.positive);
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
}
