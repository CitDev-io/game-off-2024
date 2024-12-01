using System.Collections;
using UnityEngine;
// using UnityEditor;

// [CustomEditor(typeof(UI_EOGOverlayManager))]
// public class UI_EOGOVerlayManagerEditor : Editor
// {
//     public override void OnInspectorGUI()
//     {
//         DrawDefaultInspector();

//         UI_EOGOverlayManager myComponent = (UI_EOGOverlayManager)target;
//         if (GUILayout.Button("Unfurl Tail"))
//         {
//             myComponent.wired_Player1Tail.UnfurlTail();
//             myComponent.wired_Player2Tail.UnfurlTail();
//         }
//         if (GUILayout.Button("Refurl Tail"))
//         {
//             myComponent.wired_Player1Tail.RefurlTail();
//             myComponent.wired_Player2Tail.RefurlTail();
//         }

//         if (GUILayout.Button("Present"))
//         {
//             myComponent.Present();
//         }

//         if (GUILayout.Button("Close"))
//         {
//             myComponent.CloseOverlay();
//         }
//     }
// }

public class UI_EOGOverlayManager : MonoBehaviour
{
    public CanvasGroup OverlayCanvasGroup;
    public UI_EOG_PlayerTail wired_Player1Tail;
    public UI_EOG_PlayerTail wired_Player2Tail;
    public Transform wired_PlayersBlock;
    public CanvasGroup wired_StatsBlock;


    public TMPro.TextMeshProUGUI wired_P1_Rank;
    public TMPro.TextMeshProUGUI wired_P1_OBJ;
    public TMPro.TextMeshProUGUI wired_P1_OBE;
    public TMPro.TextMeshProUGUI wired_P1_ROA;
    public TMPro.TextMeshProUGUI wired_P1_CIT;
    public TMPro.TextMeshProUGUI wired_P1_PTS;

    public TMPro.TextMeshProUGUI wired_P2_Rank;
    public TMPro.TextMeshProUGUI wired_P2_OBJ;
    public TMPro.TextMeshProUGUI wired_P2_OBE;
    public TMPro.TextMeshProUGUI wired_P2_ROA;
    public TMPro.TextMeshProUGUI wired_P2_CIT;
    public TMPro.TextMeshProUGUI wired_P2_PTS;
    public GameObject wired_VictoryBar;
    public GameObject wired_DefeatBar;



    Coroutine _toggleCoroutine;
    

    public void Present() {
        Scoreboard s = FindObjectOfType<GameBoardManager>().GridGameInstance.scoreboard;

        if (s.Stats[PlayerSlot.PLAYER1].Score > s.Stats[PlayerSlot.PLAYER2].Score) {
            wired_VictoryBar.SetActive(true);
            wired_DefeatBar.SetActive(false);
        } else {
            wired_VictoryBar.SetActive(false);
            wired_DefeatBar.SetActive(true);
        }

        wired_P1_Rank.text = s.Stats[PlayerSlot.PLAYER1].Rank.ToString();
        wired_P1_OBJ.text = s.Stats[PlayerSlot.PLAYER1].ScoreByEventType[ScoringEventType.SECRET_OBJECTIVE].ToString();
        wired_P1_OBE.text = s.Stats[PlayerSlot.PLAYER1].ScoreByEventType[ScoringEventType.OBELISKCOMPLETED].ToString();
        wired_P1_ROA.text = s.Stats[PlayerSlot.PLAYER1].ScoreByEventType[ScoringEventType.ROADCOMPLETED].ToString();
        wired_P1_CIT.text = s.Stats[PlayerSlot.PLAYER1].ScoreByEventType[ScoringEventType.CITYCOMPLETED].ToString();
        wired_P1_PTS.text = s.Stats[PlayerSlot.PLAYER1].Score.ToString();

        wired_P2_Rank.text = s.Stats[PlayerSlot.PLAYER2].Rank.ToString();
        wired_P2_OBJ.text = s.Stats[PlayerSlot.PLAYER2].ScoreByEventType[ScoringEventType.SECRET_OBJECTIVE].ToString();
        wired_P2_OBE.text = s.Stats[PlayerSlot.PLAYER2].ScoreByEventType[ScoringEventType.OBELISKCOMPLETED].ToString();
        wired_P2_ROA.text = s.Stats[PlayerSlot.PLAYER2].ScoreByEventType[ScoringEventType.ROADCOMPLETED].ToString();
        wired_P2_CIT.text = s.Stats[PlayerSlot.PLAYER2].ScoreByEventType[ScoringEventType.CITYCOMPLETED].ToString();
        wired_P2_PTS.text = s.Stats[PlayerSlot.PLAYER2].Score.ToString();

        if (_toggleCoroutine != null) {
            StopCoroutine(_toggleCoroutine);
        }
        _toggleCoroutine = StartCoroutine(AppearOnScreen());
    }

    public void CloseOverlay() {
        if (_toggleCoroutine != null) {
            StopCoroutine(_toggleCoroutine);
        }
        _toggleCoroutine = StartCoroutine(DisappearFromScreen());
    }

    IEnumerator FadeIn() {
        float duration = 1f;
        float t = 0f;
        OverlayCanvasGroup.interactable = true;
        OverlayCanvasGroup.blocksRaycasts = true;
        while (t < duration) {
            t += Time.deltaTime;
            OverlayCanvasGroup.alpha = Mathf.Lerp(0, 1, t / duration);
            yield return null;
        }
    }

    IEnumerator FadeOut() {
        float duration = 1f;
        float t = 0f;
        OverlayCanvasGroup.interactable = false;
        OverlayCanvasGroup.blocksRaycasts = false;
        while (t < duration) {
            t += Time.deltaTime;
            OverlayCanvasGroup.alpha = Mathf.Lerp(1, 0, t / duration);
            yield return null;
        }
    }



    IEnumerator AppearOnScreen() {
        StartCoroutine(FadeIn());
        // wired_PlayersBlock.localPosition = new Vector3(
        //     wired_PlayersBlock.localPosition.x,
        //     -140,
        //     wired_PlayersBlock.localPosition.z
        // );
        wired_Player1Tail.UnfurlTail();
        wired_Player2Tail.UnfurlTail();
        yield return new WaitForSeconds(1.25f);
        // now fade in the stat block
        while (wired_StatsBlock.alpha < 1) {
            wired_StatsBlock.alpha += Time.deltaTime;
            yield return null;
        }

        yield return null;
    }

    IEnumerator DisappearFromScreen() {
        // fade out the stat block
        while (wired_StatsBlock.alpha > 0) {
            wired_StatsBlock.alpha -= Time.deltaTime;
            yield return null;
        }
        wired_Player1Tail.RefurlTail();
        wired_Player2Tail.RefurlTail();

        StartCoroutine(FadeOut());
        yield return null;
    }
}
