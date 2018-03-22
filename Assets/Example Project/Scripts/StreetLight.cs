using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetLight : MonoBehaviour {

    public int OnHour = 19;
    public int OffHour = 7;

    Light light;

	// Use this for initialization
	void Start () {
        KS_TimeManager.Instance.OnTimeUpdate += OnTimeUpdate;
        light = GetComponentInChildren<Light>();
	}

    private void OnDestroy()
    {
        KS_TimeManager.Instance.OnTimeUpdate -= OnTimeUpdate;
    }

    private void OnTimeUpdate(int h, int m, int s, KS_TimeManager.DayTimeZone zone)
    {
        if(h > OnHour || h < OffHour)
        {
            if (!light.enabled)
                light.enabled = true;
        }
        else
        {
            if (light.enabled)
                light.enabled = false;
        }
    }
}
