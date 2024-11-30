using System.Collections;
using UnityEngine;
// using UnityEditor;
// using UnityEditor.Overlays;

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
//             myComponent.Present(null);
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
    

    public void Present(Scoreboard? s) {
        if (s != null) {

        }
        StartCoroutine(AppearOnScreen());
    }

    public void CloseOverlay() {
        StartCoroutine(DisappearFromScreen());
    }

    IEnumerator FadeIn() {
        float duration = 1f;
        float t = 0f;
        OverlayCanvasGroup.interactable = true;
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
        while (t < duration) {
            t += Time.deltaTime;
            OverlayCanvasGroup.alpha = Mathf.Lerp(1, 0, t / duration);
            yield return null;
        }
    }



    IEnumerator AppearOnScreen() {
        StartCoroutine(FadeIn());
        wired_PlayersBlock.localPosition = new Vector3(
            wired_PlayersBlock.localPosition.x,
            -140,
            wired_PlayersBlock.localPosition.z
        );
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
