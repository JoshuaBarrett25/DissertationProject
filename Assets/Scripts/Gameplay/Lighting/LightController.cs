using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public int dayCounter = 0;
    private bool counted = false;
    [SerializeField] private Light directional;
    [SerializeField] private Preset preset;
    [SerializeField, Range(0, 24)] private float time;
    [SerializeField] private float timeSpeedMultiplier = 2f;

    private void Update()
    {
        time += Time.deltaTime * timeSpeedMultiplier;
        time %= 24;
        UpdateLightingInScene(time/24f);
    }

    private void UpdateLightingInScene(float perc)
    {
        if (perc >= 0.9 && !counted)
        {
            dayCounter += 1;
            counted = true;
        }
        if (perc <= 0.1 && counted)
        {
            counted = false;
        }

        RenderSettings.ambientLight = preset.ambient.Evaluate(perc);
        RenderSettings.fogColor = preset.fog.Evaluate(perc);

        directional.color = preset.directional.Evaluate(perc);
        directional.transform.localRotation = Quaternion.Euler(new Vector3((perc * 360f) -90f, 170f, 0));
    }
}
