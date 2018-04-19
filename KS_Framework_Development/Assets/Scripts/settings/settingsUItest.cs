using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KS_Core;
using KS_Core.Settings;
using KS_Core.Localisation;

public class settingsUItest : MonoBehaviour {

    public KS_Scriptable_Settings settings;
    public KS_Settings settingsControler;

    //

    private int currentSettingsMenu = 0;
    public GameObject settingsContainer;

    // buttons

    public Button saveSettignsBtn;

    // Element containers

    public GameObject settingsTitleContainer;
    public GameObject settingsOptionContainer;
    public GameObject settingsOptionsPanel;

    // UI elements

    public GameObject ButtonPrefab;
    public GameObject SliderPrefab;
    public GameObject togglePrefab;
    public GameObject dropdownprefab;

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

        saveSettignsBtn.onClick.AddListener(delegate { SaveSettings(); });

        KS_Manager.OnLoadLevel += OnLevelLoad;
	}

    private void OnDestroy()
    {
        KS_Manager.OnLoadLevel -= OnLevelLoad;
    }

    private void OnLevelLoad(int i)
    {
        Destroy(gameObject);
    }

    private void SaveSettings()
    {
        settingsControler.Save();
    }

    void OnMenuClick(int i)
    {
        menuHolder[currentSettingsMenu].gameObject.SetActive(false);
        Debug.Log(i);
        menuHolder[i].gameObject.SetActive(true);

        currentSettingsMenu = i;
    }

    void OnLanguageClick(int i)
    {
        Debug.Log("Langauge: " + i);
        KS_Localisation.Instance.ChangeLanguage(i);
    }

    void OnChange(string key, string value)
    {
        settingsControler.SetString(key, value);
    }

    void PopulateSettingsMenu()
    {
        if (settings.Menus.Count > 0)
        {
            for (int i = 0; i < settings.Menus.Count; i++)
            {
                KS_Scriptable_Settings_menu m = settings.Menus[i];
                GameObject obj = GameObject.Instantiate(ButtonPrefab, settingsTitleContainer.transform);
                obj.GetComponentInChildren<KS_TextTranslate>().lineId = m.menuTitle;
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
            foreach(KS_Scriptable_Settings_menu m in settings.Menus)
            {
                GameObject parent = GameObject.Instantiate(settingsOptionsPanel, settingsOptionContainer.transform);
                parent.name = m.menuTitle;

                if(menuHolder.Count > 0)
                {
                    parent.SetActive(false);
                }

                if(m.settings.Count > 0)
                {
                    foreach(KS_Scriptable_Settings_option o in m.settings)
                    {
                        switch (o.type)
                        {
                            case KS_Scriptable_Settings_Type.slider:
                                GameObject obj = GameObject.Instantiate(SliderPrefab, parent.transform);
                                obj.GetComponentInChildren<KS_TextTranslate>().lineId = o.displayText;
                                obj.GetComponentInChildren<Slider>().minValue = o.minValue;
                                obj.GetComponentInChildren<Slider>().maxValue = o.Maxvalue;
                                obj.GetComponentInChildren<Slider>().value = float.Parse(settingsControler.GetSetting(o.configID));

                                var clickEvent = obj.GetComponentInChildren<Slider>().onValueChanged;
                                clickEvent.AddListener(delegate { OnChange(o.configID, obj.GetComponentInChildren<Slider>().value.ToString("0")); });

                                break;

                            case KS_Scriptable_Settings_Type.toggle:
                                GameObject obj2 = GameObject.Instantiate(togglePrefab, parent.transform);
                                obj2.GetComponentInChildren<KS_TextTranslate>().lineId = o.displayText;
                                obj2.GetComponentInChildren<Toggle>().isOn = bool.Parse(settingsControler.GetSetting(o.configID));

                                var clickEvent2 = obj2.GetComponentInChildren<Toggle>().onValueChanged;
                                clickEvent2.AddListener(delegate { OnChange(o.configID, obj2.GetComponentInChildren<Toggle>().isOn.ToString()); });

                                break;

                            case KS_Scriptable_Settings_Type.stepSlider:
                                GameObject obj3 = GameObject.Instantiate(SliderPrefab, parent.transform);
                                obj3.GetComponentInChildren<KS_TextTranslate>().lineId = o.displayText;
                                obj3.GetComponentInChildren<Slider>().wholeNumbers = true;
                                obj3.GetComponentInChildren<Slider>().minValue = o.minValue;
                                obj3.GetComponentInChildren<Slider>().maxValue = o.Maxvalue;
                                obj3.GetComponentInChildren<Slider>().value = float.Parse(settingsControler.GetSetting(o.configID));

                                var clickEvent3 = obj3.GetComponentInChildren<Slider>().onValueChanged;
                                clickEvent3.AddListener(delegate { OnChange(o.configID, obj3.GetComponentInChildren<Slider>().value.ToString("0")); });

                                break;

                            case KS_Scriptable_Settings_Type.Language:
                                GameObject obj4 = GameObject.Instantiate(dropdownprefab, parent.transform);
                                obj4.GetComponentInChildren<KS_TextTranslate>().lineId = o.displayText;

                                string[] languages = KS_Localisation.Instance.GetLanguages();
                                List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();

                                if (languages != null && languages.Length > 0)
                                {
                                    for (int i = 0; i < languages.Length; i++)
                                    {
                                        options.Add(new Dropdown.OptionData(languages[i]));
                                    }
                                }

                                obj4.GetComponentInChildren<Dropdown>().options = options;

                                Dropdown dropdown = obj4.GetComponentInChildren<Dropdown>();
                                Debug.Log(settingsControler.GetSetting(o.configID));
                                dropdown.value = int.Parse(settingsControler.GetSetting(o.configID));

                                var clickObject2 = obj4.GetComponentInChildren<Dropdown>().onValueChanged;
                                clickObject2.AddListener(delegate { OnLanguageClick(dropdown.value); });

                                var clickEvent4 = obj4.GetComponentInChildren<Dropdown>().onValueChanged;
                                clickEvent4.AddListener(delegate { OnChange(o.configID, obj4.GetComponentInChildren<Dropdown>().value.ToString()); });
                                break;

                            case KS_Scriptable_Settings_Type.dropdown:
                                GameObject obj5 = GameObject.Instantiate(dropdownprefab, parent.transform);
                                obj5.GetComponentInChildren<KS_TextTranslate>().lineId = o.displayText;

                                List<Dropdown.OptionData> options2 = new List<Dropdown.OptionData>();

                                for (int i = 0; i < o.dropdownOptions.Length; i++)
                                {
                                    options2.Add(new Dropdown.OptionData(o.dropdownOptions[i]));
                                }

                                Debug.Log("OPTIONS2: " + options2.Count);

                                obj5.GetComponentInChildren<Dropdown>().options = options2;

                                Dropdown dropdown2 = obj5.GetComponentInChildren<Dropdown>();
                                dropdown2.value = int.Parse(settingsControler.GetSetting(o.configID));
                                var clickObject3 = obj5.GetComponentInChildren<Dropdown>().onValueChanged;
                                //clickObject3.AddListener(delegate { void j(); });

                                var clickEvent5 = obj5.GetComponentInChildren<Dropdown>().onValueChanged;
                                clickEvent5.AddListener(delegate { OnChange(o.configID, obj5.GetComponentInChildren<Dropdown>().value.ToString()); });

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
