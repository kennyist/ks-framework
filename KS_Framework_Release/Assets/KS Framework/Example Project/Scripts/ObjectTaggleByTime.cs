using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KS_Core.GameTime;

public class ObjectTaggleByTime : MonoBehaviour {

    public int OnHour = 19;
    public int OffHour = 7;

    public GameObject toggleObject;

	// Use this for initialization
	void Start () {
        KS_TimeManager.Instance.OnTimeUpdate += OnTimeUpdate;
	}

    private void OnDestroy()
    {
        KS_TimeManager.Instance.OnTimeUpdate -= OnTimeUpdate;
    }

    private void OnTimeUpdate(int h, int m, int s, KS_TimeManager.DayTimeZone zone)
    {
        if(h > OnHour || h < OffHour)
        {
            if (!toggleObject.activeSelf)
                toggleObject.SetActive(true);
        }
        else
        {
            if (toggleObject.activeSelf)
                toggleObject.SetActive(false);
        }
    }
}
