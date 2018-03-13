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

    public enum ScreenShotSaveLocation
    {
        MyPictures,
        DataLocation
    }

    private string[] ignoreExt = { ".meta" };

    private KS_Scriptable_GameConfig gameConfig;

    public KS_FileHelper(KS_Scriptable_GameConfig config)
    {
        gameConfig = config;
        instance = this;
        Setup();
    }

	private void Setup(){
        CheckDirectories (gameConfig.windowsDataLocation);
	}

	private void CheckDirectories(DataLocation location)
	{
        Debug.Log("Checking public directories");

        string path = getBaseDirectoryStr(location);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Debug.Log("Created Game Data folder");
        }

        path = getBaseDirectoryStr(location, Folders.Configs);

		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
            Debug.Log("Created Configs folder");
        }

		path = getBaseDirectoryStr(location, Folders.Saves);

		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
            Debug.Log("Created Saves folder");
        }

		path = getBaseDirectoryStr(location, Folders.Screenshots);

		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
            Debug.Log("Created Screenshot folder");
        }
    }

    private string getBaseDirectoryStr(DataLocation location)
    {
        string path = "";

        switch (location)
        {
            case DataLocation.MyDocuments:
                path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/" + gameConfig.gameName + "/";
                break;

            case DataLocation.MyGames:
                path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/My Games/" + gameConfig.gameName + "/";
                break;

            case DataLocation.UserData:
                path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/" + gameConfig.gameName + "/";
                break;

            case DataLocation.GameFolder:
                path = Application.dataPath + "/" + gameConfig.gameName + "/";
                break;
        }

        return path.Replace("\\", "/");
    }

    private string getBaseDirectoryStr(DataLocation location, Folders folder){

		return getBaseDirectoryStr(location) + folder.ToString() + "/";
	}

    // Public 


    public bool SaveFile(Folders Type, String fileName, string data)
	{
		string basePath = getBaseDirectoryStr (gameConfig.windowsDataLocation, Type);

        try
        {
            File.WriteAllText(basePath + fileName, data);
            Debug.Log("File written: " + basePath + fileName);
        }
        catch (IOException e) {
            return false;
        }

		return true;
	}

    public bool SaveFile(Folders Type, String fileName, byte[] data)
    {
        string basePath = getBaseDirectoryStr(gameConfig.windowsDataLocation, Type);

        try
        {
            File.WriteAllBytes(basePath + fileName, data);
            Debug.Log("File written: " + basePath + fileName);
        }
        catch (IOException e)
        {
            return false;
        }

        return true;
    }

    public byte[] LoadFile(Folders from, string fileName)
	{
		string basePath = getBaseDirectoryStr (gameConfig.windowsDataLocation, from);

		return File.ReadAllBytes(basePath + fileName);
	}

    public bool RenameFile(Folders folder, string file, string newName)
    {
        try
        {
            File.Move(getBaseDirectoryStr(gameConfig.windowsDataLocation, folder) + file,
                getBaseDirectoryStr(gameConfig.windowsDataLocation, folder) + newName);
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
            File.Delete(getBaseDirectoryStr(gameConfig.windowsDataLocation, folder) + file);
        }
        catch(IOException e)
        {
            return false;
        }

        return true;
    }

	public string[] GetFolderContents(Folders folder)
	{
        string[] files = GetFiles(getBaseDirectoryStr(gameConfig.windowsDataLocation, folder)).ToArray<string>();

        if(files.Length > 0)
        {
            for(int i = 0; i < files.Length; i++)
            {
                files[i] = files[i].Replace(getBaseDirectoryStr(gameConfig.windowsDataLocation, folder), "");
            }
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

    public bool GetFile(Folders folder, string filename)
    {
        string[] path = GetFolderContents(folder);

        Debug.Log(path.Length);

        if (path == null || path.Length <= -1) return false;

        foreach(string s in path)
        {
            Debug.Log(filename + ":" + s);
            if (s.Equals(filename)) return true;
        }

        return false;
    }
}
