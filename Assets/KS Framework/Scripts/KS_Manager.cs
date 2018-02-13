using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class KS_Manager : MonoBehaviour {


	// - Public Vars
	[Header("Game Setup")]

	/// <summary>
	/// Name of the game, used by game file management
	/// </summary>
	public string gameName = "Default Title";

    [Header("Storage Settings")]

    /// <summary>
    /// File location within the project to save data
    /// </summary>
    public string localDataFolder = "KS_Data";

    /// <summary>
    /// Location of game saves and config files for Windows
    /// </summary>
    public KS_FileHelper.DataLocation windowsDataLocation = KS_FileHelper.DataLocation.MyDocuments;

	/// <summary>
	/// Location of game saves and config files for Mac
	/// </summary>
	public KS_FileHelper.DataLocation macDataLocation = KS_FileHelper.DataLocation.MyDocuments;

	/// <summary>
	/// Location of game saves and config files for Linux
	/// </summary>
	public KS_FileHelper.DataLocation linuxDataLocation = KS_FileHelper.DataLocation.MyDocuments;


	// - Private Vars
	private KS_FileHelper fileHelper;

    void Awake()
    {
        Debug.Log("Loading Manager");
        // Setup helpers
        fileHelper = new KS_FileHelper(gameName, windowsDataLocation, localDataFolder);
    }

}
