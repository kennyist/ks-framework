using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class KS_FileHelper {

	/// <summary>
	/// Games Save/Config/Screenshot folder location.
	/// </summary>
	public enum DataLocation
	{
		/// <summary>
		/// Save in My Documents
		/// </summary>
		MyDocuments,
		/// <summary>
		/// Save in My Documents/My games/
		/// </summary>
		MyGames,
		/// <summary>
		/// Save in the users UserData folder
		/// </summary>
		UserData,
		/// <summary>
		/// Save in the games location
		/// </summary>
		GameFolder
	}

	/// <summary>
	/// Availible folders
	/// </summary>
	public enum Folders
	{
		/// <summary>
		/// Game Config save folder
		/// </summary>
		Configs,
		/// <summary>
		/// Game save file storage folder
		/// </summary>
		Saves,
		/// <summary>
		/// Screenshot save location
		/// </summary>
		Screenshots
	}

	private string _gameName;
	private DataLocation _dataLocationWin = DataLocation.MyDocuments;

	public KS_FileHelper(string gameName, DataLocation loc){
		_gameName = gameName;
		_dataLocationWin = loc;
		// Setup class, loading files
		setup ();
	}

	private void setup(){
		CheckDirectories ();
	}

	private void CheckDirectories()
	{
		string path = getBaseDirectoryStr(_dataLocationWin, Folders.Configs);

		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}

		path = getBaseDirectoryStr(_dataLocationWin, Folders.Saves);

		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}

		path = getBaseDirectoryStr(_dataLocationWin, Folders.Screenshots);

		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
	}

	private string getBaseDirectoryStr(DataLocation location, Folders folder){
		string path = "";

		switch (location)
		{
			case DataLocation.MyDocuments:
				path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/" + _gameName + "/";
				break;

			case DataLocation.MyGames:
				path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/My Games/" + _gameName + "/";
				break;

			case DataLocation.UserData:
				path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/" + _gameName + "/";
				break;

			case DataLocation.GameFolder:
				path = Application.dataPath + "/" + _gameName + "/";
				break;
		}

		return path.Replace("\\", "/") + folder.ToString() + "/";
	}

	// Public 


	public bool SaveFile(Folders Type, String fileName)
	{
		string basePath = getBaseDirectoryStr (_dataLocationWin, Type);

		return true;
	}

	public object LoadFile(Folders from, string fileName)
	{
		string basePath = getBaseDirectoryStr (_dataLocationWin, from);

		return true;
	}

	public object GetFolderContents(Folders folder)
	{
		string basePath = getBaseDirectoryStr (_dataLocationWin, folder);

		return true;
	}
}
