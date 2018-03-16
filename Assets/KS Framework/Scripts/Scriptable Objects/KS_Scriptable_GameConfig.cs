using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KS_Scriptable_GameConfig : ScriptableObject {

    public string gameName = "KS_OWGF";
    public string version = "0.01";
    public int buildNumber = 0;
    public KS_FileHelper.DataLocation windowsDataLocation = KS_FileHelper.DataLocation.MyDocuments;
    public KS_FileHelper.DataLocation macDataLocation = KS_FileHelper.DataLocation.MyDocuments;
    public KS_FileHelper.DataLocation linuxDataLocation = KS_FileHelper.DataLocation.MyDocuments;
    public KS_FileHelper.ScreenShotSaveLocation ScreenShotSaveLocation = KS_FileHelper.ScreenShotSaveLocation.DataLocation;

    public string SettingsConfigName = "Settings";

    public bool loc_returnNotFound = true;
    public string loc_NotFoundLine = "# Line not found #";

    // Input 

    public string i_configName = "Input";

    // pooling

    public bool pool_ClearOnLoadLevel = true;
}
 