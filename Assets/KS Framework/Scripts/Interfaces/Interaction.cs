using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS_Core
{
    public interface KS_Iinteractable
    {
        string HoverText { get; set; }
        void OnHover();
        void OnLeaveHover();
        void OnPress();
    }
}
