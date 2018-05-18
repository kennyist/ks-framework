using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KS_Core.Input;
using KS_Core.Localisation;
using UnityEngine.UI;

public class Tut_Zone_Localisation : MonoBehaviour {

    int i = 0;

    public Text language;
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Q) || KS_Input.GetKeyDown(KS_Input.DS4ButtonToKey(DS4KeyCode.Triangle))){
            ChangeLanguage();
        }

        if (Input.GetKeyDown(KeyCode.E) || KS_Input.GetKeyDown(KS_Input.DS4ButtonToKey(DS4KeyCode.Square))){
            ShowSubtitle();
        }
    }

    void ChangeLanguage()
    {
        if (i < KS_Localisation.Instance.GetLanguages().Length -1) i++;
        else i = 0;

        KS_Localisation.Instance.ChangeLanguage(i);

        string line = KS_Localisation.Instance.GetLine("tut_language") + ": " + KS_Localisation.Instance.GetLanguages()[i];

        KS_Subtitle.Instance.ShowText(line);
        language.text = line;
    }

    void ShowSubtitle()
    {
        KS_Subtitle.Instance.ShowLocalisedText("tut_line" + Random.Range(1, 5));
    }
}
