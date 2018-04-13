using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using KS_Core.Handlers;

namespace KS_Core.Localisation
{
    /// <summary>
    /// 
    /// </summary>
    [ExecuteInEditMode]
    public class KS_Localisation : MonoBehaviour
    {

        private static KS_Localisation instance;
        /// <summary>
        /// Current active instance of KS_Localisation
        /// </summary>
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

        /// <summary>
        /// On Language changed
        /// </summary>
        public static event VoidHandler LanguageChanged;
        /// <summary>
        /// <see cref="KS_Scriptable_Translations">Translation file</see>
        /// </summary>
        public KS_Scriptable_Translations translationFile;
        /// <summary>
        /// <see cref="KS_Scriptable_GameConfig">Game Config file </see>
        /// </summary>
        public KS_Scriptable_GameConfig gameConfig;
        private int selectedLanguage = 0;

        /// <summary>
        /// Initialize new KS_Localistaion object
        /// </summary>
        public KS_Localisation()
        {
            instance = this;

            ChangeLanguage(0);
        }

        private void OnLanguageChange()
        {
            if (LanguageChanged != null)
                LanguageChanged();
        }

        /// <summary>
        /// Change the current selected language
        /// </summary>
        /// <param name="index">Langauge index</param>
        /// <returns>True if changed, False if language not found</returns>
        public bool ChangeLanguage(int index)
        {
            if (translationFile == null || translationFile.languages.Count <= 0 || index < 0 || index > translationFile.languages.Count)
            {
                return false;
            }

            selectedLanguage = index;

            OnLanguageChange();

            return true;
        }

        /// <summary>
        /// Change the current selected language
        /// </summary>
        /// <param name="language">language name</param>
        /// <returns>True if language changed, false if language not found</returns>
        public bool ChangeLanguage(string language)
        {
            if (FindLanguage(language))
            {
                for (int i = 0; i < translationFile.languages.Count; i++)
                {
                    if (translationFile.languages[i].language == language)
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

            foreach (KS_Scriptable_Translations_Language l in translationFile.languages)
            {
                if (name == l.language)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Get all languages
        /// </summary>
        /// <returns>Names of all langauges in string array</returns>
        public string[] GetLanguages()
        {
            if (translationFile == null || translationFile.languages.Count <= 0) return null;

            List<string> languages = new List<string>();

            foreach (KS_Scriptable_Translations_Language l in translationFile.languages)
            {
                languages.Add(l.language);
            }

            return (string[])languages.ToArray();
        }

        /// <summary>
        /// Get localisation line for selected langauge 
        /// </summary>
        /// <param name="lineID">Line ID set in Translation Manager editor window</param>
        /// <returns>Translation line, not found line if lineID not found (Changeable in game config)</returns>
        public string GetLine(string lineID)
        {
            if (translationFile == null || translationFile.languages.Count <= 0) return "";

            if (translationFile.languages[selectedLanguage].strings.Count > 0)
            {
                foreach (TranslationString s in translationFile.languages[selectedLanguage].strings)
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
}
