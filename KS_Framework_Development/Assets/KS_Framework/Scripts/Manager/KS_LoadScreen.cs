using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using KS_Core;
using KS_Core.Input;

namespace KS_Utility
{
    /// <summary>
    /// Simple load screen display when loading a new level
    /// </summary>
    public class KS_LoadScreen : KS_Behaviour
    {
        /// <summary>
        /// Container of the load screen objects
        /// </summary>
        public GameObject LoadScreenContainer;
        /// <summary>
        /// Wait for input before closing the screen once level is loaded
        /// </summary>
        public bool waitForInput = false;
        /// <summary>
        /// Input ID for closing the screen <see cref="KS_Scriptable_Input"/>
        /// </summary>
        public string waitInputID = "intro_continue";

        private bool loaded = false;

        /// <summary>
        /// Is the level currently loading
        /// </summary>
        public bool Loading
        {
            get { return loading; }
        }

        protected override void OnLoadLevel(int index)
        {
            base.OnLoadLevel(index);
            Debug.Log("Loading level: " + index);
            StartCoroutine(LoadScene(index));
        }

        // Use this for initialization
        void Start()
        {
            DontDestroyOnLoad(this.gameObject);
            LoadScreenContainer.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

            if (loaded)
            {
                if (waitForInput)
                {
                    if (KS_Input.GetInputDown(waitInputID)) 
                    {
                        LoadScreenContainer.SetActive(false);
                        loaded = false;

                        KS_Manager.Instance.LevelLoaded();
                    }
                }
                else
                {
                    LoadScreenContainer.SetActive(false);
                    loaded = false;

                    KS_Manager.Instance.LevelLoaded();
                }
            }
        }

        private float loadProgress = 0f;
        private bool loading = false;

        private IEnumerator LoadScene(int scene)
        {
            LoadScreenContainer.SetActive(true);
            loading = true;

            AsyncOperation async = SceneManager.LoadSceneAsync(scene);

            while (async.isDone)
            {
                loadProgress = async.progress;
                yield return null;
            }

            loading = false;
            loaded = true;
        }
    }
}