using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS_Core.Console
{
    /// <summary>
    /// Console command creation class, used for inheriting only. 
    /// </summary>
    /// <example>
    /// Example child class with registering a command and creating the command body.
    /// <code language="c#">
    /// public class ExampleCommands : KS_ConsoleCommands
    /// {
    ///    public ExampleCommands() : base()
    ///    {
    ///         KS_Console.Instance.RegisterCommand("print", ExCmd, "print text to the console E.G print \"hello world\"", "$S", true);
    ///    }
    ///    
    ///    void ExCmd(string[] args)
    ///    {
    ///        KS_Console.Instance.WriteToConsole(args[0], Color.white);
    ///    }
    /// }
    /// </code>
    /// </example>
    public class KS_ConsoleCommands
    {
        /// <summary>
        /// 
        /// </summary>
        public KS_ConsoleCommands()
        {
            KS_Console.Instance.OnCommand += OnCommand;

            KS_Console.Instance.RegisterCommand("qqq", exitgame, "Quick close the game", "", false);
            KS_Console.Instance.RegisterCommand("debug.log", debuglog, "Toggle unity debug out to console", "", false);
            KS_Console.Instance.RegisterCommand("help", help, "Show all commands, add page number after to change page", "$i", false);
            KS_Console.Instance.RegisterCommand("findcmd", findCmd, "Search commands by string, page number can be used after search string", "$s $i", false);
        }

        private void OnCommand(CommandHandeler handler, string[] args)
        {
            handler(args);
        }

        /// <summary>
        /// Remove active event links
        /// </summary>
        public void Destroy()
        {
            KS_Console.Instance.OnCommand -= OnCommand;
        }

        // Built In Commands

        void exitgame(string[] args)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }

        void help(string[] args)
        {
            int pageLength = 5;
            Debug.Log("COMMAND HELP RECEIVED");

            int totalPages = Mathf.CeilToInt(
                                    (float)KS_Console.Instance.TotalCommands /
                                    (float)pageLength
                                    );
            int page = 1;
            int start = 0;

            if (args != null)
            {
                page = int.Parse(args[0]);
            }

            if (page > totalPages) page = totalPages;

            start = pageLength * (page - 1);

            int i = 0;

            foreach (KeyValuePair<string, string> sl in KS_Console.Instance.GetAllCommandsAndHelp)
            {
                if (i < start || i >= start + pageLength) { }
                else
                {
                    KS_Console.Instance.WriteToConsole(
                                        sl.Key +
                                        ": " +
                                        sl.Value,
                                        Color.green);
                }

                i++;
            }

            KS_Console.Instance.WriteToConsole(
                                    "page " +
                                    page +
                                    " of " +
                                    totalPages,
                                    Color.cyan);

        }

        void findCmd(string[] args)
        {
            int pageLength = 5;
            SortedList<string, string> filtered = new SortedList<string, string>();

            foreach (KeyValuePair<string, string> sl in KS_Console.Instance.GetAllCommandsAndHelp)
            {
                if (sl.Key.Contains(args[0]))
                {
                    filtered.Add(sl.Key, sl.Value);
                }
            }

            int totalPages = Mathf.CeilToInt((float)filtered.Count / (float)pageLength);
            int page = 1;
            int start = 0;

            Debug.Log(args.Length);

            if (args.Length > 1 && !string.IsNullOrEmpty(args[1]))
            {
                page = int.Parse(args[1]);
            }

            if (page > totalPages) page = totalPages;
            if (page < 1) page = 1;

            start = pageLength * (page - 1);
            int end = ((start + pageLength) > filtered.Count) ? filtered.Count : start + pageLength;

            for (int i = start; i < end; i++)
            {
                KS_Console.Instance.WriteToConsole(
                                    filtered.Keys[i] +
                                    ": " +
                                    filtered.Values[i],
                                    Color.green);
            }

            KS_Console.Instance.WriteToConsole(
                                "Page " +
                                page +
                                " of " +
                                totalPages,
                                Color.cyan);
        }

        void debuglog(string[] Args)
        {
            KS_Console.Instance.OutputUnityDebug = !KS_Console.Instance.OutputUnityDebug;

            if (KS_Console.Instance.OutputUnityDebug)
            {
                KS_Console.Instance.WriteToConsole("Log toggled On", Color.green);
            }
            else
            {
                KS_Console.Instance.WriteToConsole("Log toggled Off", Color.green);
            }
        }

    }
}