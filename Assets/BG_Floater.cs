using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG_Floater : MonoBehaviour
{

    public Vector3 StartingPosition = new Vector3(-7.5f, -5.6f, 6.4f);
    public Vector3 EndingPosition = new Vector3(-1500f, 8f, 6.4f);
    public float threshold = 5f;
    public float IV_Speed = 1f;
    void Start()
    {
        StartCoroutine(FloatAround());
    }

    IEnumerator FloatAround() {
        while (true) {
            transform.localPosition = StartingPosition;
            while (Vector3.Distance(transform.localPosition, EndingPosition) > threshold) {
                transform.localPosition = Vector3.Lerp(transform.localPosition, EndingPosition, Time.deltaTime * IV_Speed);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 6.4f);
                yield return null;
            }
            yield return new WaitForSeconds(1.5f);
        }
    }
}
