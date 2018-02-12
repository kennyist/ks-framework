using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KSManager : MonoBehaviour {


	// - Public Vars
	[Header("Game Data")]

	/// <summary>
	/// Name of the game, used by game file management
	/// </summary>
	public string gameName = "Default Title";

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
	private KS_FileHelper fileHelper = new KS_FileHelper(gameName, windowsDataLocation);

}
