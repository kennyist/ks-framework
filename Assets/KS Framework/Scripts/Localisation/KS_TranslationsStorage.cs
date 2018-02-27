using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Translations", menuName = "KS_Framework/Translation/Database", order = 1)]
[System.Serializable]
public class KS_Storage_Translations : ScriptableObject {

    [System.Serializable]
    public class Language
    {
        [System.Serializable]
        public class TranslationString
        {
            [SerializeField]public string lineID;
            [SerializeField] public string lineText;
        }

        [SerializeField] public string language = "English";
        [SerializeField] public List<TranslationString> strings = new List<TranslationString>();
    }

    [SerializeField] public List<Language> languages = new List<Language>();
    [SerializeField] public string DefaultLanguage = "English";
    
}
