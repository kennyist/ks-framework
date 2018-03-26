using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS_Core.IO
{
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

    public enum LinuxDataLocation
    {
        Personal,
        LocalApplicationData,
        GameFolder
    }

    public enum OSXDataLocation
    {
        MyDocuments,
        MyGames,
        LocalApplicationData,
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