using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS_Core.Input
{
    /// <summary>
    /// Sony Dual Shock 4 key codes
    /// </summary>
    public enum DS4KeyCode
    {
        /// <summary>
        /// Square button
        /// </summary>
        Square,
        /// <summary>
        /// Circle Button
        /// </summary>
        Circle,
        /// <summary>
        /// Triangle button
        /// </summary>
        Triangle,
        /// <summary>
        /// X button
        /// </summary>
        X,
        /// <summary>
        /// L1 button
        /// </summary>
        L1,
        /// <summary>
        /// L2 button, triggered on full pull
        /// </summary>
        L2,
        /// <summary>
        /// left Stick click button
        /// </summary>
        L3,
        /// <summary>
        /// R1 button
        /// </summary>
        R1,
        /// <summary>
        /// R2 button, triggered on full pull
        /// </summary>
        R2,
        /// <summary>
        /// Right stick click button
        /// </summary>
        R3,
        /// <summary>
        /// Options button
        /// </summary>
        Options,
        /// <summary>
        /// Share button
        /// </summary>
        Share,
        /// <summary>
        /// Hone Button
        /// </summary>
        Home,
        /// <summary>
        /// Touchpad press
        /// </summary>
        TouchPad
    }

    /// <summary>
    /// Sony Dual Shock 4 Axises
    /// </summary>
    public enum DS4Axis
    {
        /// <summary>
        /// Left stick horizontal
        /// </summary>
        LeftStickX,
        /// <summary>
        /// Left stick Vertical
        /// </summary>
        LeftStickY,
        /// <summary>
        /// Right stick horizontal
        /// </summary>
        RightStickX,
        /// <summary>
        /// Right stick vertical
        /// </summary>
        RightStickY,
        /// <summary>
        /// Left trigger pull
        /// </summary>
        L2,
        /// <summary>
        /// Right trigger pull
        /// </summary>
        R2,
        /// <summary>
        /// Dpad horizontal
        /// </summary>
        DPadX,
        /// <summary>
        /// Dpad vertical
        /// </summary>
        DPadY,
        /*GyroX, Planned Addition
        GyroY,
        GyroRaw,
        AccelerometerX,
        AccelerometerY,
        AccelerometerRaw*/
    }

    /// <summary>
    /// Microsoft Xbox controller key code
    /// </summary>
    public enum XboxKeyCode
    {
        /// <summary>
        /// A Button
        /// </summary>
        A,
        /// <summary>
        /// B Button
        /// </summary>
        B,
        /// <summary>
        /// X Button
        /// </summary>
        X,
        /// <summary>
        /// Y Button
        /// </summary>
        Y,
        /// <summary>
        /// Left Bumper 
        /// </summary>
        LB,
        /// <summary>
        /// Right Bumper
        /// </summary>
        RB,
        /// <summary>
        /// Left stick click
        /// </summary>
        LeftStickCLick,
        /// <summary>
        /// Right stick click
        /// </summary>
        RightStickClick,
        /// <summary>
        /// Back Button
        /// </summary>
        Back,
        /// <summary>
        /// Start Button
        /// </summary>
        Start,
        /// <summary>
        /// Home Button
        /// </summary>
        Home,
        /// <summary>
        /// Dpad Up
        /// </summary>
        DpadUp,
        /// <summary>
        /// Dpad Left
        /// </summary>
        DpadLeft,
        /// <summary>
        /// Dpad Down
        /// </summary>
        DpadDown,
        /// <summary>
        /// Dpad Right
        /// </summary>
        DpadRight
    }

    /// <summary>
    /// Microsoft Xbox Controller Axises
    /// </summary>
    public enum XboxAxis
    {
        /// <summary>
        /// Left stick horizontal
        /// </summary>
        LeftStickX,
        /// <summary>
        /// Left stick vertical
        /// </summary>
        LeftStickY,
        /// <summary>
        /// Right stick horizontal
        /// </summary>
        RightStickX,
        /// <summary>
        /// Right stick vertical
        /// </summary>
        RightStickY,
        /// <summary>
        /// Dpad horizontal
        /// </summary>
        DpadX,
        /// <summary>
        /// Dpad vertical
        /// </summary>
        DpadY,
        /// <summary>
        /// Left trigger Pull
        /// </summary>
        LeftTrigger,
        /// <summary>
        /// Right trigger Pull
        /// </summary>
        RightTrigger
    }
}