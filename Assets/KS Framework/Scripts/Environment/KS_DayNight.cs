using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KS_Core.IO;
using KS_Core.GameTime;

public class KS_DayNight : MonoBehaviour {

    private KS_DayNight instance;
    public KS_DayNight Instance
    {
        get
        {
            if(instance != null)
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
    public Gradient nightDayColour = new Gradient();

    public float maxIntensity = 3f;
    public float minIntensity = 0f;
    public float minIntensityPoint = -0.2f;

    public float maxAmbient = 1f;
    public float minAmbient = 0f;
    public float minAmbientPoint = -0.2f;

    public Gradient nightDayFogColour = new Gradient();
    public AnimationCurve fogDensity = new AnimationCurve();
    public float fogScale = 1f;

    public float dayAtmospherThickness = 0.04f;
    public float nightAtmosphereThickness = 0.87f;

    private float skySpeed;
    private Light mainLight;
    private Skybox sky;
    private Material skyMat;

    // World data
    public double latitude = 50.376316;
    public double longitude = -4.123820;
    public int timeZone = 0;

    // Sun Manager

    public GameObject SunContanier;
    public GameObject sun;

    // Star system
    public Transform starContainer;
    public AnimationCurve starIntensity;
    

    private void Awake()
    {
        if (instance != null) Destroy(this);
        instance = this;
        StarAwake();

        KS_SaveLoad.OnSave += OnSave;
        KS_SaveLoad.OnLoad += OnLoad;
    }

    private void OnDestroy()
    {
        KS_SaveLoad.OnSave -= OnSave;
        KS_SaveLoad.OnLoad -= OnLoad;
    }

    // Use this for initialization
    void Start () {
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
	
	// Update is called once per frame
	void OnTimeUpdate (int h, int m, int s, KS_TimeManager.DayTimeZone zone) {
        UpdateSunPos(h, m, s);
        UpdateEnvironment();
    }

    private void UpdateSunPos(int h, int m, int s)
    {
        Vector2 sunPos = CalculateSunPosition(h, m, s, latitude, longitude);

        //Debug.Log(sunPos.ToString());

        Vector3 sRot = new Vector3(sunPos.x, 0, 0);
        Vector3 cRot = new Vector3(0, sunPos.y, 0);

        sun.transform.localRotation = Quaternion.Euler(sRot);
        SunContanier.transform.localRotation = Quaternion.Euler(cRot);
        starContainer.transform.rotation = Quaternion.Euler(new Vector3((float)latitude, 0, 360 * timeManager.TimeOfDay));
    }

    private void UpdateEnvironment()
    {
        float tRange = 1 - minIntensityPoint;
        float dot = Mathf.Clamp01((Vector3.Dot(mainLight.transform.forward, Vector3.down) - minIntensityPoint) / tRange);
        float i = ((maxIntensity - minIntensity) * dot) + minIntensity;

        mainLight.intensity = i;
        //SetStarIntensity(i);

        tRange = 1 - minAmbientPoint;
        dot = Mathf.Clamp01((Vector3.Dot(mainLight.transform.forward, Vector3.down) - minAmbientPoint) / tRange);
        i = ((maxAmbient - minAmbient) * dot) + minAmbient;
        RenderSettings.ambientIntensity = i;

        mainLight.color = nightDayColour.Evaluate(dot);
        RenderSettings.ambientLight = mainLight.color;

        RenderSettings.fogColor = nightDayFogColour.Evaluate(dot);
        RenderSettings.fogDensity = fogDensity.Evaluate(dot) * fogScale;

        i = ((dayAtmospherThickness - nightAtmosphereThickness) * dot) + nightAtmosphereThickness;
        skyMat.SetFloat("_AtmosphereThickness", i);

        SetStarIntensity(timeManager.TimeOfDay);
    }

    private const double Deg2Rad = Math.PI / 180.0;
    private const double Rad2Deg = 180.0 / Math.PI;

    private Vector2 CalculateSunPosition(
        int hour,
        int min,
        int sec,
        double latitude,
        double longitude)
    {
        DateTime dateTime = new DateTime(DateTime.Now.Year,
                                         DateTime.Now.Month,
                                         DateTime.Now.Day,
                                         hour,
                                         min,
                                         sec);

        // Get Julian Date and Centuries
        double JD = ToJulianDate(dateTime);
        double JC = ToJulianCenturies(JD);

        //Debug.Log("Julian Date: " + JD + " - Julian Centuries: " + JC);

        // SideReal time
        double sideRealTime = GetLST(dateTime, JD, JC);

        //Debug.Log("Sidereal Time: " + sideRealTime);

        // Refine Time to add current percentage of day
        JD += (double)dateTime.TimeOfDay.TotalHours / 24.0;
        JC = JD / 36525.0;

        //Debug.Log("Refined Julian Date: " + JD + " - refined Julian Centuries: " + JC);

        // Solar Coordinates 
        double meanLongitude = GetMeanLongitude(JC);
        double meanAnomaly = GetMeanAnomaly(JC);
        double equationOfCenter = GetEquationOfCenter(JC, meanAnomaly);
        double elipticalLongitude = GetElipticalLongitude(meanLongitude, equationOfCenter);
        double obliquity = GetObliquity(JC);
        double rightAscension = GetRightAscension(obliquity, elipticalLongitude);
        double declination = GetDeclination(rightAscension, obliquity);

        // Terestial Coordinates

        double hourAngle = GetHorizontalCoordinates(sideRealTime, rightAscension);
        double altitude = GetAltitude(declination, hourAngle);
        double azimuth = GetAzimuth(hourAngle, declination);
        

        return new Vector2(Convert.ToSingle(altitude * Rad2Deg), 
                           Convert.ToSingle(azimuth * Rad2Deg));

    }

    private double GetMeanLongitude(double julianCenturies)
    {
        return CorrectAngle(Deg2Rad * (280.466 + 36000.77 * julianCenturies));
    }

    private double GetMeanAnomaly(double julianCenturies)
    {
        return CorrectAngle(Deg2Rad * (357.529 + 35999.05 * julianCenturies));
    }

    private double GetEquationOfCenter(double julianCenturies, double meanAnomaly)
    {
        return Deg2Rad * ((1.915 - 0.005 * julianCenturies) *
               Math.Sin(meanAnomaly) + 0.02 * Math.Sin(2 * meanAnomaly));
    }

    private double GetElipticalLongitude(double meanLongitude, double equationOfCenter)
    {
        return CorrectAngle(meanLongitude + equationOfCenter);
    }

    private double GetObliquity(double julianCenturies)
    {
        return (23.439 - 0.013 * julianCenturies) * Deg2Rad;
    }

    private double GetRightAscension(double obliquity, double elipticalLongitude)
    {
        return Math.Atan2(
            Math.Cos(obliquity) * Math.Sin(elipticalLongitude),
            Math.Cos(elipticalLongitude)
            );
    }

    private double GetDeclination(double rightAscension, double obliquity)
    {
        return Math.Sin(rightAscension) * Math.Sin(obliquity);
    }

    private double GetHorizontalCoordinates(double sideRealTime, double rightAscension)
    {
        double hAngle = CorrectAngle(sideRealTime * Deg2Rad) - rightAscension;

        if(hAngle > Math.PI)
        {
            hAngle -= 2 * Math.PI;
        }

        return hAngle;
    }

    private double GetAltitude(double declination, double hourAngle)
    {
        return Math.Asin(
            Math.Sin(latitude * Deg2Rad) *
            Math.Sin(declination) + Math.Cos(latitude * Deg2Rad) *
            Math.Cos(declination) * Math.Cos(hourAngle));
    }

    private double GetAzimuth(double hourAngle, double declination)
    {
        double aN = -Math.Sin(hourAngle);
        double aD = Math.Tan(declination) * Math.Cos(latitude * Deg2Rad) -
                    Math.Sin(latitude * Deg2Rad) * Math.Cos(hourAngle);

        double azimuth = Math.Atan(aN / aD);

        if(aD < 0)
        {
            azimuth += Math.PI;
        }
        else if(aN < 0)
        {
            azimuth += 2 * Math.PI;
        }

        return azimuth;
    }

    private double ToJulianDate(DateTime date)
    {
        // Convert to UTC  
        date = date.ToUniversalTime();

        return 367 * date.Year -
            (int)((7.0 / 4.0) * (date.Year +
            (int)((date.Month + 9.0) / 12.0))) +
            (int)((275.0 * date.Month) / 9.0) +
            date.Day - 730531.5;
    }

    private double ToJulianCenturies(double julianDate)
    {
        return julianDate / 36525.0;
    }

    private double GetLST(DateTime date, double JulianDay, double JulianCentuires)
    {
        double siderealTimeHours = 6.6974 + 2400.0513 * JulianCentuires;

        double siderealTimeUT = siderealTimeHours +
            (366.2422 / 365.2422) * (double)date.TimeOfDay.TotalHours;

        return siderealTimeUT * 15 + longitude;
    }

    private double CorrectAngle(double angleInRadians)
    {
        if (angleInRadians < 0)
        {
            return 2 * Math.PI - (Math.Abs(angleInRadians) % (2 * Math.PI));
        }
        else if (angleInRadians > 2 * Math.PI)
        {
            return angleInRadians % (2 * Math.PI);
        }
        else
        {
            return angleInRadians;
        }
    }

    // Star System

    public ParticleSystem particals;
    public int maxParticals = 150;

    public bool useFile = false;
    public TextAsset starFile;

    public int starSeed = 1001;
    public float minStarSize = 0.05f;
    public float maxStarSize = 0.5f;
    public float maxStarDistance = 40f;

    private float farClipPlane;

    public float colourMulti = 1.0f;

    void StarAwake()
    {
        Debug.Log("star awake");
        var main = particals.main;

        main.maxParticles = maxParticals;

        farClipPlane = Camera.main.farClipPlane;
    }

    void StarStart()
    {
        Debug.Log("star start");
        if (useFile)
        {
            PopulateStartsFile();
        }
        else
        {
            PopulateStarsSeed();
        }
    }

    void PopulateStartsFile()
    {
        Debug.Log("star file");

        ParticleSystem.Particle[] points = new ParticleSystem.Particle[maxParticals];
        startCols = new Color[maxParticals];
        particals.GetParticles(points);

        string[] lines = starFile.text.Split(';');

        for(int i = 0; i < lines.Length; i++)
        {
            string[] lineSplit = lines[i].Split(',');
            Vector3 position = new Vector3();
            position.x = float.Parse(lineSplit[0]);
            position.y = float.Parse(lineSplit[1]);
            position.z = float.Parse(lineSplit[2]);

            float size = float.Parse(lineSplit[3]);
            float brightness = float.Parse(lineSplit[4]);

            points[i].position = position;
            points[i].startSize = size;
            startCols[i] = Color.white * brightness;
            points[i].startColor = startCols[i];
            points[i].axisOfRotation = starContainer.transform.position;
            points[i].startLifetime = Mathf.Infinity;
            points[i].remainingLifetime = Mathf.Infinity;
        }
    }

    Color[] startCols;

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
