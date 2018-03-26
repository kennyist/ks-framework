using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using KS_Core.GameTime;

public class KS_UI_Time : MonoBehaviour {

    public Text timeBox;
    public Text zoneBox;

	// Use this for initialization
	void Start () {
        KS_TimeManager.Instance.OnTimeUpdate += OnTimeUpdate;
	}
	
    void OnTimeUpdate(int h, int m, int s, KS_TimeManager.DayTimeZone zone)
    {
        timeBox.text = h.ToString("00") + ":" + m.ToString("00");
        zoneBox.text = zone.ToString().ToUpper();
    }
}
