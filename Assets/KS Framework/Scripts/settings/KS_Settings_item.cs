using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "KS_Framework/Settings/Database", order = 1)]
public class KS_Settings_database : ScriptableObject {

    public List<KS_Settings_database_menu> Menus = new List<KS_Settings_database_menu>();

    [System.Serializable]
    public class KS_Settings_database_menu
    {
        [SerializeField] public string menuTitle;

        public List<KS_Settings_database_option> settings = new List<KS_Settings_database_option>();
    }


    [System.Serializable]
    public class KS_Settings_database_option
    {
        public enum Type{
            slider,
            toggle,
            dropdown,
            key,
            radio,
        }

        public string displayText;
        public Type type;
        public string defult;

        //
        public int minValue;
        public int Maxvalue;


    }
}
