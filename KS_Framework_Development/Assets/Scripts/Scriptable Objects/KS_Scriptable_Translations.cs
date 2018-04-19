using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS_Core.Localisation
{
    /// <summary>
    /// Translation Storeage
    /// </summary>
    [CreateAssetMenu(fileName = "Translations", menuName = "KS_Framework/Translation/Database", order = 1)]
    [System.Serializable]
    public class KS_Scriptable_Translations : ScriptableObject
    {
        /// <summary>
        /// List of Languages
        /// </summary>
        [SerializeField] public List<KS_Scriptable_Translations_Language> languages = new List<KS_Scriptable_Translations_Language>();
        /// <summary>
        /// Default Language
        /// </summary>
        [SerializeField] public string DefaultLanguage = "English";
    }  

    /// <summary>
    /// Language container 
    /// </summary>
    [System.Serializable]
    public class KS_Scriptable_Translations_Language
    {
        /// <summary>
        /// Language name
        /// </summary>
        [SerializeField] public string language = "English";
        /// <summary>
        /// strings stored in this language
        /// </summary>
        [SerializeField] public List<TranslationString> strings = new List<TranslationString>();
    }

    /// <summary>
    /// Translation string
    /// </summary>
    [System.Serializable]
    public class TranslationString
    {
        /// <summary>
        /// String ID (Unique)
        /// </summary>
        [SerializeField] public string lineID;
        /// <summary>
        /// Translation text
        /// </summary>
        [SerializeField] public string lineText ;
    }
}