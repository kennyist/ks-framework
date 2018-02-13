using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.UI;

[AddComponentMenu("UI/Text Translate")]
[ExecuteInEditMode]
public class KS_TextTranslate : Text
{
    public string lineId = null;

    void Awake()
    {
        Refresh();
    }

    public void Update()
    {
        Refresh();
    }

    public void Refresh()
    {
        Debug.Log(lineId);
        if (lineId.Length > 0 && lineId != null)
        {
            text = KS_Localisation.Instance.GetLine(lineId).Replace("\\n", "\n");
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
        base.OnInspectorGUI();
    }
}

