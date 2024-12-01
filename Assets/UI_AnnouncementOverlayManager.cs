using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_AnnouncementOverlayManager : MonoBehaviour
{
    public UI_TextCrawler wired_TextCrawler;
    public Transform wired_VisualRootElement;

    public float IV_CloseAfterSec = 5f;

    bool _isAnnouncing = false;
    float _countUpToClose = 0f;

    Coroutine _currentAnnouncement;
    Coroutine _movementCoroutine;

    public Coroutine AwaitCrawlerComplete() {
        return StartCoroutine(AwaitCrawler());
    }

    IEnumerator AwaitCrawler() {
        while (!wired_TextCrawler.QueueIsEmpty) {
            yield return null;
        }
    }

    public Coroutine Announce(string message) {
        // can do logic here and ask for more than just message if we want
        if (!_isAnnouncing) {
            _isAnnouncing = true;
            if (_currentAnnouncement != null) {
                StopCoroutine(_currentAnnouncement);
            }
            _currentAnnouncement = StartCoroutine(RunAnnouncementScrawl(message));
        } else {
            wired_TextCrawler.EnqueueMessage(message);
        }
        return _currentAnnouncement;
    }

    IEnumerator RunAnnouncementScrawl(string startingMessage) {
        if (_movementCoroutine != null) {
            StopCoroutine(_movementCoroutine);
        }
        _movementCoroutine = StartCoroutine(AppearOnScreen());
        wired_TextCrawler._CrawlText.text = "";
        wired_TextCrawler.EnqueueMessage(startingMessage);
    
        while (!wired_TextCrawler.QueueIsEmpty) {
            yield return null;
        }

        _isAnnouncing = false;
        yield return new WaitForSeconds(0.45f);
        if (!_isAnnouncing) {
            if (_movementCoroutine != null) {
                StopCoroutine(_movementCoroutine);
            }
            _movementCoroutine = StartCoroutine(DisappearFromScreen());

        }

    }
    
    IEnumerator AppearOnScreen() {
        while (wired_VisualRootElement.localPosition.x < 180) {
            wired_VisualRootElement.localPosition = new Vector3(
                Mathf.Lerp(wired_VisualRootElement.localPosition.x, 203, 0.1f),
                wired_VisualRootElement.localPosition.y,
                wired_VisualRootElement.localPosition.z
            );
            yield return null;
        }
        yield return null;
    }

    IEnumerator DisappearFromScreen() {
        while (wired_VisualRootElement.localPosition.x > -800f) {
            wired_VisualRootElement.localPosition = new Vector3(
                Mathf.Lerp(wired_VisualRootElement.localPosition.x, -1000f, 0.1f),
                wired_VisualRootElement.localPosition.y,
                wired_VisualRootElement.localPosition.z
            );
            yield return null;
        }
        _isAnnouncing = false;
        yield return null;
    }
}
