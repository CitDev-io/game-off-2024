using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public delegate void MessageCompletedDelegate();

public class UI_TextCrawler : MonoBehaviour
{
    public TextMeshProUGUI _CrawlText;
    Queue<string> MessageQueue = new Queue<string>();
    public float CRAWL_SPEED = 0.05f;
    public float DISPLAY_DURATION = 1.25f;
    public float CHECK_QUEUE_TIMEOUT = 0.25f;
    string SpellItOut = "";
    Coroutine _runningCoroutine;
    public MessageCompletedDelegate OnMessageComplete;
    public bool QueueIsEmpty = true;

    public bool skipIt = false;

    void Start() {
        StartCoroutine(Crawl());
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            skipIt = true;
        }
    }

    public void ClearQueue() {
        MessageQueue.Clear();
        _CrawlText.text = "";
    }

    IEnumerator Crawl() {
        while (true) {
            if (MessageQueue.Count > 0) {
                string targetMessage = MessageQueue.Dequeue();
                SpellItOut = "";
                if (targetMessage.Length > 0) {
                    skipIt = false;
                    int index = 0;
                    foreach(char letter in targetMessage) {
                        SpellItOut += letter;
                        _CrawlText.text = SpellItOut;
                        if (index % 3 == 0) {
                            if (!skipIt) {
                                yield return new WaitForSeconds(CRAWL_SPEED);
                            }
                        }
                        index++;
                    }
                }
                if (skipIt) {
                    skipIt = false;
                }
                _CrawlText.text = targetMessage;
                OnMessageComplete?.Invoke();
                float timer = 0f;
                while (timer < DISPLAY_DURATION) {
                    timer += Time.deltaTime;
                    if (!skipIt) {
                        yield return null;
                    } else {
                        timer = DISPLAY_DURATION;
                        skipIt = false;
                    }
                }
            } else {
                QueueIsEmpty = true;
            }
            yield return new WaitForSeconds(CHECK_QUEUE_TIMEOUT);
        }
    }
    public void EnqueueMessage(string message) {
        if (MessageQueue.Contains(message)) {
            return;
        }
        QueueIsEmpty = false;
        MessageQueue.Enqueue(message);
    }
}
