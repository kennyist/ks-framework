using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KS_TimeManager : MonoBehaviour {

    private static KS_TimeManager instance;
    public static KS_TimeManager Instance
    {
        get
        {
            if(instance != null)
            {
                return instance;
            }
            else
            {
                instance = new KS_TimeManager();
                return instance;
            }
        }
    }

    public delegate void TimeUpdate(int h, int m, int s);
    public event TimeUpdate OnTimeUpdate;

    public enum TimeZone
    {
        Dawn,
        Morning,
        Midday,
        afternoon,
        Dusk,
        Midnight
    }

    public int dawnStartTime = 6 * 60;
    public int morningStartTime = 8 * 60;
    public int middayStartTime = 11 * 60;
    public int afternoonStartTime = 13 * 60;
    public int duskStartTime = 18 * 60;
    public int midnightStartTime = 23 * 60;

    private TimeZone currentZone = TimeZone.afternoon;

    // ---

    public bool realTime = false;


    public float secondsPerMinute = 3f;


    // - Private
    private int minutesPerDay = 1439;
    float currentMinute = 0;
    private int currentSecond = 0;
    float secondTimer = 0;

    // ---

    private void Start()
    {
        instance = this;
    }

    private void FixedUpdate()
    {
        if (!realTime)
        {
            UpTime();
        }
        else
        {

        }
    }

    private void UpTime()
    {
        secondTimer += Time.deltaTime;

        if(secondTimer >= secondsPerMinute)
        {
            secondTimer = 0f;

            currentMinute++;

            if(currentMinute > minutesPerDay)
            {
                currentMinute = 0;
            }

            currentSecond = (int)((currentSecond / secondsPerMinute) * 59);
            if (currentSecond > 59) currentSecond = 59;

            CheckZone();

            Debug.Log(GetTimeFormatted());

            if (OnTimeUpdate != null) {
                int[] time = GetTime();
                OnTimeUpdate(time[0], time[1], time[2]);
            }
        }
    }

    private void CheckZone()
    {
        if (currentMinute >= dawnStartTime && currentMinute < morningStartTime)
        {
            currentZone = TimeZone.Dawn;
        }
        else if (currentMinute >= morningStartTime && currentMinute <= middayStartTime)
        {
            currentZone = TimeZone.Morning;
        }
        else if (currentMinute >= middayStartTime && currentMinute <= afternoonStartTime)
        {
            currentZone = TimeZone.Midday;
        }
        else if (currentMinute >= afternoonStartTime && currentMinute <= duskStartTime)
        {
            currentZone = TimeZone.afternoon;
        }
        else if (currentMinute >= duskStartTime && currentMinute <= midnightStartTime)
        {
            currentZone = TimeZone.Dusk;
        }
        else
        {
            currentZone = TimeZone.Midnight;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int[] GetTime()
    {
        int h = (int)currentMinute / 60;
        int m = (int)currentMinute % 60;

        return new int[] { h, m, Mathf.RoundToInt(currentSecond) };
    }

    public string GetTimeFormatted()
    {
        int[] t = GetTime();

        string H = "";
        string M = "";

        if (t[0] < 10)
        {
            H = "0";
        }

        if (t[1] < 10)
        {
            M = "0";
        }

        H += t[0].ToString();
        M += t[1].ToString();

        return H + ":" + M + ":" + t[2].ToString() + " - " + currentZone.ToString();
    }

    public float TimeOfDay // game time 0 .. 1
    {
        get
        {
            float maxM = minutesPerDay * 60;
            float m = (currentMinute * 60) + (currentSecond / 60);
            return m / maxM;
        }
    }

    public void SetTime(int hours, int minuets)
    {
        if (hours > 23) hours = 23;
        if (minuets > 59) minuets = 59;

        currentMinute = (hours * 60) + minuets;
    }
}
