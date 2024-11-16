using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum UITileStatus {
    CONFIGURE_TRANSFORM,
    CONFIGURE_TERRAFORMER,
    PLACED,
    NOT_SET
};

public enum GamepieceType {
    TERRAFORMER,
    PIG
};

public class GamepieceTileAssignment {
    public int Anchor;
    public int Team;
    public GamepieceType Type;
}

public class UI_UnityTile : MonoBehaviour
{
    public readonly UITileStatus STARTING_STATUS = UITileStatus.CONFIGURE_TRANSFORM;

    UITileStatus currentStatus = UITileStatus.NOT_SET;
    public TMPro.TextMeshPro DebugText;
    public SpriteRenderer spriteRenderer;
    public Transform liftableFacePlate;
    public Tile registeredTile;
    public List<int> WorkableRotations = new List<int>();
    public List<Transform> GamepieceAnchors = new List<Transform>();
    public List<GamepieceTileAssignment> GamepieceAssignments = new List<GamepieceTileAssignment>();
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
    public UITileStatus GetStatus() {
        return currentStatus;
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
        foreach (var anchor in GamepieceAnchors) {
            anchor.gameObject.SetActive(false);
        }
    }

    void SetupTerraformerStep() {
        for (var i = 0; i < registeredTile.Placements.Length; i++) {
            GamepieceAnchors[
                registeredTile.Placements[i]
            ].gameObject.SetActive(true);
        }
        HighlightChosenTerraformer();
    }

    void HighlightChosenTerraformer() {
        for (var i = 0; i < registeredTile.Placements.Length; i++) {
            bool thereIsATerraformerHere = GamepieceAssignments.Exists(
                assignment => assignment.Anchor == registeredTile.Placements[i]
            );
            GamepieceAnchors[
                registeredTile.Placements[i]
            ].Find("HangingDot").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, thereIsATerraformerHere ? 1f : 0.75f);
        }
    }

    public void AssignTerraformerToAnchor(int anchor) {
        GamepieceAssignments.Clear(); // TODO: May need to preserve old ones in weird scenarios
        GamepieceAssignments.Add(new GamepieceTileAssignment {
            Anchor = anchor,
            Team = 0,
            Type = GamepieceType.TERRAFORMER
        });
        HighlightChosenTerraformer();
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
        DebugText.text = registeredTile.Name + registeredTile.Rotation.ToString();
        spriteRenderer.transform.rotation = Quaternion.identity;
        spriteRenderer.transform.Rotate(0, 0, -90*registeredTile.Rotation);

        foreach (var anchor in GamepieceAnchors) {
            anchor.localRotation = Quaternion.identity;
            anchor.transform.Rotate(0, 0, -90*registeredTile.Rotation*-1);
        }
    }
}