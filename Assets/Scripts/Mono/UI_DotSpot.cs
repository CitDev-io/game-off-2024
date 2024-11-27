using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DotSpot : MonoBehaviour
{
    GridPosition coords;
    bool visibility = false;

    public void OnMouseDown()
    {
        if (!visibility) return;
        ReportPositionClick();
    }

    public void Disable() {
        visibility = false;
        GetComponent<MeshRenderer>().enabled = false;
    }

    public void Enable() {
        visibility = true;
        PlayerSlot curPlayer = GameObject
            .Find("GameBoard")
            .GetComponent<GameBoardManager>()
            .CurrentPlayer;

        GetComponent<MeshRenderer>().enabled = true;
        Material mat = Resources.Load<Material>("Materials/" + (curPlayer == PlayerSlot.PLAYER1 ? "TileHighlightBlue" : "TileHighlightPink"));
        GetComponent<MeshRenderer>().SetMaterials(new List<Material> { mat });
    }

    public void SetCoords(GridPosition g) {
        coords = g;
    }

    void ReportPositionClick() {
        GameObject
            .Find("GameBoard")
            .GetComponent<GameBoardManager>()
            .OnDotClick(coords);
    }
}
