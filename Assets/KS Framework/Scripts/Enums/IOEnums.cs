using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS_Core.IO
{
    /// <summary>
    /// Locations to save game data too in windows
    /// </summary>
    public enum WindowsDataLocation
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
    /// Locations to save game data to in Linux
    /// </summary>
    public enum LinuxDataLocation
    {
        /// <summary>
        /// Save in peronal folder
        /// </summary>
        Personal,
        /// <summary>
        /// Save in Local Application Data folder
        /// </summary>
        LocalApplicationData,
        /// <summary>
        /// Save in the Games instilation folder
        /// </summary>
        GameFolder
    }

    /// <summary>
    /// Locations to save game data to OSX
    /// </summary>
    public enum OSXDataLocation
    {
        /// <summary>
        /// Save to my documents
        /// </summary>
        MyDocuments,
        /// <summary>
        /// Save to my Games folder in my documents
        /// </summary>
        MyGames,
        /// <summary>
        /// Save to local application data folder
        /// </summary>
        LocalApplicationData,
        /// <summary>
        /// Save in the games instilation folder
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

}