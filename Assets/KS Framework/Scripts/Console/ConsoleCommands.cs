using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KS_Core.Console;
using KS_Core.GameTime;

public class ConsoleCommands : KS_ConsoleCommands
{
    public ConsoleCommands() : base()
    {
        KS_Console.Instance.RegisterCommand("time.set", settime, "Set the time - time.set [hour 0-23] [minute 0-59]", "$i $i", true);
        KS_Console.Instance.RegisterCommand("time.setsmooth", settimesmooth, "Change the time over time - time.setsmooth [hour 0-23] [minute 0-59] [time]", "$f $f $f", true);
        KS_Console.Instance.RegisterCommand("time.scale", settimescale, "Set the time scale - time.scale [0.01 - 60.0]", "$f", true);
        KS_Console.Instance.RegisterCommand("env.daynight.setlonglat", setlatLong, "Set latitudde and longitude for lighting - env.daynight.setlatlong [-89.99 - 89.99] [0.01 - 179.99]", "$f $f", true);
    }

    // Env

    void settime(string[] Args)
    {
        KS_TimeManager.Instance.SetTime(int.Parse(Args[0]), int.Parse(Args[1]));

        KS_Console.Instance.WriteToConsole("Time set to " + Args[0] + ":" + Args[1], Color.green);
    }

    void settimesmooth(string[] Args)
    {
        KS_TimeManager.Instance.SetTimeOverTime(
                                int.Parse(Args[0]),
                                int.Parse(Args[1]),
                                int.Parse(Args[2]));

        KS_Console.Instance.WriteToConsole("Setting time to " + Args[0] + ":" + Args[1] + " over " + Args[2] + " Seconds", Color.green);
    }

    void settimescale(string[] Args)
    {
        KS_TimeManager.Instance.SetTimeScale(float.Parse(Args[0]));

        KS_Console.Instance.WriteToConsole("Time scale set to " + Args[0], Color.green);
    }

    void setlatLong(string[] Args)
    {

    }
}

public class ExampleCommands : KS_ConsoleCommands
{
    public ExampleCommands() : base()
    {
        KS_Console.Instance.RegisterCommand("print", ExCmd, "print text to the console E.G print \"hello world\"", "$S", true);
    }

    void ExCmd(string[] args)
    {
        KS_Console.Instance.WriteToConsole(args[0], Color.white);
    }
}
