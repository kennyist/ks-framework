using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using KS_Core.Localisation;

public class KS_TextTranslate_Editor : MonoBehaviour {

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

    [CustomEditor(typeof(KS_TextTranslate))]
    public class TextTranslateEditor : Editor
    {
        public override void OnInspectorGUI()
        {

            KS_TextTranslate t = (KS_TextTranslate)target;
            t.lineId = EditorGUILayout.TextField("Localisation Line ID:", t.lineId);
            base.OnInspectorGUI();
        }
    }
}
