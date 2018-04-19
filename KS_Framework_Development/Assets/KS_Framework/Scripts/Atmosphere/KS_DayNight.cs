using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KS_Core.IO;
using KS_Core.GameTime;
using KS_Core.Atmosphere.SunCalc;

namespace KS_Core.Atmosphere
{
    /// <summary>
    /// Day night cycle, This script handles the movement of the sun and stars aswell as updating the environment lighting. 
    /// Sun position is found via longitude and latitude and time of the day.
    /// </summary>
    public class KS_DayNight : KS_Behaviour
    {

        private KS_DayNight instance;
        /// <summary>
        /// Current active instance of KS_DayNight
        /// </summary>
        public KS_DayNight Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }
                else
                {
                    return null;
                }
            }
        }

        // Time management

        KS_TimeManager timeManager;

        // Environment Settings
        /// <summary>
        /// Colour gradiant of ambient light during the cycle
        /// </summary>
        public Gradient nightDayColour = new Gradient();
        /// <summary>
        /// Light intensity by time of the day
        /// </summary>
        public AnimationCurve Intensity = new AnimationCurve();
        /// <summary>
        /// Ambient light level by time of the day
        /// </summary>
        public AnimationCurve ambient = new AnimationCurve();

        /// <summary>
        /// Night and day fog colour by time of the day
        /// </summary>
        public Gradient nightDayFogColour = new Gradient();
        /// <summary>
        /// Fog density by time of the day
        /// </summary>
        public AnimationCurve fogDensity = new AnimationCurve();
        /// <summary>
        /// Fog scale by time of the day
        /// </summary>
        public AnimationCurve fogScale = new AnimationCurve();

        /// <summary>
        /// Atmospshere thickness during the day
        /// </summary>
        public float dayAtmospherThickness = 0.04f;
        /// <summary>
        /// Atmosphere thickness during the night
        /// </summary>
        public float nightAtmosphereThickness = 0.87f;

        private float skySpeed;
        private Light mainLight;
        private Skybox sky;
        private Material skyMat;

        // World data
        /// <summary>
        /// World lattitude for scene location
        /// </summary>
        public double latitude = 50.376316;
        /// <summary>
        /// World Longitude for scene location
        /// </summary>
        public double longitude = -4.123820;
        /// <summary>
        /// The timezone used in the long/lat location specified, if not set day night cycle will be offset
        /// </summary>
        public int timeZone = 0;

        // Sun Manager

        /// <summary>
        /// Game object parent of the sun (needed for rotation)
        /// </summary>
        public GameObject SunContanier;
        /// <summary>
        /// Game object of the sun
        /// </summary>
        public GameObject sun;

        // Star system
        /// <summary>
        /// Game object parent of the star
        /// </summary>
        public Transform starContainer;
        /// <summary>
        /// Star opacity by time of the day
        /// </summary>
        public AnimationCurve starIntensity;


        protected override void Awake()
        {
            if (instance != null) Destroy(this);
            instance = this;
            StarAwake();

            KS_SaveLoad.OnSave += OnSave;
            KS_SaveLoad.OnLoad += OnLoad;

            base.Awake();
        }

        protected override void OnDestroy()
        {
            KS_SaveLoad.OnSave -= OnSave;
            KS_SaveLoad.OnLoad -= OnLoad;

            instance = null;

            base.OnDestroy();
        }

        // Use this for initialization
        void Start()
        {
            StarStart();

            timeManager = KS_TimeManager.Instance;

            timeManager.OnTimeUpdate += OnTimeUpdate;

            mainLight = sun.GetComponent<Light>();
            skyMat = RenderSettings.skybox;
        }

        void OnSave(ref Dictionary<string, object> saveGame)
        {
        }

        void OnLoad(KS_SaveGame saveGame)
        {

        }

        void OnTimeUpdate(int h, int m, int s, KS_TimeManager.DayTimeZone zone)
        {
            UpdateSunPos(h, m, s);
            UpdateEnvironment();
        }

        private void UpdateSunPos(int h, int m, int s)
        {
            Vector2 sunPos = KS_SunCalc.CalculateSunPosition(h, m, s, latitude, longitude);
            
            Vector3 sRot = new Vector3(sunPos.x, 0, 0);
            Vector3 cRot = new Vector3(0, sunPos.y, 0);

            sun.transform.localRotation = Quaternion.Euler(sRot);
            SunContanier.transform.localRotation = Quaternion.Euler(cRot);
            starContainer.transform.rotation = Quaternion.Euler(new Vector3((float)latitude, 0, 360 * timeManager.TimeOfDay));
        }

        private void UpdateEnvironment()
        {
            float dot = timeManager.TimeOfDay;

            mainLight.intensity = Intensity.Evaluate(dot);
            RenderSettings.ambientIntensity = ambient.Evaluate(dot);
            mainLight.color = nightDayColour.Evaluate(dot);
            RenderSettings.ambientLight = mainLight.color;

            RenderSettings.fogColor = nightDayFogColour.Evaluate(dot);
            RenderSettings.fogDensity = fogDensity.Evaluate(dot);

            float i = ((dayAtmospherThickness - nightAtmosphereThickness) * dot) + nightAtmosphereThickness;
            skyMat.SetFloat("_AtmosphereThickness", i);

            SetStarIntensity(dot);
        }

        // Star System

        /// <summary>
        /// Star partical system
        /// </summary>
        public ParticleSystem particals;
        /// <summary>
        /// Max number of stars
        /// </summary>
        public int maxParticals = 150;
        /// <summary>
        /// Seed to create star positions
        /// </summary>
        public int starSeed = 1001;
        /// <summary>
        /// Minimum size a star can be (Less than 0.2f recomended)
        /// </summary>
        public float minStarSize = 0.05f;
        /// <summary>
        /// Maximum size a star can be (less than 0.5f recomended)
        /// </summary>
        public float maxStarSize = 0.5f;
        /// <summary>
        /// Max distance a star can be from the spawn position
        /// </summary>
        public float maxStarDistance = 40f;

        private float farClipPlane;

        /// <summary>
        /// Colour multiplyer to the stars
        /// </summary>
        public float colourMulti = 1.0f;

        Color[] startCols;

        void StarAwake()
        {
            Debug.Log("star awake");
            var main = particals.main;

            main.maxParticles = maxParticals;

            farClipPlane = Camera.main.farClipPlane;
        }

        void StarStart()
        {
            PopulateStarsSeed();
        }

        void PopulateStarsSeed()
        {
            Debug.Log("star seed");
            UnityEngine.Random.InitState(starSeed);

            ParticleSystem.Particle[] points = new ParticleSystem.Particle[maxParticals];
            startCols = new Color[maxParticals];
            particals.GetParticles(points);

            for (int i = 0; i < maxParticals; i++)
            {
                points[i].position = UnityEngine.Random.insideUnitSphere * maxStarDistance;
                points[i].startSize = UnityEngine.Random.Range(minStarSize, maxStarSize);
                startCols[i] = Color.white * UnityEngine.Random.Range(0.5f, 1f);
                points[i].startColor = startCols[i];
                points[i].axisOfRotation = starContainer.transform.position;
                points[i].startLifetime = Mathf.Infinity;
                points[i].remainingLifetime = Mathf.Infinity;
            }

            particals.SetParticles(points, points.Length);
        }

        void SetStarIntensity(float t)
        {
            ParticleSystem.Particle[] points = new ParticleSystem.Particle[maxParticals];
            particals.GetParticles(points);

            for (int i = 0; i < maxParticals; i++)
            {
                points[i].startColor = startCols[i] * starIntensity.Evaluate(t);
            }

            particals.SetParticles(points, points.Length);
        }

    }
}
