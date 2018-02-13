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
                return new KS_Localisation();
            }
        }
    }


    private KS_FileHelper io;
    private ArrayList languages = new ArrayList();
    private Dictionary<string, string> loadedLines = new Dictionary<string, string>();
    private int loadedLanguage;

    void Start () {
        instance = this;

        io = KS_FileHelper.Instance;

        LoadLanguages();
        parseTranslationFile(0);
	}

    void LoadLanguages()
    {
        string[] lang = io.GetApplicationDataContents(KS_FileHelper.GameDataFolders.Translate);

        for(int i = 0; i < lang.Length; i++)
        {
            lang[i] = lang[i].Replace(".txt", "");
            languages.Add(lang[i]);
            Debug.Log(lang[i]);
        }
    }

    public bool LoadLanguage(int index)
    {
        return parseTranslationFile(index);
    }

    public bool LoadLanguage(string name)
    {
        return false;
    }

    public string[] Languages
    {
        get
        {
            return (string[])languages.ToArray();
        }
    }

    public string Language
    {
        get
        {
            return languages[loadedLanguage].ToString();
        }
    }

    public int LanguageIndex
    {
        get
        {
            return loadedLanguage;
        }
    }

    private bool parseTranslationFile(int index)
    {
        string file = io.LoadGameFile(KS_FileHelper.GameDataFolders.Translate, languages[index] + ".txt");

        if (file != null) {
            loadedLanguage = index;

            Debug.Log(file);

            string[] lines = file.Split(
                        new[] { Environment.NewLine },
                        StringSplitOptions.None
                    );

            foreach (string s in lines)
            {
                string[] line = s.Split(new[] { '=' }, 2);
                Debug.Log(line);

                Debug.Log(line[0] + " --- " + line[1]);

                loadedLines.Add(line[0], line[1]);
            }

            Debug.Log(loadedLines.Count + " Lines loaded");
            return true;
        }
        else
        {
            return false;
        }
    }

    public string GetLine(string lineID)
    {
        if (lineID.Length <= 0) return "";
        if (loadedLines.ContainsKey(lineID))
        {
            return loadedLines[lineID];
        }
        else
        {
            return "# Line not found #";
        }
    }

}
