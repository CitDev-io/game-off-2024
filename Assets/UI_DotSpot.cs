using UnityEngine;
using UnityEngine.UI;

public class UI_DotSpot : MonoBehaviour
{
    GridPosition coords;
    bool enabled = false;

    public void OnMouseDown()
    {
        if (!enabled) return;
        ReportPositionClick();
    }

    public void Disable() {
        enabled = false;
        GetComponent<SpriteRenderer>().color = Color.clear;
    }

    public void Enable() {
        enabled = true;
        GetComponent<SpriteRenderer>().color = Color.black;
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
