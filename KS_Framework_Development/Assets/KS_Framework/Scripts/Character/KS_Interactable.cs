using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KS_Core;
using KS_Core.Localisation;

namespace KS_Utility
{
    /// <summary>
    /// Simple interactable object
    /// </summary>
    public class KS_Interactable : KS_Behaviour, KS_Iinteractable
    {
        public string hoverText = "";

        public void OnHover()
        {
            Debug.Log("Hover");
            KS_Subtitle.Instance.PermShowText(hoverText);
        }

        public void OnHoverLeave()
        {
            KS_Subtitle.Instance.Clear();
        }

        public void OnPress()
        {
            Debug.Log("pressed");
        }

        public void OnPressHold()
        {
            
        }

        public void OnPressHoldLeave()
        {
            
        }
    }
}