using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSoftPulse : MonoBehaviour
{
    Light GridLight;
    
    void Start()
    {
        GridLight = GetComponent<Light>();
        StartCoroutine(SoftPulse());  
    }
public float RANGE_X = 4.5f;
public float RANGE_Y = 2f;
public float RANGE_Z = 0.5f;
public float INTENSITY_X = 3.5f;
public float INTENSITY_Y = 0.7f;

    IEnumerator SoftPulse()
    {
        GridLight = GetComponent<Light>();
        while (true)
        {
            // Grid Light Range should move between 4.5f and 6.5f
            // Grid Light Intensity should move between 1.5f and 2.2f
            GridLight.range = RANGE_X + RANGE_Y * Mathf.Sin(Time.time - RANGE_Z);
            GridLight.intensity = INTENSITY_X + INTENSITY_Y * Mathf.Sin(Time.time);
            yield return null;
        }
    }
}
