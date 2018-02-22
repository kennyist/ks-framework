using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;

public class KS_FileHelper {

    private static KS_FileHelper instance;

    public static KS_FileHelper Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }
            else
            {
                return null;
            }
        }
    }

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

    public enum GameDataFolders
    {
        /// <summary>
        /// Translate files folder
        /// </summary>
        Translate
    }

	private string _gameName;
    private string _localFolder;
	private DataLocation _dataLocationWin = DataLocation.MyDocuments;
    private string[] ignoreExt = { ".meta" };

	public KS_FileHelper(string gameName, DataLocation loc, string localFolder){
        Debug.Log("File Helper Loading");
        instance = this;

        _gameName = gameName;
		_dataLocationWin = loc;
        _localFolder = localFolder;

		// Setup class, loading files
		setup ();

        Debug.Log("File Helper Loaded");
    }

	private void setup(){
        CheckDirectories ();
	}

	private void CheckDirectories()
	{
        Debug.Log("Checking public directories");

        string path = getBaseDirectoryStr(_dataLocationWin);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Debug.Log("Created Game Data folder");
        }

        path = getBaseDirectoryStr(_dataLocationWin, Folders.Configs);

		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
            Debug.Log("Created Configs folder");
        }

		path = getBaseDirectoryStr(_dataLocationWin, Folders.Saves);

		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
            Debug.Log("Created Saves folder");
        }

		path = getBaseDirectoryStr(_dataLocationWin, Folders.Screenshots);

		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
            Debug.Log("Created Screenshot folder");
        }

        Debug.Log("Checking Game Data directories");

        path = getApplicationbasePath();

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Debug.Log("Created Application Data folder");
        }

        path = getApplicationbasePath(GameDataFolders.Translate);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Debug.Log("Created Translation folder");
        }
    }

    private string getBaseDirectoryStr(DataLocation location)
    {
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

        return path.Replace("\\", "/");
    }

    private string getBaseDirectoryStr(DataLocation location, Folders folder){

		return getBaseDirectoryStr(location) + folder.ToString() + "/";
	}

    private string getApplicationbasePath()
    {
        return Application.dataPath + "/" + _localFolder + "/";
    }

    private string getApplicationbasePath(GameDataFolders folder)
    {
        return Application.dataPath + "/" + _localFolder + "/" + folder.ToString() + "/";
    }

    // Public 


    public bool SaveFile(Folders Type, String fileName, string data)
	{
		string basePath = getBaseDirectoryStr (_dataLocationWin, Type);

        try
        {
            File.WriteAllText(basePath + fileName, data);
        }
        catch (IOException e) {
            return false;
        }

		return true;
	}

    public bool SaveGameFile(GameDataFolders Type, String fileName, string data)
    {
        string basePath = getApplicationbasePath(Type);

        try
        {
            File.WriteAllText(basePath + fileName, data);
        }
        catch (IOException e)
        {
            return false;
        }

        return true;
    }

    public object LoadFile(Folders from, string fileName)
	{
		string basePath = getBaseDirectoryStr (_dataLocationWin, from);

		return true;
	}

    public string LoadGameFile(GameDataFolders from, string fileName)
    {
        string ret = null;

        try
        {
            ret = File.ReadAllText(getApplicationbasePath(GameDataFolders.Translate) + fileName);
        } 
        catch(IOException e)
        {
            Debug.LogError(e);
        }

        return ret;
    }

    public bool RenameFile(Folders folder, string file, string newName)
    {
        try
        {
            File.Move(getBaseDirectoryStr(_dataLocationWin, folder) + file,
                getBaseDirectoryStr(_dataLocationWin, folder) + newName);
        }
        catch (IOException e)
        {
            return false;
        }

        return true;
    }

    public bool DeleteFile(Folders folder, string file)
    {
        try
        {
            File.Delete(getBaseDirectoryStr(_dataLocationWin, folder) + file);
        }
        catch(IOException e)
        {
            return false;
        }

        return true;
    }

	public string[] GetFolderContents(Folders folder)
	{
        string[] files = GetFiles(getBaseDirectoryStr(_dataLocationWin, Folders.Saves)).ToArray<string>();

        return files;
	}

    public string[] GetApplicationDataContents(GameDataFolders folder)
    {
        string[] files = GetFiles(getApplicationbasePath(GameDataFolders.Translate)).ToArray<string>();

        for (int i = 0;i < files.Length; i++)
        {
            files[i] = files[i].Replace(getApplicationbasePath(GameDataFolders.Translate), "");
        }

        return files;
    }

    private IEnumerable<string> GetFiles(string path)
    {
        var files = Directory.GetFiles(path, "*.*");

        foreach (var file in files)
        {
            if (ignoreExt.Contains(Path.GetExtension(file)))
            {
                continue;
            }

            yield return file;
        }
    }
}
