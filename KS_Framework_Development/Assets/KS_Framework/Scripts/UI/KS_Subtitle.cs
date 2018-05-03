using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using KS_Core.Input;

namespace KS_Core.Localisation
{
    /// <summary>
    /// Subtitle display
    /// </summary>
    public class KS_Subtitle : MonoBehaviour
    {

        private static KS_Subtitle instance;
        /// <summary>
        /// Current active instance of KS_Subtitle
        /// </summary>
        public static KS_Subtitle Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }
                else
                {
                    return new KS_Subtitle();
                }
            }
        }

        /// <summary>
        /// Subtitle display method
        /// </summary>
        public enum SubtitleMethod
        {
            /// <summary>
            /// Clears line current displayed text on new line
            /// </summary>
            LineByLine
        }

        /// <summary>
        /// Subtitle display text element
        /// </summary>
        public Text subMainText;
        /// <summary>
        /// Default time till clear displayed text
        /// </summary>
        public float DefaultMainShowTime = 5f;
        private float MainTimer = 0f;
        /// <summary>
        /// Secondary subtitle display element 
        /// </summary>
        public Text subSecondaryText;
        /// <summary>
        /// Default time till secondary text is cleared
        /// </summary>
        public float DefaultSecondaryShowTime = 2f;
        private float secondaryTimer = 0f;

        /// <summary>
        /// Display text on screen
        /// </summary>
        /// <param name="text">String to show</param>
        public void ShowText(string text)
        {
            subMainText.text = text;
            MainTimer = DefaultMainShowTime;
        }

        /// <summary>
        /// Displaye text on screen
        /// </summary>
        /// <param name="text">String to show</param>
        /// <param name="showTime">Time till cleared</param>
        public void ShowText(string text, float showTime)
        {
            subMainText.text = text;
            MainTimer = showTime;
        }
        
        /// <summary>
        /// Display string from localisation
        /// </summary>
        /// <param name="lineid">Localisation lineID <see cref="KS_Scriptable_Translations"/></param>
        public void ShowLocalisedText(string lineid)
        {
            string text = KS_Localisation.Instance.GetLine(lineid);
            subMainText.text = text;
            MainTimer = DefaultMainShowTime;
        }

        /// <summary>
        /// Display string from localisation with timer
        /// </summary>
        /// <param name="lineid">Localisation lineID <see cref="KS_Scriptable_Translations"/></param>
        /// <param name="showTime">Time in seconds till text is cleared</param>
        public void ShowLocalisedText(string lineid, float showTime)
        {
            string text = KS_Localisation.Instance.GetLine(lineid);
            subMainText.text = text;
            MainTimer = showTime;
        }

        /// <summary>
        /// Display localisation string on screen with no auto clear
        /// </summary>
        /// <param name="lineID">Localisation lineID <see cref="KS_Scriptable_Translations"/></param>
        public void PermShowLocalisedText(string lineID)
        {
            string text = KS_Localisation.Instance.GetLine(lineID);
            subMainText.text = text;
        }

        /// <summary>
        /// Display text on screen with no auto clear
        /// </summary>
        /// <param name="text">Text to display</param>
        public void PermShowText(string text)
        {
            subMainText.text = text;
        }

        /// <summary>
        /// Clear all text displayed
        /// </summary>
        public void Clear()
        {
            subMainText.text = "";
            subSecondaryText.text = "";
        }

        // Use this for initialization
        void Start()
        {
            instance = this;
        }

        // Update is called once per frame
        void Update()
        {
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
        }
    }
}