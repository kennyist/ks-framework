using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KS_Core.Handlers;

namespace KS_Core.Input
{

    public class KS_Input : MonoBehaviour
    {

        public static event VoidHandler OnSwapController;
        public static event VoidHandler OnSwapKeyboardMouse;

        private static KS_Input instance;
        public static KS_Input Instance
        {
            get
            {
                return instance;
            }
        }

        public KS_Scriptable_GameConfig gameConfig;
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

        private static KS_Scriptable_Input_object findInput(string ID)
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

        public static bool GetInput(string ID)
        {
            KS_Scriptable_Input_object key = findInput(ID);

            Debug.Log(ID);

            if (key != null)
            {
                Debug.Log(ID + " Found: " + key.curKey.ToString());
                return UnityEngine.Input.GetKey(key.curKey);
            }

            return false;
        }

        public static bool GetInputDown(string ID)
        {
            KS_Scriptable_Input_object key = findInput(ID);

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

        public static bool GetInputUp(string ID)
        {
            KS_Scriptable_Input_object key = findInput(ID);

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

        public static bool GetKey(KeyCode keyCode)
        {
            return UnityEngine.Input.GetKey(keyCode);
        }

        public static bool GetKeyUp(KeyCode keyCode)
        {
            return UnityEngine.Input.GetKeyUp(keyCode);
        }

        public static bool GetKeyDown(KeyCode keyCode)
        {
            return UnityEngine.Input.GetKeyDown(keyCode);
        }

        public static float GetAxis(string ID)
        {
            KS_Scriptable_Input_object key = findInput(ID);

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

        public static KeyCode DS4ButtonToKey(DS4KeyCode key)
        {
            switch (key)
            {
                case DS4KeyCode.Square:
                    return KeyCode.JoystickButton0;
                    break;

                case DS4KeyCode.X:
                    return KeyCode.JoystickButton1;
                    break;

                case DS4KeyCode.Circle:
                    return KeyCode.JoystickButton2;
                    break;

                case DS4KeyCode.Triangle:
                    return KeyCode.JoystickButton3;
                    break;

                case DS4KeyCode.L1:
                    return KeyCode.JoystickButton4;
                    break;

                case DS4KeyCode.R1:
                    return KeyCode.JoystickButton5;
                    break;

                case DS4KeyCode.L2:
                    return KeyCode.JoystickButton6;
                    break;

                case DS4KeyCode.R2:
                    return KeyCode.JoystickButton7;
                    break;

                case DS4KeyCode.Share:
                    return KeyCode.JoystickButton8;
                    break;

                case DS4KeyCode.Options:
                    return KeyCode.JoystickButton9;
                    break;

                case DS4KeyCode.L3:
                    return KeyCode.JoystickButton10;
                    break;

                case DS4KeyCode.R3:
                    return KeyCode.JoystickButton11;
                    break;

                case DS4KeyCode.Home:
                    return KeyCode.JoystickButton12;
                    break;

                case DS4KeyCode.TouchPad:
                    return KeyCode.JoystickButton13;
                    break;
            }

            return KeyCode.Joystick1Button0;
        }

        public static float GetDS4Axis(DS4Axis axis)
        {
            switch (axis)
            {
                case DS4Axis.LeftStickX:
                    return UnityEngine.Input.GetAxis("DS4 X-Axis");
                    break;

                case DS4Axis.LeftStickY:
                    return UnityEngine.Input.GetAxis("DS4 Y-Axis");
                    break;

                case DS4Axis.RightStickX:
                    return UnityEngine.Input.GetAxis("DS4 3rd Axis");
                    break;

                case DS4Axis.RightStickY:
                    //return Input.GetAxis("DS4 4th Axis");
                    return UnityEngine.Input.GetAxis("DS4 6th Axis");
                    break;

                case DS4Axis.L2:
                    return UnityEngine.Input.GetAxis("DS4 4th Axis");
                    //return Input.GetAxis("DS4 5th Axis");
                    break;

                case DS4Axis.R2:
                    return UnityEngine.Input.GetAxis("DS4 5th Axis");
                    //return Input.GetAxis("DS4 6th Axis");
                    break;

                case DS4Axis.DPadX:
                    return UnityEngine.Input.GetAxis("DS4 7th Axis");
                    break;

                case DS4Axis.DPadY:
                    return UnityEngine.Input.GetAxis("DS4 8th Axis");
                    break;
            }

            return 0f;

        }


        public static KeyCode XboxButtonToKeyCode(XboxKeyCode key)
        {
            switch (key)
            {
                case XboxKeyCode.A:
                    return KeyCode.JoystickButton0;
                    break;

                case XboxKeyCode.B:
                    return KeyCode.JoystickButton1;
                    break;

                case XboxKeyCode.X:
                    return KeyCode.JoystickButton2;
                    break;

                case XboxKeyCode.Y:
                    return KeyCode.JoystickButton3;
                    break;

                case XboxKeyCode.LB:
                    return KeyCode.JoystickButton4;
                    break;

                case XboxKeyCode.RB:
                    return KeyCode.JoystickButton5;
                    break;

                case XboxKeyCode.Back:
                    return KeyCode.JoystickButton6;
                    break;

                case XboxKeyCode.Start:
                    return KeyCode.JoystickButton7;
                    break;

                case XboxKeyCode.LeftStickCLick:
                    return KeyCode.JoystickButton8;
                    break;

                default:
                    return KeyCode.A;
                    break;

            }
        }

    }
}