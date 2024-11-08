using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_UnityTile : MonoBehaviour
{
    public TMPro.TextMeshPro DebugText;
    Tile registeredTile;
    public void RegisterTile(Tile tile, Vector3 worldSpot)
    {
        registeredTile = tile;
        tile.OnTileRotated += OnTileRotated;
        transform.position = worldSpot;
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Images/Tile_" + tile.Name);
        OnTileRotated();
    }

    void OnDestroy() {
        registeredTile.OnTileRotated -= OnTileRotated;
    }

    void OnTileRotated()
    {
        transform.Rotate(0, 0, -90*registeredTile.Rotation);
        DebugText.text = registeredTile.Name + " " + registeredTile.Rotation;
    }
}
