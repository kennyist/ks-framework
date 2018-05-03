using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KS_Core.Localisation;

namespace KS_Core.Localisation
{
    /// <summary>
    /// Text game element using Localisation LineID to get the text in game. Updates automatically on language change.
    /// </summary>
    [AddComponentMenu("UI/Text Translate")]
    public class KS_TextTranslate : Text
    {
        /// <summary>
        /// Localisation LineID <see cref="KS_Scriptable_Translations"/>
        /// </summary>
        public string lineId = "";
        private string lastID = null;

        private void Awake()
        {
            Refresh();
        }

        protected override void OnEnable()
        {
            KS_Localisation.LanguageChanged += LanguageChanged;
            GetLine();
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            KS_Localisation.LanguageChanged -= LanguageChanged;
            base.OnDisable();
        }

        protected override void Start()
        {
            lastID = lineId;

            GetLine();

            base.Start();
        }

        private void LanguageChanged()
        {
            GetLine();
        }

        private bool GetLine()
        {
            if (lineId.Length > 0 && lineId != null)
            {
                text = KS_Localisation.Instance.GetLine(lineId).Replace("\\n", "\n");

                return true;
            }

            return false;
        }

        public void Update()
        {
            Refresh();
        }

        public void Refresh()
        {
            if (!lineId.Equals(lastID))
            {
                if (!GetLine())
                {

                    text = "";
                }

            }
        }
    }
}