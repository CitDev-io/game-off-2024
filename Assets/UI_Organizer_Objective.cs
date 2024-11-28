using UnityEngine;
using UnityEngine.UI;

public class UI_Organizer_Objective : MonoBehaviour
{
    public TMPro.TextMeshProUGUI Wired_ObjectiveText;
    public TMPro.TextMeshProUGUI Wired_StatusText;
    public Image Wired_Icon;
    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }
}
