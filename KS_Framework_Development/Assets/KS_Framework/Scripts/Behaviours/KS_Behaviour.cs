using UnityEngine;

namespace KS_Core
{
    /// <summary>
    /// Basic KS Framework behaviour inheriting Monobehaviour. This behaviour provides a few overridable callbacks for core framework events.
    /// <inheritdoc cref="MonoBehaviour"/>
    /// </summary>
    public abstract class KS_Behaviour : MonoBehaviour
    {
        /// <summary>
        /// Register Event listeners
        /// </summary>
        protected virtual void Awake()
        {
            KS_Manager.OnManagerStart += OnManagerStart;
            KS_Manager.OnPause += OnPause;
            KS_Manager.OnPlay += OnPlay;
            KS_Manager.OnStateChange += OnGameStateChange;
            KS_Manager.OnLevelLoaded += OnLevelLoaded;
            KS_Manager.OnLoadLevel += OnLoadLevel;
        }

        /// <summary>
        /// Unregister event listeners
        /// </summary>
        protected virtual void OnDestroy()
        {
            KS_Manager.OnManagerStart -= OnManagerStart;
            KS_Manager.OnPause -= OnPause;
            KS_Manager.OnPlay -= OnPlay;
            KS_Manager.OnStateChange -= OnGameStateChange;
            KS_Manager.OnLevelLoaded -= OnLevelLoaded;
            KS_Manager.OnLoadLevel -= OnLoadLevel;
        }

        /// <summary>
        /// Current instance of KS_manager
        /// <seealso cref="KS_Manager"/>
        /// </summary>
        public KS_Manager Manager { get { return KS_Manager.Instance; } }

        /// <summary>
        /// Current KS Manager game state
        /// </summary>
        public KS_Manager.GameState GameState { get { return KS_Manager.Instance.State; } }

        /// <summary>
        /// Overridable callback: On manager start, called when manager has finished setup
        /// </summary>
        protected virtual void OnManagerStart() { }

        /// <summary>
        /// Overridable callback: On level loaded, Called when a new level has been fully loaded
        /// </summary>
        protected virtual void OnLevelLoaded() { }

        /// <summary>
        /// Overridable callback: On load level, Called when a new level has been called from the manager.
        /// </summary>
        /// <param name="index">Level index sent to the KS_Manager</param>
        protected virtual void OnLoadLevel(int index) { }

        /// <summary>
        /// Overridable callback: On Game State Change, Called when game state has been changed in the game manager.
        /// </summary>
        /// <param name="state">KS_manager game state</param>
        protected virtual void OnGameStateChange(KS_Manager.GameState state) { }

        /// <summary>
        /// Overrideable callback: On Pause, Called when the game has been paused.
        /// </summary>
        protected virtual void OnPause() { }

        /// <summary>
        /// Overridable callback: On Player, Called when the game has been unpaused.
        /// </summary>
        protected virtual void OnPlay() { }
    }
}
