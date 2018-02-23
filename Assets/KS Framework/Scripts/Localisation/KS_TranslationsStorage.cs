using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Translations", menuName = "KS_Framework/Translation/Database", order = 1)]
public class KS_Storage_Translations : ScriptableObject {

    [System.Serializable]
    public class Language
    {
        [System.Serializable]
        public class TranslationString
        {
            public string lineID;
            public string lineText;
        }

        public string language = "English";
        public List<TranslationString> strings = new List<TranslationString>();
    }

    public List<Language> languages = new List<Language>();
    public string DefaultLanguage = "English";
    public string SelectedLanguage = "English";
    
}
