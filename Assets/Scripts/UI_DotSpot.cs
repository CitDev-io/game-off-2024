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
        GetComponent<SpriteRenderer>().enabled = false;
    }

    public void Enable() {
        visibility = true;
        GetComponent<SpriteRenderer>().enabled = true;
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