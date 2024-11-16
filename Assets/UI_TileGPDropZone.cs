using UnityEngine;

public class UI_TileGPDropZone : MonoBehaviour
{
    public void OnMouseDown() {
        int anchorClicked = int.Parse(transform.parent.name.Replace("GPAnchor-", ""));
        GameObject
            .Find("GameBoard")
            .GetComponent<GameBoardManager>()
            .UserAssignsTerraformerToAnchor(anchorClicked);
    }
}
