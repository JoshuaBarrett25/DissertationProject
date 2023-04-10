using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    public float timeSpeedMultiplier;
    public float startingTime;
    public Color[] ambient;
    public AnimationCurve lightCurve;
    public float sunIntensity;
    public float moonIntensity;

    public Light sun;
    public Light moon;

    private float rise;
    private float set;
    private TimeSpan[] setTime = new TimeSpan[2];

    private DateTime time;
    // Start is called before the first frame update
    void Start()
    {
        time = DateTime.Now.Date + TimeSpan.FromHours(startingTime);
        rise = 7;
        set = 20;
        setTime[0] = TimeSpan.FromHours(rise);
        setTime[1] = TimeSpan.FromHours(set);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime();
        SunPosition();
        LightingUpdate();
    }

    private void SunPosition()
    {
        float position;

        if (time.TimeOfDay > setTime[0] && time.TimeOfDay < setTime[1])
        {
            TimeSpan riseDif = TimeDif(setTime[0], setTime[1]);
            TimeSpan setDif = TimeDif(setTime[0], time.TimeOfDay);

            double perc = setDif.TotalMinutes / riseDif.TotalMinutes;

            position = Mathf.Lerp(0, 180, (float)perc);
        }

        else
        {
            TimeSpan riseDif = TimeDif(setTime[1], setTime[0]);
            TimeSpan setDif = TimeDif(setTime[1], time.TimeOfDay);

            double perc = setDif.TotalMinutes / riseDif.TotalMinutes;
            position = Mathf.Lerp(180, 360, (float)perc);

        }
        sun.transform.rotation = Quaternion.AngleAxis(position, Vector3.right);
    }

    private void LightingUpdate()
    {
        float pos = Vector3.Dot(sun.transform.forward, Vector3.down);
        sun.intensity = Mathf.Lerp(0, sunIntensity, lightCurve.Evaluate(pos));
        moon.intensity = Mathf.Lerp(moonIntensity, 0, lightCurve.Evaluate(pos));
        RenderSettings.ambientLight = Color.Lerp(ambient[1], ambient[0], lightCurve.Evaluate(pos));
    }

    private TimeSpan TimeDif(TimeSpan fromT, TimeSpan toT)
    {
        TimeSpan dif = toT - fromT;


        if (dif.TotalSeconds < 0)
        {
            dif += TimeSpan.FromHours(24);
        }

        return dif;
    }


    private void UpdateTime()
    {
        time = time.AddSeconds(Time.deltaTime * timeSpeedMultiplier);
    }
}
