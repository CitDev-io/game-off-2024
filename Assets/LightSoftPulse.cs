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
// public float INTENSITY_X = 3.5f;
// public float INTENSITY_Y = 0.7f;

    IEnumerator SoftPulse()
    {
        GridLight = GetComponent<Light>();
        while (true)
        {
            // GridLight.intensity = INTENSITY_X + INTENSITY_Y * Mathf.Sin(Time.time);
            yield return new WaitForSeconds(0.02f);
            GridLight.range = RANGE_X + RANGE_Y * Mathf.Sin(Time.time - RANGE_Z);
            // yield return new WaitForSeconds(0.01f);
        }
    }
}
