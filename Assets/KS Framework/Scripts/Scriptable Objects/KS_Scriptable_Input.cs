using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Input", menuName = "KS_Framework/Input/Database", order = 1)]
public class KS_Scriptable_Input : ScriptableObject {
    public List<KS_Scriptable_Input_object> Inputs = new List<KS_Scriptable_Input_object>();
}

[System.Serializable]
public class KS_Scriptable_Input_object
{ 
    public string ID;
    public KS_Scriptable_input_type type;
    public bool EditableInGame = true;

    //Keyboard
    public KeyCode DefaultKey;
    public KeyCode curKey;

    public bool UseDS4 = false;
    public DS4KeyCode DefaultDS4 = DS4KeyCode.X;
    public bool UseXbox = false;

    //Mouse
    public int MouseButton = 0;
    public int curMouseButton = 0;
}

[System.Serializable]
public enum KS_Scriptable_input_type
{
    Keyboard,
    Mouse,
    Axis
}
