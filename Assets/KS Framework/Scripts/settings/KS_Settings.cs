using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KS_Settings : MonoBehaviour {

    private static KS_Settings instance;
    public static KS_Settings Instance
    {
        get
        {
            if (instance != null) return instance;
            else return null;
        }
    }

    public KS_Settings_database settings;
    public string configName = "Settings";

    private KS_IniParser settingsConfig;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        settingsConfig = new KS_IniParser();

        if (!settingsConfig.DoesExist(configName))
        {
            Debug.Log("Doesnt exist");
            PopulateWithDefaults();
            settingsConfig.Save(configName);
            Debug.Log(configName + " created");
        }

        settingsConfig.Load(configName);
	}

    void PopulateWithDefaults()
    {
        if(settings.Menus.Count > 0)
        {
            foreach(KS_Settings_database.KS_Settings_database_menu m in settings.Menus)
            {
                if(m.settings.Count > 0)
                {
                    foreach(KS_Settings_database.KS_Settings_database_option o in m.settings)
                    {
                        settingsConfig.Set(m.menuTitle, o.configID, o.defult, o.configHelp);
                    } 
                }
            }
        }
    }

    public string GetSetting(string settingID)
    {
        return settingsConfig.Get(settingID);
    }

    public void SetString(string key, string value)
    {
        string[] setting = settingsConfig.GetLine(key);

        if(setting != null || setting.Length > -1)
        {
            settingsConfig.Set(setting[0], setting[1], value, setting[3]);
        }
    }

    public void Save()
    {
        settingsConfig.Save(configName);
    }
}
