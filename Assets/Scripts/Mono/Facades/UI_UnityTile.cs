using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UITileStatus {
    CONFIGURE_TRANSFORM,
    CONFIGURE_TERRAFORMER,
    PLACED,
    NOT_SET
};

public class UI_UnityTile : MonoBehaviour
{
    public readonly UITileStatus STARTING_STATUS = UITileStatus.CONFIGURE_TRANSFORM;

    UITileStatus currentStatus = UITileStatus.NOT_SET;
    public TMPro.TextMeshPro DebugText;
    public SpriteRenderer spriteRenderer;
    public Transform liftableFacePlate;
    public Tile registeredTile;
    public List<int> WorkableRotations = new List<int>();
    public List<Transform> DoodadAnchors = new List<Transform>();
    public SpriteRenderer RotationIcon;
    public GridPosition gridPosition;

    float FLOATING_ELEVATION_START = -0.55f;
    float FLOATING_ELEVATION_SPEED = 2f;
    float FLOATING_ELEVATION_ARCH = 0.001f;
    float PLACED_ELEVATION = 0f;

    public void RotateWorkableOnly() {
        if (currentStatus == UITileStatus.CONFIGURE_TRANSFORM) {
            if (WorkableRotations.Count == 0) return;

            int indexOfCurrent = WorkableRotations.IndexOf(registeredTile.Rotation);
            registeredTile.Rotation = WorkableRotations[(indexOfCurrent + 1) % WorkableRotations.Count];
            OnTileRotated();
        }
    }

    public void RegisterTile(Tile tile, GridPosition worldSpot, List<int> workableRotations)
    {
        registeredTile = tile;
        WorkableRotations = workableRotations;
        gridPosition = worldSpot;
        tile.OnTileRotated += OnTileRotated;
        transform.position = new Vector3(
            worldSpot.x - 7f,
            worldSpot.y - 7f,
            PLACED_ELEVATION
        );
        spriteRenderer.sprite = Resources.Load<Sprite>("Images/Tile_" + tile.Name);
        OnTileRotated();
    }

    public void SetStatus(UITileStatus status) {
        if (currentStatus == status) {
            return;
        }
        currentStatus = status;
        ClearStagingUI();
        switch (status) {
            case UITileStatus.CONFIGURE_TRANSFORM:
                RotationIcon.gameObject.SetActive(true);
                StartCoroutine(StagingFloat());
                break;
            case UITileStatus.CONFIGURE_TERRAFORMER:
            // check your z axis and animate it to where you need it
                SetupTerraformerStep();
                break;
            case UITileStatus.PLACED:
                FinalizePlacement();
                break;
        }
    }

    void ClearStagingUI() {
        RotationIcon.gameObject.SetActive(false);
    }

    void SetupTerraformerStep() {
        // show the terraformer layer
    }

    void Start() {
        if (currentStatus == UITileStatus.NOT_SET) {
            SetStatus(STARTING_STATUS);
        }
    }

    IEnumerator StagingFloat() {
        liftableFacePlate.position = new Vector3(
                liftableFacePlate.position.x,
                liftableFacePlate.position.y,
                FLOATING_ELEVATION_START
            );
        while (currentStatus == UITileStatus.CONFIGURE_TRANSFORM) {
            liftableFacePlate.position = new Vector3(
                liftableFacePlate.position.x,
                liftableFacePlate.position.y,
                liftableFacePlate.position.z + (Mathf.Sin(Time.time * FLOATING_ELEVATION_SPEED) * FLOATING_ELEVATION_ARCH)
            );
            yield return null;
        }
    }

    void FinalizePlacement() {
        liftableFacePlate.position = new Vector3(transform.position.x, transform.position.y, PLACED_ELEVATION);
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
