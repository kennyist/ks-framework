﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using KS_Core.Localisation;

[AddComponentMenu("UI/Text Translate")]
public class KS_TextTranslate : Text
{
    public string lineId = "";
    private string lastID = null;

    void Awake()
    {
        Refresh();
    }

    protected override void OnEnable()
    {
        KS_Localisation.LanguageChanged += LanguageChanged;
        GetLine();
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        KS_Localisation.LanguageChanged -= LanguageChanged;
        base.OnDisable();
    }

    protected override void Start()
    {
        lastID = lineId;

        GetLine();

        base.Start();
    }

    private void LanguageChanged()
    {
        GetLine();
    }

    private bool GetLine()
    {
        if (lineId.Length > 0 && lineId != null)
        {
            text = KS_Localisation.Instance.GetLine(lineId).Replace("\\n", "\n");

            return true;
        }

        return false;
    }

    public void Update()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (!lineId.Equals(lastID))
        {
            if (!GetLine())
            {

                text = "";
            }

        }
    }

#if UNITY_EDITOR
    [MenuItem("GameObject/UI/Text Translate", false, 10)]
    static void CreateCustomGameObject(MenuCommand menuCommand)
    {
        // Create a custom game object
        GameObject go = new GameObject("Text Translate");
        go.AddComponent<KS_TextTranslate>();
        go.GetComponent<KS_TextTranslate>().enabled = true;
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }
#endif
}

[CustomEditor(typeof(KS_TextTranslate))]
public class TextTranslateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        
        KS_TextTranslate t = (KS_TextTranslate)target;
        t.lineId = EditorGUILayout.TextField("Localisation Line ID:",t.lineId);
        base.OnInspectorGUI();
    }
}

