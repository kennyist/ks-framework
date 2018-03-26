namespace KS_Core
{
    using UnityEngine;

    public abstract class KS_Behaviour : MonoBehaviour
    {

        private void Awake()
        {
            KS_Manager.OnManagerStart += OnManagerStart;
            KS_Manager.OnPause += OnPause;
            KS_Manager.OnPlay += OnPlay;
            KS_Manager.OnStateChange += OnGameStateChange;
            KS_Manager.OnLevelLoaded += OnLevelLoaded;
            KS_Manager.OnLoadLevel += OnLoadLevel;
        }

        private void OnDestroy()
        {
            KS_Manager.OnManagerStart -= OnManagerStart;
            KS_Manager.OnPause -= OnPause;
            KS_Manager.OnPlay -= OnPlay;
            KS_Manager.OnStateChange -= OnGameStateChange;
            KS_Manager.OnLevelLoaded -= OnLevelLoaded;
            KS_Manager.OnLoadLevel -= OnLoadLevel;
        }

        public KS_Manager Manager { get { return KS_Manager.Instance; } }

        public virtual void OnManagerStart() { }

        public virtual void OnLevelLoaded() { }

        public virtual void OnLoadLevel(int index) { }

        public virtual void OnGameStateChange(KS_Manager.GameState sate) { }

        public virtual void OnPause() { }

        public virtual void OnPlay() { }
    }
}
