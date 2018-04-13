using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KS_Core.IO;

namespace KS_Core
{
    /// <summary>
    /// Storage of game settings and KS framework settings
    /// </summary>
    public class KS_Scriptable_GameConfig : ScriptableObject
    {
        /// <summary>
        /// Name of the game
        /// </summary>
        public string gameName = "Game Name";
        /// <summary>
        /// Current game version
        /// </summary>
        public string version = "0.01";
        /// <summary>
        /// Current build number
        /// </summary>
        public int buildNumber = 0;
        /// <summary>
        /// Where to store game files on windows systems
        /// </summary>
        public WindowsDataLocation windowsDataLocation = WindowsDataLocation.MyDocuments;
        /// <summary>
        /// Where to store game files on OSX systems
        /// </summary>
        public OSXDataLocation macDataLocation = OSXDataLocation.GameFolder;
        /// <summary>
        /// Where to store game files on Linux systems
        /// </summary>
        public LinuxDataLocation linuxDataLocation = LinuxDataLocation.GameFolder;

        /// <summary>
        /// Name of the settings config
        /// </summary>
        public string SettingsConfigName = "Settings";

        /// <summary>
        /// Localisation: return line not found text (true), return text used as lineID (false)
        /// </summary>
        public bool loc_returnNotFound = true;
        /// <summary>
        /// Localisation: line not found text
        /// </summary>
        public string loc_NotFoundLine = "# Line not found #";

        // Input 

        /// <summary>
        /// Input: Config name
        /// </summary>
        public string i_configName = "Input";

        // pooling

        /// <summary>
        /// Pooling: clear pool on level load
        /// </summary>
        public bool pool_ClearOnLoadLevel = true;

        // Saving loading

        /// <summary>
        /// Save/Load: Save format
        /// </summary>
        public string saveFileFormat = ".save";

        
    }
}