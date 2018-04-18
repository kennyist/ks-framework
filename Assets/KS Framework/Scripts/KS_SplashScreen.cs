using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KS_Core.Handlers;
using KS_Core.Splash;
using KS_Core;
using KS_Core.Input;
using UnityEngine.UI;

namespace KS_Core.Splash
{

    /// <summary>
    /// This class hadndles the running of the splash screens
    /// </summary>
    public class KS_SplashScreen : KS_Behaviour
    {
        /// <summary>
        /// On all splash screens shown
        /// </summary>
        public static event VoidHandler OnFinish;

        /// <summary>
        /// Splash screen objects <see cref="KS_SplashObject"/>
        /// </summary>
        public List<KS_SplashObject> splashScreens = new List<KS_SplashObject>();
        /// <summary>
        /// Are splash screens skippable
        /// </summary>
        public bool skippable = true;
        /// <summary>
        /// Input ID for skipping a splash screen, See KS input manager editor window
        /// </summary>
        public string skipInput = "skip_btn";
        /// <summary>
        /// Input ID for continuing, See KS input manager editor window
        /// </summary>
        public string waitForInputButton = "continue_btn";
        /// <summary>
        /// Fade overlay
        /// </summary>
        public Image fadeOverlay;

        /// <summary>
        /// Is the splash screen running
        /// </summary>
        private bool running = false;

        /// <summary>
        /// Current active splash screen
        /// </summary>
        private int screenIndex = -1;

        protected override void Awake()
        {
            base.Awake();

            for (int i = 0; i < splashScreens.Count; i++)
            {
                splashScreens[i].container.SetActive(false);
            }
        }

        protected override void OnManagerStart()
        {
            if (Manager.State == KS_Manager.GameState.Intro && !running)
            {
                StartSplash();
            }
        }

        protected override void OnGameStateChange(KS_Manager.GameState state)
        {
            if (state == KS_Manager.GameState.Intro && !running)
            {
                StartSplash();
            }
        }

        private void StartSplash()
        {

            if (splashScreens.Count <= 0)
            {
                Debug.Log("KS:FW - KS_SplashScreen:StartSplash() - No Splash Screens added");

                if (OnFinish != null)
                    OnFinish();
            }

            running = true;

            NextScreen();
        }

        float timer = 0f;
        float aliveTime = 0f;
        float fadeTimer = 0f;
        bool fadeIn = true;

        private void Update()
        {
            if (!running) return;

            // Inputs 

            if (KS_Input.GetInputDown(skipInput))
            {
                Skip();
            }

            if (KS_Input.GetInputDown(waitForInputButton))
            {
                Continue();
            }

            // Timer

            if (fadeTimer > 0)
            {
                timer += Time.deltaTime;

                if (fadeIn)
                {
                    SetFadeAlpha(1f - timer / fadeTimer);

                    if (timer > fadeTimer)
                    {
                        StartTimer();
                    }
                }
                else
                {
                    SetFadeAlpha(timer / fadeTimer);

                    if (timer > fadeTimer)
                    {
                        NextScreen();
                    }
                }
            }

            if (screenIndex < splashScreens.Count && !splashScreens[screenIndex].waitForInput)
            {
                if (aliveTime > 0f)
                {
                    timer += Time.deltaTime;


                    if (timer > aliveTime)
                    {
                        if (splashScreens[screenIndex].fadeInOut)
                        {
                            Fade(splashScreens[screenIndex].fadeTime, true);
                        }
                        else
                        {
                            NextScreen();
                        }
                    }
                }
            }
        }

        void NextScreen()
        {
            if (screenIndex >= 0)
            {
                splashScreens[screenIndex].container.SetActive(false);
            }

            screenIndex++;

            if (screenIndex < splashScreens.Count)
            {
                splashScreens[screenIndex].container.SetActive(true);

                if (splashScreens[screenIndex].fadeInOut)
                {
                    Fade(splashScreens[screenIndex].fadeTime);
                }
                else
                {
                    StartTimer();
                }
            }
            else
            {
                if (OnFinish != null)
                {
                    OnFinish();
                }

                Manager.SetGameState(KS_Manager.GameState.MainMenu);

                Destroy(gameObject);
            }
        }

        void Fade(float time, bool fadeOut = false)
        {
            aliveTime = 0;
            timer = 0f;
            fadeTimer = time;
            fadeIn = !fadeOut;
        }

        void SetFadeAlpha(float percent)
        {
            Color col = fadeOverlay.color;
            col.a = percent;
            fadeOverlay.color = col;
        }

        void StartTimer()
        {
            aliveTime = splashScreens[screenIndex].aliveTime;
            timer = 0f;
            fadeTimer = 0f;
        }

        private void Continue()
        {
            //Debug.Log("KS:FW - KS_SplashScreen:Continue() - Continue Pressed");

            if (splashScreens[screenIndex].waitForInput)
            {
                if (splashScreens[screenIndex].fadeInOut)
                {
                    Fade(splashScreens[screenIndex].fadeTime, true);
                }
                else
                {
                    NextScreen();
                }
            }
        }

        private void Skip()
        {
            //Debug.Log("KS:FW - KS_SplashScreen:Skip() - Skip Pressed");

            if (splashScreens[screenIndex].skippable)
            {
                if (splashScreens[screenIndex].fadeInOut)
                {
                    if (fadeTimer > 0f)
                    {
                        if (fadeIn)
                        {
                            fadeIn = false;
                            timer = splashScreens[screenIndex].fadeTime - timer;
                        }
                    }
                }
                else
                {
                    NextScreen();
                }
            }
        }
    }
}