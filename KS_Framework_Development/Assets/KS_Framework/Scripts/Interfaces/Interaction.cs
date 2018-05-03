using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS_Core
{
    /// <summary>
    /// Add interactablilty to objects from <see cref="KS_Character.KS_CharacterController">Character controller</see>
    /// </summary>
    public interface KS_Iinteractable
    {
        /// <summary>
        /// On player press use button on object
        /// </summary>
        void OnPress();
        /// <summary>
        /// On player hold use button on object
        /// </summary>
        void OnPressHold();
        /// <summary>
        /// On player release hold button on object
        /// </summary>
        void OnPressHoldLeave();
        /// <summary>
        /// On player hover over object
        /// </summary>
        void OnHover();
        /// <summary>
        /// On player leave hover over object
        /// </summary>
        void OnHoverLeave();
    }
}
