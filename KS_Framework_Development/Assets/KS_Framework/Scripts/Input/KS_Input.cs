using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KS_Core.Handlers;
using KS_Core.Parsers;

namespace KS_Core.Input
{
    /// <summary>
    /// Wrapper class over Unitys input class. This allows the editing of inputs during run time and saving any edits to config file.
    /// </summary>
    public class KS_Input : MonoBehaviour
    {

        private static KS_Input instance;
        /// <summary>
        /// Current active instance of KS_Input
        /// </summary>
        public static KS_Input Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Game config <see cref="KS_Scriptable_GameConfig"/>
        /// </summary>
        public KS_Scriptable_GameConfig gameConfig;
        /// <summary>
        /// Game Input config <see cref="KS_Scriptable_Input"/>
        /// </summary>
        public KS_Scriptable_Input inputFile;
        public bool _temp_UseDS4 = true;

        private static KS_Scriptable_Input inputConfig;
        private static KS_IniParser parser;

        // Use this for initialization
        void Start()
        {
            if (instance != null) Destroy(this);
            instance = this;

            inputConfig = inputFile;

            parser = new KS_IniParser();

            if (!parser.DoesExist(gameConfig.i_configName))
            {
                PopulateWithDefaults();
                parser.Save(gameConfig.i_configName);
            }

            parser.Load(gameConfig.i_configName);
            CheckInputs();
            parser.Save(gameConfig.i_configName);

            for (int i = 0; i < UnityEngine.Input.GetJoystickNames().Length; i++)
            {
                Debug.Log("Joystick " + i + ": " + UnityEngine.Input.GetJoystickNames()[i]);
            }
        }

        private void PopulateWithDefaults()
        {
            if (inputConfig.Inputs.Count > 0)
            {
                foreach (KS_Scriptable_Input_object input in inputConfig.Inputs)
                {
                    if (input.EditableInGame)
                    {
                        switch (input.type)
                        {
                            case KS_Scriptable_input_type.Keyboard:
                                parser.Set("Keyboard", input.ID, input.DefaultKey.ToString(), input.ConfigHelpText);

                                break;

                            case KS_Scriptable_input_type.Mouse:
                                parser.Set("Mouse", input.ID, input.MouseButton.ToString(), input.ConfigHelpText);
                                break;

                            case KS_Scriptable_input_type.Axis:
                                parser.Set("Axis", input.ID + "_positive", input.positive.ToString(), input.ConfigHelpText);
                                if (input.negitive != KeyCode.None)
                                {
                                    parser.Set("Axis", input.ID + "_negative", input.negitive.ToString());
                                }

                                if (input.UseDS4)
                                {
                                    parser.Set("Axis", input.ID + "_DS4", input.DS4Axis.ToString());
                                }

                                if (input.UseXbox)
                                {
                                    parser.Set("Axis", input.ID + "_XBX", input.XboxAxis.ToString());
                                }

                                if (input.UseDS4 || input.UseDS4)
                                {
                                    parser.Set("Axis", input.ID + "_deadzone", input.deadZone.ToString());
                                }
                                break;
                        }
                    }
                }
            }
        }

        private void CheckInputs()
        {
            if (inputConfig.Inputs.Count > 0)
            {
                foreach (KS_Scriptable_Input_object input in inputConfig.Inputs)
                {
                    switch (input.type)
                    {
                        case KS_Scriptable_input_type.Keyboard:
                            CheckInputs_Keyboard(input);
                            break;

                        case KS_Scriptable_input_type.Mouse:
                            CheckInputs_Mouse(input);
                            break;
                    }
                }
            }
        }

        private void CheckInputs_Keyboard(KS_Scriptable_Input_object input)
        {
            if (Enum.IsDefined(typeof(KeyCode), parser.Get(input.ID)))
            {
                input.curKey = (KeyCode)Enum.Parse(typeof(KeyCode), parser.Get(input.ID));
            }
            else
            {
                parser.Set("Keyboard", input.ID, input.DefaultKey.ToString());
                input.curKey = input.DefaultKey;
            }
        }

        private void CheckInputs_Mouse(KS_Scriptable_Input_object input)
        {
            int i;

            if (int.TryParse(parser.Get(input.ID), out i))
            {
                input.curMouseButton = i;
            }
            else
            {
                parser.Set("Mouse", input.ID, input.MouseButton.ToString());
                input.curMouseButton = input.MouseButton;
            }
        }

        private static KS_Scriptable_Input_object FindInput(string ID)
        {
            if (inputConfig == null || inputConfig.Inputs == null) return null;

            if (inputConfig.Inputs.Count > 0)
            {
                for (int i = 0; i < inputConfig.Inputs.Count; i++)
                {
                    if (inputConfig.Inputs[i].ID == ID)
                    {
                        return inputConfig.Inputs[i];
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Is input currently being pressed
        /// </summary>
        /// <param name="ID">Input ID as set in Input Manager editor window</param>
        /// <returns></returns>
        public static bool GetInput(string ID)
        {
            KS_Scriptable_Input_object key = FindInput(ID);

            Debug.Log(ID);

            if (key != null)
            {
                Debug.Log(ID + " Found: " + key.curKey.ToString());
                return UnityEngine.Input.GetKey(key.curKey);
            }

            return false;
        }

        /// <summary>
        /// Is input currently being pressed down
        /// </summary>
        /// <param name="ID">Input ID as set in Input Manager editor window</param>
        /// <returns>True the moment the input is pressed down else False</returns>
        public static bool GetInputDown(string ID)
        {
            KS_Scriptable_Input_object key = FindInput(ID);

            if (key != null)
            {
                if (key.UseDS4)
                {
                    if (UnityEngine.Input.GetKeyDown(DS4ButtonToKey(key.DefaultDS4)))
                    {
                        return true;
                    }
                }

                return UnityEngine.Input.GetKeyDown(key.curKey);
            }

            return false;
        }

        /// <summary>
        /// Is input currently being pressed up
        /// </summary>
        /// <param name="ID">Input ID as set in Input Manager editor window</param>
        /// <returns>True the moment the input is released else False</returns>
        public static bool GetInputUp(string ID)
        {
            KS_Scriptable_Input_object key = FindInput(ID);

            if (key != null)
            {
                if (key.UseDS4)
                {
                    if (UnityEngine.Input.GetKeyUp(DS4ButtonToKey(key.DefaultDS4)))
                    {
                        return true;
                    }
                }

                return UnityEngine.Input.GetKeyUp(key.curKey);
            }

            return false;
        }

        /// <summary>
        /// Is key currently being pressed
        /// </summary>
        /// <param name="keyCode">Unity keycode</param>
        /// <returns>True while the key is pressed down else False</returns>
        public static bool GetKey(KeyCode keyCode)
        {
            return UnityEngine.Input.GetKey(keyCode);
        }

        /// <summary>
        /// Is key currently being pressed up
        /// </summary>
        /// <param name="keyCode">Unity keycode</param>
        /// <returns>True the moment the key is released else False</returns>
        public static bool GetKeyUp(KeyCode keyCode)
        {
            return UnityEngine.Input.GetKeyUp(keyCode);
        }

        /// <summary>
        /// Is input currently being pressed down
        /// </summary>
        /// <param name="keyCode">Unity keycode</param>
        /// <returns>True the moment the key is pressed else False</returns>
        public static bool GetKeyDown(KeyCode keyCode)
        {
            return UnityEngine.Input.GetKeyDown(keyCode);
        }

        /// <summary>
        /// Get the current readings for an Axis
        /// </summary>
        /// <param name="ID">Axis ID as set in Input Manager editor window</param>
        /// <returns>Float axis reading from -1 to 1, if above 1 its mouse reading</returns>
        public static float GetAxis(string ID)
        {
            KS_Scriptable_Input_object key = FindInput(ID);

            if (key != null)
            {
                float DS4 = 0, xbox = 0, mouse = 0, keyboard = 0;
                float axisPos = 0, axisNeg = 0;

                if (key.UseDS4)
                {
                    DS4 = GetDS4Axis(key.DS4Axis);

                    if (DS4 > 0 && DS4 < key.deadZone) DS4 = 0;
                    if (DS4 < 0 && DS4 > -key.deadZone) DS4 = 0;

                    if (DS4 > 0)
                    {
                        axisPos = DS4;
                    }
                    else
                    {
                        axisNeg = DS4;
                    }
                }

                if (key.UseXbox)
                {
                    xbox = 0f;

                    if (xbox > 0 && DS4 < key.deadZone) xbox = 0;
                    if (xbox < 0 && DS4 > -key.deadZone) xbox = 0;
                }

                if (key.mouseX)
                {
                    mouse = UnityEngine.Input.GetAxis("Mouse X");

                    if (mouse < axisNeg)
                    {
                        axisNeg = mouse;
                    }
                    else if (mouse > axisPos)
                    {
                        axisPos = mouse;
                    }
                }

                if (key.mouseY)
                {
                    mouse = UnityEngine.Input.GetAxis("Mouse Y");

                    if (mouse < axisNeg)
                    {
                        axisNeg = mouse;
                    }
                    else if (mouse > axisPos)
                    {
                        axisPos = mouse;
                    }
                }

                if (key.positive != KeyCode.None)
                {
                    if (UnityEngine.Input.GetKey(key.positive)) keyboard = 1f;
                    if (UnityEngine.Input.GetKey(key.negitive))
                    {
                        if (keyboard > 0) keyboard = 0f;
                        else keyboard = -1f;
                    }

                    if (keyboard < axisNeg)
                    {
                        axisNeg = keyboard;
                    }
                    else if (keyboard > axisPos)
                    {
                        axisPos = keyboard;
                    }
                }

                if (axisNeg != 0)
                {
                    return axisNeg += axisPos;
                }
                else
                {
                    return axisPos;
                }
            }

            return 0f;
        }

        /// <summary>
        /// Convert DS4keyCode to Unity key code 
        /// </summary>
        /// <param name="key">DS4 Key Code <see cref="DS4KeyCode"/></param>
        /// <returns>returns joystick keycode</returns>
        public static KeyCode DS4ButtonToKey(DS4KeyCode key)
        {
            switch (key)
            {
                case DS4KeyCode.Square:
                    return KeyCode.JoystickButton0;

                case DS4KeyCode.X:
                    return KeyCode.JoystickButton1;

                case DS4KeyCode.Circle:
                    return KeyCode.JoystickButton2;

                case DS4KeyCode.Triangle:
                    return KeyCode.JoystickButton3;

                case DS4KeyCode.L1:
                    return KeyCode.JoystickButton4;

                case DS4KeyCode.R1:
                    return KeyCode.JoystickButton5;

                case DS4KeyCode.L2:
                    return KeyCode.JoystickButton6;

                case DS4KeyCode.R2:
                    return KeyCode.JoystickButton7;

                case DS4KeyCode.Share:
                    return KeyCode.JoystickButton8;

                case DS4KeyCode.Options:
                    return KeyCode.JoystickButton9;

                case DS4KeyCode.L3:
                    return KeyCode.JoystickButton10;

                case DS4KeyCode.R3:
                    return KeyCode.JoystickButton11;

                case DS4KeyCode.Home:
                    return KeyCode.JoystickButton12;

                case DS4KeyCode.TouchPad:
                    return KeyCode.JoystickButton13;
            }

            return KeyCode.Joystick1Button0;
        }

        /// <summary>
        /// Get Ds4 axis readings 
        /// </summary>
        /// <param name="axis">DS4 axis <see cref="DS4Axis"/></param>
        /// <returns>Axis reading from -1f to 1f</returns>
        public static float GetDS4Axis(DS4Axis axis)
        {
            switch (axis)
            {
                case DS4Axis.LeftStickX:
                    return UnityEngine.Input.GetAxis("DS4 X-Axis");

                case DS4Axis.LeftStickY:
                    return UnityEngine.Input.GetAxis("DS4 Y-Axis");

                case DS4Axis.RightStickX:
                    return UnityEngine.Input.GetAxis("DS4 3rd Axis");

                case DS4Axis.RightStickY:
                    //return Input.GetAxis("DS4 4th Axis");
                    return UnityEngine.Input.GetAxis("DS4 6th Axis");

                case DS4Axis.L2:
                    return UnityEngine.Input.GetAxis("DS4 4th Axis");
                    //return Input.GetAxis("DS4 5th Axis");

                case DS4Axis.R2:
                    return UnityEngine.Input.GetAxis("DS4 5th Axis");
                    //return Input.GetAxis("DS4 6th Axis");

                case DS4Axis.DPadX:
                    return UnityEngine.Input.GetAxis("DS4 7th Axis");

                case DS4Axis.DPadY:
                    return UnityEngine.Input.GetAxis("DS4 8th Axis");
            }

            return 0f;

        }

        /// <summary>
        /// Convert Xbox key code to Unity Keycode
        /// </summary>
        /// <param name="key">Xbox key code <see cref="XboxKeyCode"/></param>
        /// <returns>Unity joystick keycode</returns>
        public static KeyCode XboxButtonToKeyCode(XboxKeyCode key)
        {
            switch (key)
            {
                case XboxKeyCode.A:
                    return KeyCode.JoystickButton0;

                case XboxKeyCode.B:
                    return KeyCode.JoystickButton1;

                case XboxKeyCode.X:
                    return KeyCode.JoystickButton2;

                case XboxKeyCode.Y:
                    return KeyCode.JoystickButton3;

                case XboxKeyCode.LB:
                    return KeyCode.JoystickButton4;

                case XboxKeyCode.RB:
                    return KeyCode.JoystickButton5;

                case XboxKeyCode.Back:
                    return KeyCode.JoystickButton6;

                case XboxKeyCode.Start:
                    return KeyCode.JoystickButton7;

                case XboxKeyCode.LeftStickCLick:
                    return KeyCode.JoystickButton8;

                default:
                    return KeyCode.A;

            }
        }

    }
}