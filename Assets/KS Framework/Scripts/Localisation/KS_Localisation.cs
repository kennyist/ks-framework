using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[ExecuteInEditMode]
public class KS_Localisation : MonoBehaviour {

    private static KS_Localisation instance;

    public static KS_Localisation Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }
            else
            {
                instance = new KS_Localisation();
                return instance;
            }
        }
    }

    public delegate void VoidDelegate();
    public static event VoidDelegate LanguageChanged;
    public KS_Scriptable_Translations translationFile;

    public KS_Scriptable_GameConfig gameConfig;
    private int selectedLanguage = 0;

    public KS_Localisation () {
        instance = this;

        ChangeLanguage(0);
	}

    private void OnLanguageChange()
    {
        if (LanguageChanged != null)
            LanguageChanged();
    }

    public bool ChangeLanguage(int index)
    {
        if(translationFile == null || translationFile.languages.Count <= 0 || index < 0 || index > translationFile.languages.Count)
        {
            return false;
        }

        selectedLanguage = index;

        OnLanguageChange();

        return true;
    }

    public bool ChangeLanguage(string language)
    {
        if (FindLanguage(language))
        {
            for(int i = 0; i < translationFile.languages.Count; i++)
            {
                if(translationFile.languages[i].language == language)
                {
                    selectedLanguage = i;
                }
            }
            return true;
        }

        OnLanguageChange();

        return false;
    }

    private bool FindLanguage(string name)
    {
        if (translationFile == null || translationFile.languages.Count <= 0) return false;

        foreach(KS_Scriptable_Translations_Language l in translationFile.languages)
        {
            if(name == l.language)
            {
                return true;
            }
        }

        return false;
    }

    public string[] GetLanguages()
    {
        if (translationFile == null || translationFile.languages.Count <= 0) return null;

        List<string> languages = new List<string>();

        foreach(KS_Scriptable_Translations_Language l in translationFile.languages)
        {
            languages.Add(l.language);
        }

        return (string[]) languages.ToArray();        
    }

    public string GetLine(string lineID)
    {
        if (translationFile == null || translationFile.languages.Count <= 0) return "";

        if(translationFile.languages[selectedLanguage].strings.Count > 0)
        {
            foreach (KS_Scriptable_Translations_Language.TranslationString s in translationFile.languages[selectedLanguage].strings)
            {
                if (s.lineID.ToLower() == lineID.ToLower()) return s.lineText;
            }
        }

        if (gameConfig.loc_returnNotFound)
            return gameConfig.loc_NotFoundLine;
        else
            return lineID;
    }

}
