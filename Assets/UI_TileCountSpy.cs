using UnityEngine;
using UnityEngine.UI;

public class UI_TileCountSpy : MonoBehaviour
{
    GameBoardManager gameBoardManager;
    TMPro.TextMeshProUGUI tileCountText;

    
    void Start()
    {
        gameBoardManager = FindObjectOfType<GameBoardManager>();
        tileCountText = GetComponent<TMPro.TextMeshProUGUI>();
    }

    void FixedUpdate()
    {
        tileCountText.text = gameBoardManager.GridGameInstance.TilesLeft() + "/72";
    }
}
