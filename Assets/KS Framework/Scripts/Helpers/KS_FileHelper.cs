using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;

namespace KS_Core.IO
{

    public class KS_FileHelper
    {

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

        private string[] ignoreExt = { ".meta" };

        private KS_Scriptable_GameConfig gameConfig;

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
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public string GetFolderPath(WindowsDataLocation location, Folders folder)
        {
            return BaseDirectoryPath(location) + gameConfig.gameName + "/" + folder.ToString() + "/";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public string GetFolderPath(LinuxDataLocation location, Folders folder)
        {
            return BaseDirectoryPath(location) + gameConfig.gameName + "/" + folder.ToString() + "/";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public string GetFolderPath(OSXDataLocation location, Folders folder)
        {
            return BaseDirectoryPath(location) + gameConfig.gameName + "/" + folder.ToString() + "/";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public byte[] LoadFileToBytes(Folders folder, string fileName)
        {
            string basePath = GetFolderPathByOS(folder);

            return File.ReadAllBytes(basePath + fileName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string LoadFileToString(Folders folder, string fileName)
        {
            string basePath = GetFolderPathByOS(folder);

            return File.ReadAllText(basePath + fileName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="file"></param>
        /// <param name="newName"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="file"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool GetFile(Folders folder, string filename)
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
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static string GetDirectoryPath(WindowsDataLocation location)
        {
            return BaseDirectoryPath(location);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static string GetDirectoryPath(OSXDataLocation location)
        {
            return BaseDirectoryPath(location);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static string GetDirectoryPath(LinuxDataLocation location)
        {
            return BaseDirectoryPath(location);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="GameFolderName"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static string GetFolderPath(WindowsDataLocation location, string GameFolderName, Folders folder)
        {
            return BaseDirectoryPath(location) + GameFolderName + "/" + folder.ToString() + "/";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="GameFolderName"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static string GetFolderPath(OSXDataLocation location, string GameFolderName, Folders folder)
        {
            return BaseDirectoryPath(location) + GameFolderName + "/" + folder.ToString() + "/";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="GameFolderName"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static string GetFolderPath(LinuxDataLocation location, string GameFolderName, Folders folder)
        {
            return BaseDirectoryPath(location) + GameFolderName + "/" + folder.ToString() + "/";
        }

        #endregion
    }
}
