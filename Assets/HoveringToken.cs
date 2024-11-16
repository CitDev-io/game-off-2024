using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoveringToken : MonoBehaviour
{

    float FLOATING_ELEVATION_START = .15f;
    public float FLOATING_ELEVATION_SPEED = 2f;
    public float FLOATING_ELEVATION_ARCH = 0.001f;
    float startingElevation;
    void Start()
    {
        startingElevation = transform.localPosition.y;
    }

    void OnEnable() {
        StartCoroutine(StagingFloat());
    }

    void OnDisable() {
        StopAllCoroutines();
        transform.localPosition = new Vector3(
            transform.localPosition.x,
            startingElevation,
            transform.localPosition.z
        );
    }

    IEnumerator StagingFloat() {
        transform.localPosition = new Vector3(
                transform.localPosition.x,
                FLOATING_ELEVATION_START,
                transform.localPosition.z
            );
        while (true) {
            transform.localPosition = new Vector3(
                transform.localPosition.x,
                transform.localPosition.y + (Mathf.Sin(Time.time * FLOATING_ELEVATION_SPEED) * FLOATING_ELEVATION_ARCH),
                transform.localPosition.z
            );
            yield return null;
        }
    }
}
