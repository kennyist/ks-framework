using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KS_Core;
using KS_Core.Localisation;
using KS_Core.GameTime;
using KS_Core.Atmosphere;
using KS_Core.Input;
using UnityEngine.UI;
using System;

public class Tut_Zone_TimeDayNight : MonoBehaviour {

    double latitude, longitude;
    float scale;

    KS_TimeManager timeManager;
    KS_DayNight dayNight;

    public Text timeText;
    public Text zoneText;
    public Text scaletext;
    public Text latLongText;

	// Use this for initialization
	void OnEnable () {
        timeManager = KS_TimeManager.Instance;
        dayNight = KS_DayNight.Instance;

        latitude = dayNight.latitude;
        longitude = dayNight.longitude;
        scale = timeManager.secondsPerMinute;

        KS_TimeManager.Instance.OnTimeUpdate += OnTimeUpdate;
	}

    private void OnDisable()
    {
        timeManager.OnTimeUpdate -= OnTimeUpdate;
        timeManager.secondsPerMinute = scale;
        dayNight.latitude = latitude;
        dayNight.longitude = longitude;
        timeManager.SetTime(11, 0);
    }

    bool dpadL = false;
    bool dpadR = false;
    bool dpadU = false;
    bool dpadD = false;

    // Update is called once per frame
    void Update () {
        if(KS_Input.GetDS4Axis(DS4Axis.DPadX) < 0 || Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (!dpadL)
            {
                dpadL = true;
                ResetTimeScale();
            }
        } else { dpadL = false; }

        if (KS_Input.GetDS4Axis(DS4Axis.DPadX) > 0 || Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (!dpadR)
            {
                dpadR = true;
                IncreaseTimeScale();
            }
        }
        else { dpadR = false; }

        if (KS_Input.GetDS4Axis(DS4Axis.DPadY) < 0 || Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (!dpadD)
            {
                dpadD = true;
                AddHour();
            }
        }
        else { dpadD = false; }

        if (KS_Input.GetDS4Axis(DS4Axis.DPadY) > 0 || Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (!dpadU)
            {
                dpadU = true;
                QuickCycle();
            }
        }
        else { dpadU = false; }

        if (Input.GetKeyDown(KeyCode.Alpha5) || KS_Input.GetKeyDown(KS_Input.DS4ButtonToKey(DS4KeyCode.X)))
        {
            ResetLongLat();
        }

        if (Input.GetKeyDown(KeyCode.Alpha6) || KS_Input.GetKeyDown(KS_Input.DS4ButtonToKey(DS4KeyCode.Square)))
        {
            SetLongLatFinland();
        }

        if (Input.GetKeyDown(KeyCode.Alpha7) || KS_Input.GetKeyDown(KS_Input.DS4ButtonToKey(DS4KeyCode.Triangle)))
        {
            SetLongLatEquator();
        }
    }

    private void OnTimeUpdate(int h, int m, int s, KS_TimeManager.DayTimeZone zone)
    {
        timeText.text = "Current time - " + timeManager.GetTimeFormatted();
        zoneText.text = "Current zone - " + zone.ToString();
        latLongText.text = "Lat/long - " + dayNight.latitude + " : " + dayNight.longitude;
        scaletext.text = "Scale - " + timeManager.secondsPerMinute.ToString("0.000") + "s per minute";
    }

    void ResetTimeScale()
    {
        timeManager.secondsPerMinute = scale;

        KS_Subtitle.Instance.ShowText("Time Scale: 1x", 2f);
    }

    void IncreaseTimeScale()
    {
        timeManager.secondsPerMinute = timeManager.secondsPerMinute / 4;
    }

    void QuickCycle()
    {
        timeManager.SetTime(11, 0);
        timeManager.SetTimeOverTime(10, 58, 12);

        KS_Subtitle.Instance.ShowText("Quick 24 hour cycle starting", 2f);
    }

    void AddHour()
    {
        int[] time = timeManager.GetTime();

        if (time[0] >= 23) time[0] = 0;
        else time[0]++;

        timeManager.SetTime(time[0], time[1]);
    }

    void ResetLongLat()
    {
        dayNight.latitude = latitude;
        dayNight.longitude = longitude;
        dayNight.timeZone = 0;
    }

    void SetLongLatFinland()
    {
        dayNight.latitude = -80.113037;
        dayNight.longitude = 25.825314;
        dayNight.timeZone = -4;
    }

    void SetLongLatEquator()
    {
        dayNight.latitude = 0.01;
        dayNight.longitude = 0.01;
    }
}
