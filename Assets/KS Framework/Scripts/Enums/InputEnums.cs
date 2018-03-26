using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS_Core.Input
{
    public enum DS4KeyCode
    {
        Square,
        Circle,
        Triangle,
        X,
        L1,
        L2,
        L3,
        R1,
        R2,
        R3,
        Options,
        Share,
        Home,
        TouchPad
    }

    public enum DS4Axis
    {
        LeftStickX,
        LeftStickY,
        RightStickX,
        RightStickY,
        L2,
        R2,
        DPadX,
        DPadY,
        /*GyroX, Planned Addition
        GyroY,
        GyroRaw,
        AccelerometerX,
        AccelerometerY,
        AccelerometerRaw*/
    }

    public enum XboxKeyCode
    {
        A,
        B,
        X,
        Y,
        LB,
        RB,
        LeftStickCLick,
        RightStickClick,
        Back,
        Start,
        Home,
        DpadUp,
        DpadLeft,
        DpadDown,
        DpadRight
    }

    public enum XboxAxis
    {
        LeftStickX,
        LeftStickY,
        RightStickX,
        RightStickY,
        DpadX,
        DpadY,
        LeftTrigger,
        RightTrigger
    }
}