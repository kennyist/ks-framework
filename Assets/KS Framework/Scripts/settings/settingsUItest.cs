using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class settingsUItest : MonoBehaviour {

    public KS_Settings_database settings;

    public GameObject settingsContainer;

    // Element containers

    public GameObject settingsTitleContainer;
    public GameObject settingsOptionContainer;

    // UI elements

    public GameObject ButtonPrefab;
    public GameObject SliderPrefab;
    public GameObject togglePrefab;

	// Use this for initialization
	void Start () {
        PopulateSettings();
	}

    void PopulateSettings()
    {
        if(settings.Menus.Count > 0)
        {
            foreach(KS_Settings_database.KS_Settings_database_menu m in settings.Menus)
            {
                GameObject obj = GameObject.Instantiate(ButtonPrefab, settingsTitleContainer.transform);
                obj.name = "!!";
                obj.GetComponentInChildren<Text>().text = m.menuTitle;
            }
        }

        if(settings.Menus[0].settings.Count > 0)
        {
            foreach(KS_Settings_database.KS_Settings_database_option o in settings.Menus[0].settings)
            {
                switch (o.type)
                {
                    case KS_Settings_database.KS_Settings_database_option.Type.slider:
                        GameObject obj = GameObject.Instantiate(SliderPrefab, settingsOptionContainer.transform);
                        obj.GetComponentInChildren<Text>().text = o.displayText;
                        obj.GetComponentInChildren<Slider>().minValue = o.minValue;
                        obj.GetComponentInChildren<Slider>().maxValue = o.Maxvalue;
                        obj.GetComponentInChildren<Slider>().value = float.Parse(o.defult);

                        Debug.Log(float.Parse(o.defult));
                        

                        break;

                    case KS_Settings_database.KS_Settings_database_option.Type.toggle:
                        GameObject obj2 = GameObject.Instantiate(togglePrefab, settingsOptionContainer.transform);
                        obj2.GetComponentInChildren<Text>().text = o.displayText;
                        obj2.GetComponentInChildren<Toggle>().isOn = bool.Parse(o.defult);

                        break;

                }
            }
        }
    }

	
	// Update is called once per frame
	void Update () {
		
	}
}
