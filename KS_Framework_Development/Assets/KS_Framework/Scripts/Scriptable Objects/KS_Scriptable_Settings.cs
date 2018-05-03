using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS_Core.Settings
{

    /// <summary>
    /// Settings container for in game settings which are saved to config between closure
    /// </summary>
    [CreateAssetMenu(fileName = "Settings", menuName = "KS: Framework/Databases/Settings", order = 4)]
    public class KS_Scriptable_Settings : ScriptableObject
    {

        public List<KS_Scriptable_Settings_menu> Menus = new List<KS_Scriptable_Settings_menu>();
    }

    /// <summary>
    /// In game options Menu object
    /// </summary>
    [System.Serializable]
    public class KS_Scriptable_Settings_menu
    {
        /// <summary>
        /// Display name of the options window
        /// </summary>
        [SerializeField] public string menuTitle;

        public List<KS_Scriptable_Settings_option> settings = new List<KS_Scriptable_Settings_option>();
    }

    /// <summary>
    /// Editing type for settings input
    /// </summary>
    [System.Serializable]
    public enum KS_Scriptable_Settings_Type
    {
        /// <summary>
        /// Step slider recomended for Int
        /// </summary>
        stepSlider,
        /// <summary>
        /// Slider recomended for Float or int
        /// </summary>
        slider,
        /// <summary>
        /// Bool toggle
        /// </summary>
        toggle,
        /// <summary>
        /// Drop down menu
        /// </summary>
        dropdown,
        /// <summary>
        /// String input
        /// </summary>
        key,
        /// <summary>
        /// Language change, Auto completed version of Dropdown
        /// </summary>
        Language
    }

    [System.Serializable]
    public class KS_Scriptable_Settings_option
    {
        /// <summary>
        /// Option ID and config save ID
        /// </summary>
        public string configID;
        /// <summary>
        /// Help text for this setting in the config
        /// </summary>
        public string configHelp;
        /// <summary>
        /// Text to display on the menu, localisation lineID compatable
        /// </summary>
        public string displayText;
        /// <summary>
        /// Setting editing type
        /// </summary>
        public KS_Scriptable_Settings_Type type;
        /// <summary>
        /// Default value
        /// </summary>
        public string defult;

        //
        /// <summary>
        /// Min value for int types
        /// </summary>
        public int minValue;
        /// <summary>
        /// Max value for int types
        /// </summary>
        public int Maxvalue;

        //
        /// <summary>
        /// Options to display on drop down menu
        /// </summary>
        public string[] dropdownOptions;


    }
}
