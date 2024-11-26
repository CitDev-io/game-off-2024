using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
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
    // public TMPro.TextMeshPro DebugText;
    public SpriteRenderer spriteRenderer;
    public Transform liftableFacePlate;
    public Tile registeredTile;
    public List<int> WorkableRotations = new List<int>();
    public List<Transform> GamepieceAnchors = new List<Transform>();
    public SpriteRenderer RotationIcon;
    public GridPosition gridPosition;

    float FLOATING_ELEVATION_START = -0.55f;
    float FLOATING_ELEVATION_SPEED = 2f;
    float FLOATING_ELEVATION_ARCH = 0.001f;
    float PLACED_ELEVATION = 0.4f;

    public void RotateWorkableOnly() {
        if (currentStatus == UITileStatus.CONFIGURE_TRANSFORM) {
            if (WorkableRotations.Count == 0) return;

            int indexOfCurrent = WorkableRotations.IndexOf(registeredTile.Rotation);
            registeredTile.Rotation = WorkableRotations[(indexOfCurrent + 1) % WorkableRotations.Count];
            OnTileRotated();
        }
    }

    public void RegisterTile(Tile tile, GridPosition worldSpot, List<int> workableRotations, TileSurvey ts)
    {
        registeredTile = tile;
        registeredTile.NormalizedSurvey = ts;
        WorkableRotations = workableRotations;
        gridPosition = worldSpot;
        tile.OnTileRotated += OnTileRotated;
        transform.position = new Vector3(
            worldSpot.x - 7f,
            worldSpot.y - 7f,
            PLACED_ELEVATION
        );
        spriteRenderer.sprite = Resources.Load<Sprite>("Images/Tiles/Tile_" + tile.Name);
        OnTileRotated();
    }
    public UITileStatus GetStatus() {
        return currentStatus;
    }
    public void SetStatus(UITileStatus status, PlayerSlot player) {
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
                FinalizePlacement(player);
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
        GamepieceAnchors[4].gameObject.SetActive(registeredTile.obelisk != null);

        
        
        // we have a list of placements int[]{ 1, 6, 9 }
        // need to cut that list down based on whether or not that tile is eligible for a terraformer
        // GamepieceAnchor[anchorid] not groups

        //foreach placement
        // index within placements array is the group id

        // gamepieceanchors are a filled 15 array. won't have nulls.
        for (var groupId = 0; groupId < registeredTile.Placements.Length; groupId++) {
            int anchorIdForGroup = registeredTile.Placements[groupId];

// ******** NO anchors found for the full city tile!!!
            
            // need to call tile or board and get the opposing first tpi/components to make the lookup
            List<CardinalDirection> DirectionOfExistingTile = registeredTile.GetCardinalDirectionsForGroupIndexId(groupId);

            bool eligibleToPlaceTf = true;
            foreach(CardinalDirection cdirPossible in DirectionOfExistingTile) {
                bool AnchorIsForRoad = registeredTile.Roads
                    .Any(e => e.RoadGroupId == groupId && registeredTile.LocalToNormalizedDirection(e.localizedDirection) == cdirPossible);


                Tile NeighborTile = registeredTile.NormalizedSurvey.TileInDirection(cdirPossible);
    
                if (NeighborTile == null) {
                    continue;
                }
                
                if (AnchorIsForRoad) {
                    CardinalDirection OppositeDirection = (CardinalDirection)(((int)cdirPossible + 2) % 4);
                    int GroupId = NeighborTile.GetGroupIndexIdForNormalizedDirectionalSide(OppositeDirection);

                    eligibleToPlaceTf = eligibleToPlaceTf && GameObject
                        .Find("GameBoard")
                        .GetComponent<GameBoardManager>()
                        .LookupTFEligibilityForTileAndGroupId(
                            NeighborTile,
                            GroupId
                        );
                } else {
                    List<MicroEdge> microEdgesInThisDirection = registeredTile.Edges
                        .Where(e => e.EdgeGroupId == groupId && registeredTile.GetCardinalDirectionForRotatedEdge(e) == cdirPossible)
                        .ToList();

                    // repeating unneccessarily honestly
                    foreach(MicroEdge me in microEdgesInThisDirection) {
                        // find the opposing microedge in the neighbor tile
                        // i ultimatiely want to get whatever is currently in the opposite normalized spot id
                        int thisLocalizedIndex = (registeredTile.Edges.IndexOf(me) + (registeredTile.Rotation * 2))% 8;
                        int thisLocalizedOpponentIndex = (thisLocalizedIndex + (thisLocalizedIndex % 2 == 0 ? 5 : 3)) % 8;

                        // remove rotations to normalize
                        int normalizedOppoSpotId = (thisLocalizedOpponentIndex + (NeighborTile.Rotation * 6)) % 8;
                        // if oppo weren't rotated, we would just grab the ME at normalizedOppoSpotId
                        // UnityEngine.Debug.Log("my oppo localized index is: " + oppoLocalizedIndex);
                        MicroEdge oppoSpot = NeighborTile.Edges[normalizedOppoSpotId];
                        
                        // find the group id of the opposing microedge
                        int oppoGroupId = oppoSpot.EdgeGroupId;
                        
                        int GroupId = oppoGroupId;
                        eligibleToPlaceTf = eligibleToPlaceTf && GameObject
                        .Find("GameBoard")
                        .GetComponent<GameBoardManager>()
                        .LookupTFEligibilityForTileAndGroupId(
                            NeighborTile,
                            GroupId
                        );
                    }
                }
            }

            GamepieceAnchors[anchorIdForGroup].gameObject.SetActive(eligibleToPlaceTf);
        }
        HighlightChosenTerraformer();
    }

    void HighlightChosenTerraformer() {
        for (var i = 0; i < registeredTile.Placements.Length; i++) {
            bool thereIsATerraformerHere = registeredTile.GamepieceAssignments.Exists(
                assignment => assignment.Anchor == registeredTile.Placements[i]
            );
            GamepieceAnchors[
                registeredTile.Placements[i]
            ].Find("HangingDot").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, thereIsATerraformerHere ? 1f : 0.75f);
        }
    }

    public void AssignTerraformerToAnchorFacade(int anchor, PlayerSlot slot) {
        registeredTile.AssignTerraformerToAnchor(anchor, slot, gridPosition);
        HighlightChosenTerraformer();
    }

    public void CancelGamepiecePlacement() {
        registeredTile.ClearGamepiecePlacement();
        HighlightChosenTerraformer();
    }

    void Start() {
        if (currentStatus == UITileStatus.NOT_SET) {
            SetStatus(STARTING_STATUS, PlayerSlot.PLAYER1);
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

    void FinalizePlacement(PlayerSlot player) {
        liftableFacePlate.position = new Vector3(transform.position.x, transform.position.y, PLACED_ELEVATION);
        foreach(GamepieceTileAssignment gamepiece in registeredTile.GamepieceAssignments) {
            var Gamepiece = Instantiate(
                Resources.Load<GameObject>("Gamepiece_" + gamepiece.Type.ToString())
            );
            Gamepiece.GetComponent<UI_AnchorTag>().AnchorId = gamepiece.Anchor;
            Gamepiece.GetComponent<UI_AnchorTag>().gridPosition = gridPosition;
            Gamepiece.GetComponent<SpriteRenderer>().sprite =
                Resources.Load<Sprite>(
                    "Terraformer" + 
                    (player == PlayerSlot.PLAYER1 ? "Blue" : "Pink")
                    + "Glow");
            Debug.Log("***** GAMEPIECE DROPPED");
            Gamepiece.transform.position = GamepieceAnchors[gamepiece.Anchor].position + new Vector3(0, 0.1f, 0f);
            Gamepiece.transform.rotation = Quaternion.Euler(0f, 0, -12.5f);
            Gamepiece.transform.SetParent(transform);
        }
    }

    void OnDestroy() {
        registeredTile.OnTileRotated -= OnTileRotated;
    }

    void OnTileRotated()
    {
        // DebugText.text = registeredTile.Name + registeredTile.Rotation.ToString();
        spriteRenderer.transform.rotation = Quaternion.identity;
        spriteRenderer.transform.Rotate(0, 0, -90*registeredTile.Rotation);

        foreach (var anchor in GamepieceAnchors) {
            anchor.localRotation = Quaternion.identity;
            anchor.transform.Rotate(0, 0, -90*registeredTile.Rotation*-1);
        }
    }
}