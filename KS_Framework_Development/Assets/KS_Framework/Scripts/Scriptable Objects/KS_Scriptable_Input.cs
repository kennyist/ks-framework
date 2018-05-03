using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS_Core.Input
{

    /// <summary>
    /// Input database
    /// </summary>
    [CreateAssetMenu(fileName = "Input", menuName = "KS: Framework/Databases/Input", order = 3)]
    public class KS_Scriptable_Input : ScriptableObject
    {
        /// <summary>
        /// List of saved inputs
        /// </summary>
        public List<KS_Scriptable_Input_object> Inputs = new List<KS_Scriptable_Input_object>();
    }

    /// <summary>
    /// Input data and type storage
    /// </summary>
    [System.Serializable]
    public class KS_Scriptable_Input_object
    {
        /// <summary>
        /// Unique input name
        /// </summary>
        public string ID;
        /// <summary>
        /// Type of input <see cref="KS_Scriptable_input_type"/>
        /// </summary>
        public KS_Scriptable_input_type type;
        /// <summary>
        /// editable in game? If true, saves to "Input.CFG" by default and is publicly editable
        /// </summary>
        public bool EditableInGame = true;
        /// <summary>
        /// Help text for people editing input.cfg 
        /// </summary>
        public string ConfigHelpText = "";

        //Keyboard
        /// <summary>
        /// Default (Perminant) key
        /// </summary>
        public KeyCode DefaultKey;
        /// <summary>
        /// Current key, Can be changed in game and by editing config
        /// </summary>
        public KeyCode curKey;

        /// <summary>
        /// Allow DS4 input 
        /// </summary>
        public bool UseDS4 = false;
        /// <summary>
        /// Default DS4 button (Perminant)
        /// </summary>
        public DS4KeyCode DefaultDS4;
        /// <summary>
        /// Allow Xbox input
        /// </summary>
        public bool UseXbox = false;
        /// <summary>
        /// Default Xbox button (Perminant)
        /// </summary>
        public XboxKeyCode DefaultXbox;

        //Mouse
        /// <summary>
        /// Mouse button index (Perminant)
        /// </summary>
        public int MouseButton = 0;
        /// <summary>
        /// Current mouse button
        /// </summary>
        public int curMouseButton = 0;

        // Axis
        /// <summary>
        /// Positive key
        /// </summary>
        public KeyCode positive;
        /// <summary>
        /// Negative key
        /// </summary>
        public KeyCode negitive;
        /// <summary>
        /// read mouse horizontal
        /// </summary>
        public bool mouseX;
        /// <summary>
        /// Read mouse vertical
        /// </summary>
        public bool mouseY;
        /// <summary>
        /// Ds4 axis to read
        /// </summary>
        public DS4Axis DS4Axis;
        /// <summary>
        /// xbox Axis to read
        /// </summary>
        public XboxAxis XboxAxis;
        /// <summary>
        /// Controller Deadzone
        /// </summary>
        public float deadZone = 0.10f;
    }

    /// <summary>
    /// KS_Input input types
    /// </summary>
    [System.Serializable]
    public enum KS_Scriptable_input_type
    {
        /// <summary>
        /// Keyboard key input
        /// </summary>
        Keyboard,
        /// <summary>
        /// Mouse input
        /// </summary>
        Mouse,
        /// <summary>
        /// Axis input
        /// </summary>
        Axis
    }
}