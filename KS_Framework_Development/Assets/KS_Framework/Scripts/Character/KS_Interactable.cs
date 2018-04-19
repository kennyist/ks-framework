using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KS_Core;

namespace KS_Utility
{
    /// <summary>
    /// Simple interactable object
    /// </summary>
    public class KS_Interactable : KS_Behaviour, KS_Iinteractable
    {
        public string hoverText = "Some text to show";

        public string HoverText
        {
            get
            {
                return hoverText;
            }

            set
            {
                hoverText = value;
            }
        }

        public void OnHover()
        {

        }

        public void OnLeaveHover()
        {

        }

        public void OnPress()
        {
            Debug.Log("pressed");
        }
    }
}