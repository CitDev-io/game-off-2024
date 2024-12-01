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
        wired_TextCrawler.OnMessageComplete += RenewTimer;
    }

    void RenewTimer() {
        _countUpToClose = 0f;
    }

    Coroutine _currentAnnouncement;

    public Coroutine Announce(string message, int extraDelay = 0) {
        // can do logic here and ask for more than just message if we want
        if (!_isAnnouncing) {
            FindAnyObjectByType<GameController_DDOL>().PlaySound("Place_Terraformer");
            _isAnnouncing = true;
            _countUpToClose -= extraDelay;
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
        if (_countUpToClose > 0) {
            _countUpToClose = 0f;
        }
        while (_countUpToClose < IV_CloseAfterSec) {
            _countUpToClose += Time.deltaTime;
            yield return null;
        }
        yield return StartCoroutine(DisappearFromScreen());
    }
    
    IEnumerator AppearOnScreen() {
        FindAnyObjectByType<GameController_DDOL>().PlaySound("Incoming_Transmission");
        wired_VisualRootElement.gameObject.SetActive(true);
        yield return null;
    }

    IEnumerator DisappearFromScreen() {
        wired_VisualRootElement.gameObject.SetActive(false);
        _isAnnouncing = false;
        yield return null;
    }
}
