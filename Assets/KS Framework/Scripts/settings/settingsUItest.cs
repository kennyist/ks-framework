using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class settingsUItest : MonoBehaviour {

    public KS_Settings_database settings;
    public KS_Settings settingsControler;

    //

    private int currentSettingsMenu = 0;
    public GameObject settingsContainer;

    // Element containers

    public GameObject settingsTitleContainer;
    public GameObject settingsOptionContainer;
    public GameObject settingsOptionsPanel;

    // UI elements

    public GameObject ButtonPrefab;
    public GameObject SliderPrefab;
    public GameObject togglePrefab;

    private List<Holder> menuHolder = new List<Holder>();

    //

    class Holder
    {
        public GameObject gameObject;
        public bool active;

        public  Holder(GameObject o, bool b)
        {
            gameObject = o;
            active = b;
        }
    }

	// Use this for initialization
	void Start () {
        settingsControler = KS_Settings.Instance;
        PopulateSettingsMenu();
        PopulateSettings();
	}

    void OnMenuClick(int i)
    {
        menuHolder[currentSettingsMenu].gameObject.SetActive(false);
        Debug.Log(i);
        menuHolder[i].gameObject.SetActive(true);

        currentSettingsMenu = i;
    }

    void PopulateSettingsMenu()
    {
        if (settings.Menus.Count > 0)
        {
            for (int i = 0; i < settings.Menus.Count; i++)
            {
                KS_Settings_database.KS_Settings_database_menu m = settings.Menus[i];
                GameObject obj = GameObject.Instantiate(ButtonPrefab, settingsTitleContainer.transform);
                obj.GetComponentInChildren<Text>().text = m.menuTitle;
                var clickEvent = obj.GetComponent<Button>().onClick;
                int x = i;
                clickEvent.AddListener(() => OnMenuClick(x));

                obj = null;
            }
        }
    }

    void PopulateSettings()
    {
        if(settings.Menus[0].settings.Count > 0)
        {
            foreach(KS_Settings_database.KS_Settings_database_menu m in settings.Menus)
            {
                GameObject parent = GameObject.Instantiate(settingsOptionsPanel, settingsOptionContainer.transform);
                parent.name = m.menuTitle;

                if(menuHolder.Count > 0)
                {
                    parent.SetActive(false);
                }

                if(m.settings.Count > 0)
                {
                    foreach(KS_Settings_database.KS_Settings_database_option o in m.settings)
                    {
                        switch (o.type)
                        {
                            case KS_Settings_database.KS_Settings_database_option.Type.slider:
                                GameObject obj = GameObject.Instantiate(SliderPrefab, parent.transform);
                                obj.GetComponentInChildren<Text>().text = o.displayText;
                                obj.GetComponentInChildren<Slider>().minValue = o.minValue;
                                obj.GetComponentInChildren<Slider>().maxValue = o.Maxvalue;
                                obj.GetComponentInChildren<Slider>().value = float.Parse(settingsControler.GetSetting(o.configID));

                                break;

                            case KS_Settings_database.KS_Settings_database_option.Type.toggle:
                                GameObject obj2 = GameObject.Instantiate(togglePrefab, parent.transform);
                                obj2.GetComponentInChildren<Text>().text = o.displayText;
                                obj2.GetComponentInChildren<Toggle>().isOn = bool.Parse(settingsControler.GetSetting(o.configID));

                                break;

                            case KS_Settings_database.KS_Settings_database_option.Type.stepSlider:
                                GameObject obj3 = GameObject.Instantiate(SliderPrefab, parent.transform);
                                obj3.GetComponentInChildren<Text>().text = o.displayText;
                                obj3.GetComponentInChildren<Slider>().wholeNumbers = true;
                                obj3.GetComponentInChildren<Slider>().minValue = o.minValue;
                                obj3.GetComponentInChildren<Slider>().maxValue = o.Maxvalue;
                                obj3.GetComponentInChildren<Slider>().value = float.Parse(settingsControler.GetSetting(o.configID));

                                break;
                        }
                    }
                }

                menuHolder.Add(new Holder(parent, false));
            }
        }
    }

	
	// Update is called once per frame
	void Update () {
		
	}
}
