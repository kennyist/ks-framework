using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;

namespace KS_Core.IO
{
    /// <summary>
    /// 
    /// </summary>
    public class KS_FileHelper
    {

        private static KS_FileHelper instance;
        /// <summary>
        /// Current Active instance of KS_FileHelper
        /// </summary>
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

        private string[] ignoreExt = { ".meta" };

        private KS_Scriptable_GameConfig gameConfig;

        /// <summary>
        /// Create a new instance of KS_FileHelper
        /// </summary>
        /// <param name="config"><see cref="KS_Scriptable_GameConfig">KS Framework Game Config</see></param>
        public KS_FileHelper(KS_Scriptable_GameConfig config)
        {
            gameConfig = config;
            instance = this;
            CheckDirectories();
        }

        /* Check Directories Exist */
        #region CheckDirectories

        /// <summary>
        /// 
        /// </summary>
        private void CheckDirectories()
        {
            Debug.Log("[KS_FileHelper] - CheckDirectories - Checking public directories");

            foreach (Folders s in Enum.GetValues(typeof(Folders)))
            {
                string path = GetFolderPathByOS(s);

                Debug.Log("[KS_FileHelper] - CheckDirectories - Checking path: " + path);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    Debug.Log("[KS_FileHelper] - CheckDirectories - Created folder at path: " + path);
                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        private void CheckDirectories(OSXDataLocation location)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        private void CheckDirectories(LinuxDataLocation location)
        {

        }

        #endregion

        /* Get Directory Paths */
        #region Directory Paths

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private static string BaseDirectoryPath(WindowsDataLocation location)
        {
            string path = "";

            switch (location)
            {
                case WindowsDataLocation.MyDocuments:
                    path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/";
                    break;

                case WindowsDataLocation.MyGames:
                    path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/My Games/";
                    break;

                case WindowsDataLocation.UserData:
                    path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/";
                    break;

                case WindowsDataLocation.GameFolder:
                    path = Application.dataPath + "/";
                    break;
            }

            return path.Replace("\\", "/");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private static string BaseDirectoryPath(LinuxDataLocation location)
        {
            string path = "";

            switch (location)
            {
                case LinuxDataLocation.GameFolder:
                    path = Application.dataPath + "/";
                    break;

                case LinuxDataLocation.LocalApplicationData:
                    path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/";
                    break;

                case LinuxDataLocation.Personal:
                    path = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/";
                    break;
            }

            return path;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        private static string BaseDirectoryPath(OSXDataLocation location)
        {
            string path = "";

            switch (location)
            {
                case OSXDataLocation.GameFolder:
                    path = Application.dataPath + "/";
                    break;

                case OSXDataLocation.LocalApplicationData:
                    path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/";
                    break;

                case OSXDataLocation.MyDocuments:
                    path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/";
                    break;

                case OSXDataLocation.MyGames:
                    path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/My Games/";
                    break;
            }

            return "";
        }

        /// <summary>
        /// Get absolute path for windows data location and folder
        /// </summary>
        /// <param name="location">Save location <seealso cref="WindowsDataLocation"/> </param>
        /// <param name="folder">Folder <seealso cref="Folders"/></param>
        /// <returns>String path to choosen folder</returns>
        public string GetFolderPath(WindowsDataLocation location, Folders folder)
        {
            return BaseDirectoryPath(location) + gameConfig.gameName + "/" + folder.ToString() + "/";
        }

        /// <summary>
        /// Get absolute path for Linux data location and folder 
        /// </summary>
        /// <param name="location">Save location <seealso cref="LinuxDataLocation"/> </param>
        /// <param name="folder">Folder <seealso cref="Folders"/></param>
        /// <returns>String path to choosen folder</returns>
        public string GetFolderPath(LinuxDataLocation location, Folders folder)
        {
            return BaseDirectoryPath(location) + gameConfig.gameName + "/" + folder.ToString() + "/";
        }

        /// <summary>
        /// Get absolute path for OSX data location and folder
        /// </summary>
        /// <param name="location">Save location <seealso cref="OSXDataLocation"/> </param>
        /// <param name="folder">Folder <seealso cref="Folders"/></param>
        /// <returns>String path to choosen folder</returns>
        public string GetFolderPath(OSXDataLocation location, Folders folder)
        {
            return BaseDirectoryPath(location) + gameConfig.gameName + "/" + folder.ToString() + "/";
        }

        /// <summary>
        /// Get absolute path to folder on current OS
        /// </summary>
        /// <param name="folder">Folder <seealso cref="Folders"/></param>
        /// <returns>String path to choosen folder</returns>
        public string GetFolderPathByOS(Folders folder)
        {
            string basePath = null;

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            basePath = GetFolderPath(gameConfig.windowsDataLocation, folder);
#endif
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
            basePath = GetFolderPath(gameConfig.macDataLocation, folder);
#endif
#if UNITY_STANDALONE_LINUX
            basePath = GetFolderPath(gameConfig.linuxDataLocation, Folders);
#endif

            return basePath;
        }

        /// <summary>
        /// Get Absolute path to game data directory
        /// </summary>
        /// <returns>String path to game data directory</returns>
        public string GetDirectoryPathByOS()
        {
            string basePath = null;

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
            basePath = BaseDirectoryPath(gameConfig.windowsDataLocation);
#endif
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
            basePath = BaseDirectoryPath(gameConfig.macDataLocation);
#endif
#if UNITY_STANDALONE_LINUX
            basePath = BaseDirectoryPath(gameConfig.linuxDataLocation);
#endif

            return basePath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetGameFolderPath()
        {
            return GetDirectoryPathByOS() + gameConfig.gameName + "/";
        }

        #endregion

        /* Saving */
        #region Saving 

        /// <summary>
        /// Save string data to a file in choosen folder
        /// </summary>
        /// <param name="folder">Folder to save in <seealso cref="Folders"/></param>
        /// <param name="fileName">file save name including extenstion</param>
        /// <param name="data">the data to save to the file</param>
        /// <returns>True if saved, false if error occured</returns>
        public bool SaveFile(Folders folder, String fileName, string data)
        {
            string basePath = GetFolderPathByOS(folder);

            if (string.IsNullOrEmpty(basePath)) return false;   // Base directory path not found

            try
            {
                File.WriteAllText(basePath + fileName, data);
                Debug.Log("File written: " + basePath + fileName);
            }
            catch (IOException e)
            {
                Debug.LogException(e);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Save byte data to a file in choosen folder
        /// </summary>
        /// <param name="folder">Folder to save in <seealso cref="Folders"/></param>
        /// <param name="fileName">File save name including extensiion</param>
        /// <param name="data">Byte data to save to the file</param>
        /// <returns>True if saved, false if an error ocured</returns>
        public bool SaveFile(Folders folder, String fileName, byte[] data)
        {
            string basePath = GetFolderPathByOS(folder);

            try
            {
                File.WriteAllBytes(basePath + fileName, data);
                Debug.Log("File written: " + basePath + fileName);
            }
            catch (IOException e)
            {
                Debug.LogException(e);
                return false;
            }

            return true;
        }

        #endregion
        /* File loading and editing */
        #region File Loading and Editing

        /// <summary>
        /// Load file from folder to byte array
        /// </summary>
        /// <param name="folder">Folder to load from <seealso cref="Folders"/></param>
        /// <param name="fileName">the file to load including extension</param>
        /// <returns>Byte array of file contents</returns>
        public byte[] LoadFileToBytes(Folders folder, string fileName)
        {
            string basePath = GetFolderPathByOS(folder);

            return File.ReadAllBytes(basePath + fileName);
        }

        /// <summary>
        /// Load file from folder to string
        /// </summary>
        /// <param name="folder">Folder to load from <seealso cref="Folders"/></param>
        /// <param name="fileName">File name to load including extension</param>
        /// <returns>String of file contents</returns>
        public string LoadFileToString(Folders folder, string fileName)
        {
            string basePath = GetFolderPathByOS(folder);

            return File.ReadAllText(basePath + fileName);
        }

        /// <summary>
        /// Rename file
        /// </summary>
        /// <param name="folder">Folder the file is located</param>
        /// <param name="file">Original file name including extension</param>
        /// <param name="newName">New file name including extension</param>
        /// <returns>True if renamed, false if an error occured</returns>
        public bool RenameFile(Folders folder, string file, string newName)
        {
            string basePath = GetFolderPathByOS(folder);

            try
            {
                File.Move(basePath + file,
                    basePath + newName);
            }
            catch (IOException e)
            {
                Debug.LogException(e);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Delete file from folder
        /// </summary>
        /// <param name="folder">Folder to delete from</param>
        /// <param name="file">File name including extension</param>
        /// <returns>True if deleted, false if an error occured</returns>
        public bool DeleteFile(Folders folder, string file)
        {
            string basePath = GetFolderPathByOS(folder);

            try
            {
                File.Delete(basePath + file);
            }
            catch (IOException e)
            {
                Debug.LogException(e);
                return false;
            }

            return true;
        }
        #endregion

        /* Read Folder Contents */
        #region Read Folder Contents

        /// <summary>
        /// Get names of all files inside folder
        /// </summary>
        /// <param name="folder">Folder to read <seealso cref="Folders"/></param>
        /// <returns>Sstring array of file names with extensions</returns>
        public string[] GetFolderContents(Folders folder)
        {
            string basePath = GetFolderPathByOS(folder);

            string[] files = GetFiles(basePath).ToArray<string>();

            if (files.Length > 0)
            {
                for (int i = 0; i < files.Length; i++)
                {
                    files[i] = files[i].Replace(basePath, "");
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

        /// <summary>
        /// Check if file exists
        /// </summary>
        /// <param name="folder">Folder to check in <seealso cref="Folders"/></param>
        /// <param name="filename">name of the file to check including extension</param>
        /// <returns>True if found, false if no found</returns>
        public bool DoesFileExist(Folders folder, string filename)
        {
            string[] path = GetFolderContents(folder);

            Debug.Log(path.Length);

            if (path == null || path.Length <= -1) return false;

            foreach (string s in path)
            {
                Debug.Log(filename + ":" + s);
                if (s.Equals(filename)) return true;
            }

            return false;
        }
        #endregion

        /* Static Helpers */
        #region Static Helpers
        
        /// <summary>
        /// Get absolute path to windows data location
        /// </summary>
        /// <param name="location">Location to find <seealso cref="WindowsDataLocation"/></param>
        /// <returns>Path to location</returns>
        public static string GetDirectoryPath(WindowsDataLocation location)
        {
            return BaseDirectoryPath(location);
        }

        /// <summary>
        /// Get absolute path to OSX data location
        /// </summary>
        /// <param name="location">Location to find <seealso cref="OSXDataLocation"/></param>
        /// <returns>Path to location</returns>
        public static string GetDirectoryPath(OSXDataLocation location)
        {
            return BaseDirectoryPath(location);
        }

        /// <summary>
        /// Get absolute path to Linux data location
        /// </summary>
        /// <param name="location">Location to find <seealso cref="LinuxDataLocation"/></param>
        /// <returns>Path to location</returns>
        public static string GetDirectoryPath(LinuxDataLocation location)
        {
            return BaseDirectoryPath(location);
        }

        /// <summary>
        /// Get absolute path to windows data location, including game name and child folder
        /// </summary>
        /// <param name="location">Location to find <seealso cref="WindowsDataLocation"/></param>
        /// <param name="GameFolderName">Name of the games folder</param>
        /// <param name="folder">Folder to find <seealso cref="Folders"/></param>
        /// <returns>Path to location</returns>
        public static string GetFolderPath(WindowsDataLocation location, string GameFolderName, Folders folder)
        {
            return BaseDirectoryPath(location) + GameFolderName + "/" + folder.ToString() + "/";
        }

        /// <summary>
        /// Get absolute path to OSX data location, including game name and child folder
        /// </summary>
        /// <param name="location">Location to find <seealso cref="OSXDataLocation"/></param>
        /// <param name="GameFolderName">Name of the games folder</param>
        /// <param name="folder">Folder to find <seealso cref="Folders"/></param>
        /// <returns>Path to location</returns>
        public static string GetFolderPath(OSXDataLocation location, string GameFolderName, Folders folder)
        {
            return BaseDirectoryPath(location) + GameFolderName + "/" + folder.ToString() + "/";
        }

        /// <summary>
        /// Get absolute path to Linux data location, including game name and child folder
        /// </summary>
        /// <param name="location">Location to find <seealso cref="LinuxDataLocation"/></param>
        /// <param name="GameFolderName">Name of the games folder</param>
        /// <param name="folder">Folder to find <seealso cref="Folders"/></param>
        /// <returns>Path to location</returns>
        public static string GetFolderPath(LinuxDataLocation location, string GameFolderName, Folders folder)
        {
            return BaseDirectoryPath(location) + GameFolderName + "/" + folder.ToString() + "/";
        }

        #endregion
    }
}
