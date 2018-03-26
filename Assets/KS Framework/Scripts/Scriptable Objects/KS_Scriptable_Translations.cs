using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS_Core.Localisation
{

    [CreateAssetMenu(fileName = "Translations", menuName = "KS_Framework/Translation/Database", order = 1)]
    [System.Serializable]
    public class KS_Scriptable_Translations : ScriptableObject
    {
        [SerializeField] public List<KS_Scriptable_Translations_Language> languages = new List<KS_Scriptable_Translations_Language>();
        [SerializeField] public string DefaultLanguage = "English";
    }  

    [System.Serializable]
    public class KS_Scriptable_Translations_Language
    {
        [SerializeField] public string language = "English";
        [SerializeField] public List<TranslationString> strings = new List<TranslationString>();
    }

    [System.Serializable]
    public class TranslationString
    {
        [SerializeField] public string lineID;
        [SerializeField] public string lineText ;
    }
}