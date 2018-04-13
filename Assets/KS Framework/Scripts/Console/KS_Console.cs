using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace KS_Core.Console
{
    /// <summary>
    /// A commands handler for returned method and arguments
    /// </summary>
    /// <param name="parameters">Returned arguements from the console</param>
    public delegate void CommandHandeler(string[] parameters);

    /// <summary>
    /// KS console, this class is responsible for all the internal aspects of the console such as command handeling, Log handling and converting an command input.
    /// </summary>
    public class KS_Console
    {

        private static KS_Console instance;
        /// <summary>
        /// Current active instance of KS_Console
        /// </summary>
        /// <returns>Active KS_console instance, new instance if no active instance exists.</returns>
        public static KS_Console Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }
                else
                {
                    return new KS_Console();
                }
            }
        }

        /// <summary>
        /// On commad called delegate
        /// </summary>
        /// <param name="handler">Commands method hanlder</param>
        /// <param name="args">Commands arguments</param>
        public delegate void OnCommandDelegate(CommandHandeler handler, string[] args);
        /// <summary>
        /// On command called
        /// </summary>
        public event OnCommandDelegate OnCommand;
        /// <summary>
        /// On text added to the consoles log delegate
        /// </summary>
        /// <param name="text">Text added to the log</param>
        public delegate void OnLogUpdateDelegate(string text);
        /// <summary>
        /// On new text added to the consoles log
        /// </summary>
        public event OnLogUpdateDelegate OnLogUpdate;

        /// <summary>
        /// Container for all information for a command, such as command name, method to call and parameter setup.
        /// </summary>
        public class ConsoleCommand
        {
            /// <summary>
            /// The string to enter into the console
            /// </summary>
            public string command { get; private set; }
            /// <summary>
            /// The commands handler to call when activated
            /// </summary>
            public CommandHandeler handler { get; private set; }
            /// <summary>
            /// The Commands help text, Displayed in the console
            /// </summary>
            public string help { get; private set; }
            /// <summary>
            /// The commands Argument format 
            /// </summary>
            public string[] format { get; private set; }
            /// <summary>
            /// Does the command require all arguments to be filled?
            /// </summary>
            public bool requiresAllParams { get; private set; }

            /// <summary>
            /// Number of arguments for the command
            /// </summary>
            public int numParams { get; private set; }

            /// <summary>
            /// Initialise new console command, <see cref="KS_Console.RegisterCommand(ConsoleCommand)"> Used only with KS_Console </see>.
            /// </summary>
            /// <param name="command">The command activate name for users</param>
            /// <param name="handler">The callback method when activated. <see cref="KS_ConsoleCommands">See KS_ConsoleCommands for use</see></param>
            /// <param name="help">The Commands help text, Displayed in console</param>
            /// <param name="paramFormat">Commands Paramaters format, Supported formats: $I = int, $S = string, $F = float, $B = bool</param>
            /// <param name="requiresAllParams">All parameters need to be filled?</param>
            /// <example>
            /// The following example is the findcmd command, that uses a string parameter for searching and int parameter for page.
            /// 
            /// <code language="c#">
            /// ConsoleCommand example = new ConsoleCommand("findcmd", findCmd, "Search commands by string, page number can be used after search string", "$s $i", false)
            /// </code>
            /// </example>
            public ConsoleCommand(string command, CommandHandeler handler, string help, string paramFormat, bool requiresAllParams = true)
            {
                this.command = command;
                this.handler = handler;
                this.help = help;
                this.requiresAllParams = requiresAllParams;

                if (!string.IsNullOrEmpty(paramFormat))
                {
                    string[] formatSplit = paramFormat.Split(' ');
                    this.numParams = formatSplit.Length;
                    this.format = ParseFormat(formatSplit);
                }
                else
                {
                    this.numParams = 0;
                    this.format = null;
                }
            }

            private string[] ParseFormat(string[] format)
            {
                string[] ret = new string[numParams];

                for (int i = 0; i < numParams; i++)
                {
                    switch (format[i])
                    {
                        case "$I":
                        case "$i":
                            ret[i] = "int";
                            break;

                        case "$F":
                        case "$f":
                            ret[i] = "float";
                            break;

                        case "$B":
                        case "$b":
                            ret[i] = "bool";
                            break;

                        default:
                        case "$s":
                            ret[i] = "string";
                            break;
                    }
                }

                return ret;
            }
        }

        private int lastUsedIndex = -1;
        private List<string> log = new List<string>();

        private List<string> lastUsed = new List<string>();
        private Dictionary<string, ConsoleCommand> commands = new Dictionary<string, ConsoleCommand>();

        private bool outputUnityDebug = false;
        /// <summary>
        /// Output unity debug log to the console?
        /// </summary>
        public bool OutputUnityDebug
        {
            get
            {
                return outputUnityDebug;
            }

            set
            {
                outputUnityDebug = value;
            }
        }

        /// <summary>
        /// Create new instance of KS_Console
        /// </summary>
        public KS_Console()
        {
            instance = this;
            Application.logMessageReceived += HandleLog;
        }

        void HandleLog(string logString, string stackTrace, LogType type)
        {
            if (!outputUnityDebug)
                return;

            switch (type)
            {
                case LogType.Log:
                default:
                    AppendLog(logString, Color.grey);
                    break;

                case LogType.Assert:
                case LogType.Exception:
                case LogType.Error:
                    AppendLog(logString, Color.red);
                    break;

                case LogType.Warning:
                    AppendLog(logString, Color.yellow);
                    break;
            }
        }

        /// <summary>
        /// Register a new command to the console.
        /// </summary>
        /// <param name="command">The command activate name for users</param>
        /// <param name="handler">The callback method when activated. <see cref="KS_ConsoleCommands">See KS_ConsoleCommands for use</see></param>
        /// <param name="help">The Commands help text, Displayed in console</param>
        /// <param name="format">Commands Paramaters format, Supported formats: $I = int, $S = string, $F = float, $B = bool</param>
        /// <param name="requiresAllParams">All parameters need to be filled?</param>
        /// <example>
        /// Example of the findcmd command being registered.
        /// 
        /// <code language="c#">
        /// KS_Console.Instance.RegisterCommand("findcmd", findCmd, "Search commands by string, page number can be used after search string", "$s $i", false);
        /// </code>
        /// </example>
        public void RegisterCommand(string command, CommandHandeler handler, string help, string format, bool requiresAllParams = true)
        {
            commands.Add(command, new ConsoleCommand(command, handler, help, format, requiresAllParams));
        }

        /// <summary>
        /// Register new command to the console
        /// </summary>
        /// <param name="command"><see cref="KS_Console.ConsoleCommand.ConsoleCommand(string, CommandHandeler, string, string, bool)"/></param>
        public void RegisterCommand(ConsoleCommand command)
        {
            commands.Add(command.command, command);
        }

        /// <summary>
        /// Run console command string
        /// </summary>
        /// <param name="command">Input string enetered into console</param>
        /// <example>
        /// Basic input string for the help command with page number:
        /// <code language="c#">
        /// RunCommandString("help 4");
        /// </code>
        /// This convert and run the help command parsing 4 in as method arguments
        /// </example>
        public void RunCommandString(string command)
        {
            Debug.Log("Command: " + command);

            command.TrimStart();
            command.TrimEnd();

            if (string.IsNullOrEmpty(command))
            {
                return;
            }

            lastUsed.Add(command);
            lastUsedIndex = -1;
            AppendLog(command, Color.white);

            string[] full = command.Split(' ');

            string[] args = Regex.Split(StringImplode(full, 1), "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*) (?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");

            for (int i = 0; i < args.Length; i++)
            {
                args[i] = args[i].Replace("\"", string.Empty);

            }

            if (args.Length == 1 && string.IsNullOrEmpty(args[0]))
            {
                args = null;
            }

            RunCommand(full[0].ToLower(), args);
        }

        private string StringImplode(string[] s, int start)
        {
            string r = "";

            for (int i = start; i < s.Length; i++)
            {
                r += s[i] + " ";
            }

            return r.TrimEnd();
        }


        private void RunCommand(string command, string[] Args)
        {
            Debug.Log("Got command: " + command);

            ConsoleCommand toRun = null;

            if (!commands.TryGetValue(command, out toRun))
            {
                AppendLog(string.Format("Unknown command '{0}', type 'help' for list.", command), Color.cyan);
            }
            else
            {
                if (toRun.handler == null)
                {
                    AppendLog(string.Format("Unable to process command '{0}', handler was null.", command), Color.red);
                }
                else
                {
                    if (toRun.requiresAllParams)
                    {
                        if (Args == null || toRun.numParams != Args.Length)
                        {
                            AppendLog("Number of parameters does not match the command", Color.red);
                            return;
                        }
                    }

                    if (Args != null)
                    {
                        if (!CheckCommandArgsFormat(toRun, Args))
                        {
                            return;
                        }
                    }

                    if (OnCommand != null)
                        OnCommand(toRun.handler, Args);


                }
            }
        }

        private bool CheckCommandArgsFormat(ConsoleCommand command, string[] args)
        {
            List<string> errors = new List<string>();
            bool error = false;

            Debug.Log(args.Length);

            for (int i = 0; i < args.Length; i++)
            {
                switch (command.format[i])
                {
                    case "string":

                        break;

                    case "int":
                        int j = 0;
                        if (!int.TryParse(args[i], out j))
                        {
                            errors.Add("Parameter " + (i + 1) + " Expected INT");
                            error = true;
                        }
                        break;

                    case "float":
                        float k = 0f;
                        if (!float.TryParse(args[i], out k))
                        {
                            errors.Add("Parameter " + (i + 1) + " Expected Float");
                            error = true;
                        }
                        break;

                    case "bool":
                        bool l = false;
                        if (!bool.TryParse(args[i], out l))
                        {
                            errors.Add("Parameter " + (i + 1) + " Expected Bool");
                            error = true;
                        }
                        break;
                }
            }

            if (error)
            {
                string errorText = "Command Errors: ";
                bool first = true;

                for (int i = 0; i < errors.Count; i++)
                {
                    if (first)
                    {
                        errorText += errors[i] + "";
                        first = false;
                    }
                    else
                    {
                        errorText += ", " + errors[i];
                    }
                }

                AppendLog(errorText, Color.red);

                return false;
            }
            else
            {
                return true;
            }
        }


        private void AppendLog(string text, Color colour)
        {
            string add = "\n<color=#" + ColorUtility.ToHtmlStringRGBA(colour) + ">" +
                            text + "</color>";
            log.Add(add);

            if (log.Count > 100)
            {
                log.RemoveAt(0);
            }

            if (OnLogUpdate != null)
            {
                OnLogUpdate(add);
            }
        }

        /// <summary>
        /// Write text to the console log
        /// </summary>
        /// <param name="text">Text to show</param>
        /// <param name="colour">Colour of the text</param>
        public void WriteToConsole(string text, Color colour)
        {
            AppendLog(text, colour);
        }

        /// <summary>
        /// Total commands regstered
        /// </summary>
        public int TotalCommands
        {
            get
            {
                return commands.Count;
            }
        }

        /// <summary>
        /// Get all commands with help text sorted alphabeticaly by comamnd name.
        /// </summary>
        public SortedList<string, string> GetAllCommandsAndHelp
        {
            get
            {
                SortedList<string, string> temp = new SortedList<string, string>();

                foreach (KeyValuePair<string, ConsoleCommand> kv in commands)
                {
                    temp.Add(kv.Key, kv.Value.help);
                }

                return temp;
            }
        }

        private string GetLogString()
        {
            string logText = "";

            foreach (string s in log)
            {
                logText += s;
            }

            return logText;
        }

        /// <summary>
        /// Get console log to string
        /// </summary>
        public string GetLog
        {
            get
            {
                return GetLogString();
            }
        }

        /// <summary>
        /// Get a previosuly used command from previous command list.
        /// </summary>
        public string PreviousCommand
        {
            get
            {
                lastUsedIndex++;

                if (lastUsedIndex >= lastUsed.Count) lastUsedIndex--;


                return GetUsed(lastUsedIndex);
            }
        }

        /// <summary>
        /// Get a next used command from prvious command list.
        /// </summary>
        public string NextUsedCommand
        {
            get
            {
                if (lastUsedIndex > -1) lastUsedIndex--;

                return GetUsed(lastUsedIndex);
            }
        }

        private string GetUsed(int index)
        {
            int i;

            if (index < 0) return "";

            if (index <= 0)
            {
                i = lastUsed.Count - 1;
            }
            else
            {
                i = (lastUsed.Count - 1) - index;
            }

            if (i < 0)
            {
                return "";
            }
            else
            {
                return lastUsed[i];
            }
        }

    }
}
