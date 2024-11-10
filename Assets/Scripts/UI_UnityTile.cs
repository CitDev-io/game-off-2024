using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_UnityTile : MonoBehaviour
{
    public TMPro.TextMeshPro DebugText;
    public SpriteRenderer spriteRenderer;
    public Transform liftableFacePlate;
    public Tile registeredTile;
    bool IsStaging = true;
    public List<int> WorkableRotations = new List<int>();
    public SpriteRenderer RotationIcon;
    public GridPosition gridPosition;

    void Start() {
        StartCoroutine(StagingFloat());
    }

    IEnumerator StagingFloat() {
        while (IsStaging) {
            liftableFacePlate.position = new Vector3(
                liftableFacePlate.position.x,
                liftableFacePlate.position.y,
                liftableFacePlate.position.z + (Mathf.Sin(Time.time * 2) * 0.1f) - 0.5f
            );
            yield return null;
            liftableFacePlate.position = new Vector3(
                liftableFacePlate.position.x,
                liftableFacePlate.position.y,
                0f
            );
        }
    }

    void Update() {
        // if staging, hover
        if (!IsStaging) {
            RotationIcon.gameObject.SetActive(false);
        }

        // if not staging, check if we're not placed and place
    }

    public void RotateWorkableOnly() {
        if (IsStaging) {
            if (WorkableRotations.Count == 0) return;
            // cycle through rotations
            int indexOfCurrent = WorkableRotations.IndexOf(registeredTile.Rotation);
            registeredTile.Rotation = WorkableRotations[(indexOfCurrent + 1) % WorkableRotations.Count];
            OnTileRotated();
        }
    }

    public void FinalizePlacement() {
        IsStaging = false;
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }

    public void RegisterTile(Tile tile, GridPosition worldSpot)
    {
        registeredTile = tile;
        tile.OnTileRotated += OnTileRotated;
        transform.position = new Vector3(
            worldSpot.x - 7f,
            worldSpot.y - 7f,
            0f
        );
        spriteRenderer.sprite = Resources.Load<Sprite>("Images/Tile_" + tile.Name);
        OnTileRotated();
    }

    void OnDestroy() {
        registeredTile.OnTileRotated -= OnTileRotated;
    }

    void OnTileRotated()
    {
        spriteRenderer.transform.rotation = Quaternion.identity;
        spriteRenderer.transform.Rotate(0, 0, -90*registeredTile.Rotation);
        DebugText.text = registeredTile.Name + " " + registeredTile.Rotation;
    }
}
