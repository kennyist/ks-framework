using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using KS_Core.Localisation;

public class KS_Subtitle : MonoBehaviour {

    private KS_Subtitle instance;
    public KS_Subtitle Instance
    {
        get
        {
            if(instance != null)
            {
                return instance;
            }
            else
            {
                return new KS_Subtitle();
            }
        }
    }

    public enum SubtitleMethod
    {
        LineByLine
    }

    public Text subMainText;
    public float DefaultMainShowTime = 5f;
    private float MainTimer = 0f;
    public Text subSecondaryText;
    public float DefaultSecondaryShowTime = 2f;
    private float secondaryTimer = 0f;

    public void ShowText(string text)
    {
        subMainText.text = text;
        MainTimer = DefaultMainShowTime;
    }

    public void ShowText(string text, float showTime)
    {
        subMainText.text = text;
        MainTimer = showTime;
    }

    public void ShowLocalisedText(string lineid)
    {
        string text = KS_Localisation.Instance.GetLine(lineid);
        subMainText.text = text;
        MainTimer = DefaultMainShowTime;
    }

    public void ShowLocalisedText(string lineid, float showTime)
    {
        string text = KS_Localisation.Instance.GetLine(lineid);
        subMainText.text = text;
        MainTimer = showTime;
    }

	// Use this for initialization
	void Start () {
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
        if (MainTimer <= 0f)
        {
            if (subMainText.text != "")
            {
                subMainText.text = "";
            }
        }
        else
        {
            MainTimer -= Time.deltaTime;
        }

        if (secondaryTimer <= 0f)
        {
            if (subSecondaryText.text != "")
            {
                subSecondaryText.text = "";
            }
        }
        else
        {
            secondaryTimer -= Time.deltaTime;
        }



        // testing

        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            ShowText("Testing the subtitles, something about lipsum");
        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            ShowText("Testing the subtitles with custom timer, something about lipsum", 3f);
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            ShowLocalisedText("test_string");
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            ShowLocalisedText("subtest", 3f);
        }

        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            KS_Localisation.Instance.ChangeLanguage(1);
            ShowText("Changed to german", 1f);
            Debug.Log(KS_Localisation.Instance.GetLine("TestString"));
        }

        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            KS_Localisation.Instance.ChangeLanguage(0);
            ShowText("Changed to english", 1f);
            Debug.Log(KS_Localisation.Instance.GetLine("TestString"));
        }
    }
}
