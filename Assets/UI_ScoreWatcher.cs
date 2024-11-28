using UnityEngine;
using UnityEngine.UI;

public class UI_ScoreWatcher : MonoBehaviour
{
    GameBoardManager gameBoardManager;
    TMPro.TextMeshProUGUI scoreText;
    public PlayerSlot slotWatch;
    public Image TF1;
    public Image TF2;
    public Image TF3;
    public Image TF4;
    public Image TF5;
    public Image TF6;
    public Image TF7;

    
    void Start()
    {
        gameBoardManager = FindObjectOfType<GameBoardManager>();
        scoreText = GetComponent<TMPro.TextMeshProUGUI>();
    }

    void FixedUpdate()
    {
        if (gameBoardManager.GridGameInstance.scoreboard == null) {
            Debug.Log("NO SCOREBOARD??");
        }
        scoreText.text = gameBoardManager.GridGameInstance.scoreboard.GetScoreForPlayerSlot(slotWatch).ToString();
        int tfCount = gameBoardManager.GridGameInstance.scoreboard.Stats[slotWatch].TerraformerCount;
        string color = slotWatch == PlayerSlot.PLAYER1 ? "Blue" : "Pink";
        TF1.sprite = (tfCount >= 1) ? Resources.Load<Sprite>("Images/Terraformer" + color) : Resources.Load<Sprite>("Images/Terraformer" + color + "Greyed");
        TF2.sprite = (tfCount >= 2) ? Resources.Load<Sprite>("Images/Terraformer" + color) : Resources.Load<Sprite>("Images/Terraformer" + color + "Greyed");
        TF3.sprite = (tfCount >= 3) ? Resources.Load<Sprite>("Images/Terraformer" + color) : Resources.Load<Sprite>("Images/Terraformer" + color + "Greyed");
        TF4.sprite = (tfCount >= 4) ? Resources.Load<Sprite>("Images/Terraformer" + color) : Resources.Load<Sprite>("Images/Terraformer" + color + "Greyed");
        TF5.sprite = (tfCount >= 5) ? Resources.Load<Sprite>("Images/Terraformer" + color) : Resources.Load<Sprite>("Images/Terraformer" + color + "Greyed");
        TF6.sprite = (tfCount >= 6) ? Resources.Load<Sprite>("Images/Terraformer" + color) : Resources.Load<Sprite>("Images/Terraformer" + color + "Greyed");
        TF7.sprite = (tfCount >= 7) ? Resources.Load<Sprite>("Images/Terraformer" + color) : Resources.Load<Sprite>("Images/Terraformer" + color + "Greyed");
        if (gameBoardManager.CurrentPlayer == slotWatch)
        {
            transform.parent.localPosition = new Vector3(50f, transform.parent.localPosition.y, transform.parent.localPosition.z);
        }
        else
        {
            transform.parent.localPosition = new Vector3(0f, transform.parent.localPosition.y, transform.parent.localPosition.z);
        }
    }
}
