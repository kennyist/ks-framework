using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KS_Core.IO;

namespace KS_Core
{
    public class KS_Scriptable_GameConfig : ScriptableObject
    {

        public string gameName = "Game Name";
        public string version = "0.01";
        public int buildNumber = 0;
        public WindowsDataLocation windowsDataLocation = WindowsDataLocation.MyDocuments;
        public OSXDataLocation macDataLocation = OSXDataLocation.GameFolder;
        public LinuxDataLocation linuxDataLocation = LinuxDataLocation.GameFolder;

        public string SettingsConfigName = "Settings";

        public bool loc_returnNotFound = true;
        public string loc_NotFoundLine = "# Line not found #";

        // Input 

        public string i_configName = "Input";

        // pooling

        public bool pool_ClearOnLoadLevel = true;

        // Saving loading

        public string saveFileFormat = ".save";
    }
}