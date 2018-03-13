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

    public KS_Scriptable_Settings settings;
    public KS_Scriptable_GameConfig gameConfig;

    private KS_IniParser settingsConfig;

    private void Start()
    {
        instance = this;

        settingsConfig = new KS_IniParser();

        if (!settingsConfig.DoesExist(gameConfig.SettingsConfigName))
        {
            Debug.Log("Doesnt exist");
            PopulateWithDefaults();
            settingsConfig.Save(gameConfig.SettingsConfigName);
            Debug.Log(gameConfig.SettingsConfigName + " created");
        }

        settingsConfig.Load(gameConfig.SettingsConfigName);
	}

    void PopulateWithDefaults()
    {
        if(settings.Menus.Count > 0)
        {
            foreach(KS_Scriptable_Settings_menu m in settings.Menus)
            {
                if(m.settings.Count > 0)
                {
                    foreach(KS_Scriptable_Settings_option o in m.settings)
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
        Debug.Log(settingsConfig.Count());
        settingsConfig.Save(gameConfig.SettingsConfigName);
        Debug.Log(settingsConfig.Count());
    }
}
