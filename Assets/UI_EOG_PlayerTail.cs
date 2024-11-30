using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_EOG_PlayerTail : MonoBehaviour
{
    public RectTransform _wiredTailImageTransform;

    public void UnfurlTail() {
        StartCoroutine(UnfurlTailRoutine());
    }

    public void RefurlTail() {
        StartCoroutine(RefurlTailRoutine());
    }

    IEnumerator UnfurlTailRoutine() {
        float duration = 1f;
        float t = 0f;
        Vector2 start = new Vector2(125f, _wiredTailImageTransform.sizeDelta.y);
        Vector2 end = new Vector2(1125f, _wiredTailImageTransform.sizeDelta.y);
        while (t < duration) {
            t += Time.deltaTime;
            _wiredTailImageTransform.sizeDelta = Vector2.Lerp(start, end, t / duration);
            yield return null;
        }
    }

    IEnumerator RefurlTailRoutine() {
        float duration = 1f;
        float t = 0f;
        Vector2 start = new Vector2(1125f, _wiredTailImageTransform.sizeDelta.y);
        Vector2 end = new Vector2(125f, _wiredTailImageTransform.sizeDelta.y);
        while (t < duration) {
            t += Time.deltaTime;
            _wiredTailImageTransform.sizeDelta = Vector2.Lerp(start, end, t / duration);
            yield return null;
        }
    }
}
