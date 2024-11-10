using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_StageRotateButton : MonoBehaviour
{
    void OnMouseDown()
    {
        GameObject
            .Find("GameBoard")
            .GetComponent<GameBoardManager>()
            .RotateStagedTile();
    }
}
