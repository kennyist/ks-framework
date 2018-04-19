using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS_Core.Splash
{
    /// <summary>
    /// Splash screen object options container
    /// </summary>
    [System.Serializable]
    public class KS_SplashObject
    {
        /// <summary>
        /// The parent object to all items in this splash instance
        /// </summary>
        [SerializeField]
        public GameObject container;
        /// <summary>
        /// Time for the spash screen to stay on screen
        /// </summary>
        [SerializeField]
        public float aliveTime = 5f;
        /// <summary>
        /// Can the player skip this splash screen
        /// </summary>
        [SerializeField]
        public bool skippable = false;
        /// <summary>
        /// Fade in/out the splash (true), instantly swap (false)
        /// </summary>
        [SerializeField]
        public bool fadeInOut = true;
        /// <summary>
        /// Time to fade this splash screen
        /// </summary>
        [SerializeField]
        public float fadeTime = 1f;
        /// <summary>
        /// Wait for input to continue past the splace screen (true)
        /// </summary>
        [SerializeField]
        public bool waitForInput = false;
    }

}
