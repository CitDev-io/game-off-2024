using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CaptainOverlayManager : MonoBehaviour
{
    public UI_TextCrawler wired_TextCrawler;
    public Transform wired_VisualRootElement;

    public float IV_CloseAfterSec = 5f;

    bool _isAnnouncing = false;
    float _countUpToClose = 0f;
    void Awake() {
        wired_TextCrawler.OnMessageCompleted += RenewTimer;
    }

    void RenewTimer() {
        _countUpToClose = 0f;
    }

    Coroutine _currentAnnouncement;

    public Coroutine Announce(string message) {
        // can do logic here and ask for more than just message if we want
        if (!_isAnnouncing) {
            _isAnnouncing = true;
            _currentAnnouncement = StartCoroutine(RunAnnouncementScrawl(message));
        } else {
            wired_TextCrawler.EnqueueMessage(message);
        }
        return _currentAnnouncement;
    }


    IEnumerator RunAnnouncementScrawl(string startingMessage) {
        Debug.Log("STARTING CAPTAIN STUFF");
        yield return StartCoroutine(AppearOnScreen());
        wired_TextCrawler.EnqueueMessage(startingMessage);
        wired_TextCrawler._CrawlText.text = "";
        _countUpToClose = 0f;
        while (_countUpToClose < IV_CloseAfterSec) {
            _countUpToClose += Time.deltaTime;
            yield return null;
        }
        yield return StartCoroutine(DisappearFromScreen());
    }
    
    IEnumerator AppearOnScreen() {
        wired_VisualRootElement.gameObject.SetActive(true);
        yield return null;
    }

    IEnumerator DisappearFromScreen() {
        wired_VisualRootElement.gameObject.SetActive(false);
        _isAnnouncing = false;
        yield return null;
    }
}
