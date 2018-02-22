using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KS_DayNight : MonoBehaviour {

    // Time management

    KS_TimeManager timeManager;

    // Sun Manager

    public GameObject SunContanier;
    public GameObject sun;

    // Use this for initialization
    void Start () {
        timeManager = KS_TimeManager.Instance;

        timeManager.OnTimeUpdate += OnTimeUpdate;
	}
	
	// Update is called once per frame
	void OnTimeUpdate (int h, int m, int s) {

    }

    private void FixedUpdate()
    {
        UpdateSunPos();
        UpdateEnvironment();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            CalculateSunPosition(12, 30, 0, 50.376316, -4.123820);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            CalculateSunPosition(23, 30, 0, 50.376316, -4.123820);
        }
    }

    private void UpdateSunPos()
    {

    }

    private void UpdateEnvironment()
    {

    }

    private void CalculateSunPosition(
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

        // Number of days from J2000.0.  
        double julianDate = ConvertToJulian(dateTime);
        Debug.Log("JD: " + julianDate);

        // Get julian Centuries
        double julianCenturies = julianDate / 36525.0;
        Debug.Log("JD: " + julianCenturies);

        double n = julianDate - 2451545.0; ;
        Debug.Log("N: " + n);
        double L = (280.560 + 0.9856474 * n) % 360;
        Debug.Log("l: " + L);
        double g = (357.528 + 0.9856003 * n) % 360;
        Debug.Log("g: " + g);
        double lambda = (L + 1.915 * DegreesSin(g) + 0.020 * DegreesSin(2 * g)) % 360;
        Debug.Log("lambda: " + lambda);
        double R = 1.00014 - 0.01617 * DegreesSin(g) - 0.00014 * DegreesSin(2 * g);
        Debug.Log("R: " + R);

        double epsilon = 23.439 - 0.0000004 * n;
        Debug.Log("epsilon: " + epsilon);
        double alpha = Math.Atan2(
                            DegreesCos(epsilon) *
                            DegreesSin(lambda),
                            DegreesCos(lambda));
        Debug.Log("Alpha: " + alpha);
        double delta = Math.Asin(DegreesSin(epsilon) *
                                 DegreesSin(lambda));
        Debug.Log("Declination: " + delta);

        // Side reel time
        double time = (double) dateTime.Hour + ((double)dateTime.Minute / 60);
        double date = (julianDate - 2451545.0) / 36525.0;
        Debug.Log(time);
        double LST = 100.46 + 0.985647 * date + longitude + 15 * time;

        Debug.Log("LST: " + LST);

        // Horizontal Coordinates  
        double hourAngle = LST - alpha;

        if (hourAngle < 0)
        {
            hourAngle += 360;
        }

        if(hourAngle > 360)
        {
            hourAngle -= 360;
        }

        Debug.Log("Hour angle: " + hourAngle);

        double lat = Mathf.Abs((float)delta) * 360;
        double longi = LST;

        Debug.Log("lat: " + lat + " Long: " + longi);


        Vector2 position = new Vector2(
            Convert.ToSingle(lat),
            Convert.ToSingle(longi)
            );


    }

    private double DegreesSin(double value)
    {
        return Math.Sin(RadiansToDegrees(value));
    }

    private double RadiansToDegrees(double value)
    {
        return value * (180 / Math.PI);
    }

    private double DegreesToRadians(double value)
    {
        return value / (180 * Math.PI);
    }

    private double DegreesCos(double value)
    {
        return Math.Cos(RadiansToDegrees(value));
    }

    private double ConvertToJulian(DateTime Date)
    {
        int Month = Date.Month;
        int Day = Date.Day;
        int Year = Date.Year;

        if (Month < 3)
        {
            Month = Month + 12;
            Year = Year - 1;
        }
        long JulianDay = Day + (153 * Month - 457) / 5 + 365 * Year + (Year / 4) - (Year / 100) + (Year / 400) + 1721119;
        return JulianDay;
    }
}
