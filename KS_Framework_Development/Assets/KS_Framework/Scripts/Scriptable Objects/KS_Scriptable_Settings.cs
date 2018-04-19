using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS_Core.Settings
{

    [CreateAssetMenu(fileName = "Settings", menuName = "KS_Framework/Settings/Database", order = 1)]
    public class KS_Scriptable_Settings : ScriptableObject
    {

        public List<KS_Scriptable_Settings_menu> Menus = new List<KS_Scriptable_Settings_menu>();
    }

    [System.Serializable]
    public class KS_Scriptable_Settings_menu
    {
        [SerializeField] public string menuTitle;

        public List<KS_Scriptable_Settings_option> settings = new List<KS_Scriptable_Settings_option>();
    }

    [System.Serializable]
    public enum KS_Scriptable_Settings_Type
    {
        stepSlider,
        slider,
        toggle,
        dropdown,
        key,
        Language
    }

    [System.Serializable]
    public class KS_Scriptable_Settings_option
    {

        public string configID;
        public string configHelp;
        public string displayText;
        public KS_Scriptable_Settings_Type type;
        public string defult;

        //
        public int minValue;
        public int Maxvalue;

        //
        public string[] dropdownOptions;


    }
}
