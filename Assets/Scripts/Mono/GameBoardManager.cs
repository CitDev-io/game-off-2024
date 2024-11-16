using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBoardManager : MonoBehaviour
{
    public GameObject DotPrefab;
    public GameObject UnityTilePrefab;
    public Image InHandTileImg;
    TileGrid TheGrid;
    TileBag TheBag = new TileBag();
    public Tile TemporarilyGlobalTileInHand;
    public UI_UnityTile StagedTile;
    int Confirmations = 0;
    Dictionary<GridPosition, UI_DotSpot> ClickGrid = new Dictionary<GridPosition, UI_DotSpot>();
    public GameObject TilePlacementUserInput;
    Coroutine CameraOperator;

    public float DEFAULT_CAMERA_FOV = 45f;
    public float ZOOMED_CAMERA_FOV = 25f;
    public float AUTO_ZOOM_SPEED = 2.5f;
    public float AUTO_PAN_SPEED = 3f;
    public float AUTO_PAN_SNAP_DISTANCE = 0.1f;
    public float AUTO_ZOOM_SNAP_DISTANCE = 0.1f;
    
    void Start()
    {
        InitializeGame();
        InitializeClickGrid();
        StartGame();
    }

    void StartGame() {
        PlaceTileInHand(TheBag.DrawTile());
    }

    void PlaceTileInHand(Tile tile) {
        TemporarilyGlobalTileInHand = tile;
        UpdateClickGrid();
        InHandTileImg.sprite = Resources.Load<Sprite>("Images/Tile_" + tile.Name);
        InHandTileImg.transform.rotation = Quaternion.Euler(
            new Vector3(0, 0, (-90 * tile.Rotation) + 21.42f)
        );
    }

    void UpdateClickGrid() {
        ClearClickGrid();

        foreach (GridPosition pos in TheGrid.GetEligiblePositionsAllRotations(TemporarilyGlobalTileInHand)) {
            ClickGrid[pos].Enable();
        }
    }

    void ClearClickGrid() {
        foreach (UI_DotSpot dot in ClickGrid.Values) {
            dot.Disable();
        }
    }

    List<int> GetWorkableRotations(Tile tile, GridPosition coords) {
        List<int> workableRotations = new List<int>();
        for (int i = 0; i < 4; i++) {
            Tile checkTile = TileFactory.CreateTileWithRotations(
                (TileType) Enum.Parse(typeof(TileType), tile.Name),
                i
            );
            if (TheGrid.CanPlaceTile(checkTile, coords)) {
                workableRotations.Add(i);
            }
        }
        return workableRotations;
    }

    UI_UnityTile StageUnityTileAt(Tile tile, GridPosition coords) {
        ClearClickGrid();
        List<int> Workables = GetWorkableRotations(tile, coords);
        Tile tileToDrop = TileFactory.CreateTileWithRotations(
            (TileType) Enum.Parse(typeof(TileType), tile.Name),
            Workables.Count > 0 ? Workables[0] : 0
        );
        StagedTile = Instantiate(UnityTilePrefab).GetComponent<UI_UnityTile>();
        StagedTile.RegisterTile(
            tileToDrop,
            coords,
            Workables
        );
        EvaluateStagingPhase();
        return StagedTile;
    }

    void ClearStagingUI() {
        TilePlacementUserInput.SetActive(false);
    }

    void InitializeGame() {
        TheGrid = new TileGrid(TileFactory.CreateTile(TileType.D));
        var StarterTile = StageUnityTileAt(TheGrid.grid[7, 7], new GridPosition(7, 7));
        StarterTile.SetStatus(UITileStatus.PLACED);
        ClearStagingUI();
    }

    void InitializeClickGrid() {
        for (int x = 0; x < 15; x++) {
            for (int y = 0; y < 15; y++) {
                GameObject dot = Instantiate(DotPrefab, new Vector3(x - 7, y - 7, 0.2f), Quaternion.Euler(-90f, 0, 0));
                dot.transform.parent = transform;

                GridPosition coords = new GridPosition(x, y);
                UI_DotSpot dotSpot = dot.GetComponent<UI_DotSpot>();
                dotSpot.SetCoords(coords);
                ClickGrid[coords] = dotSpot;
            }
        }   
    }

    void CameraControlTo(Vector3 target, float cameraFov) {
        if (CameraOperator != null) {
            StopCoroutine(CameraOperator);
        }
        CameraOperator = StartCoroutine(RoutineCameraControl(target, cameraFov));
    }

    IEnumerator RoutineCameraControl(Vector3 target, float cameraFov) {
        while (Vector3.Distance(Camera.main.transform.position, target) > AUTO_PAN_SNAP_DISTANCE || Mathf.Abs(Camera.main.fieldOfView - cameraFov) > AUTO_ZOOM_SNAP_DISTANCE) {
            Vector3 lerpSpot = Vector3.Lerp(
                Camera.main.transform.position,
                target,
                Time.deltaTime * AUTO_PAN_SPEED
            );
            Camera.main.transform.position = new Vector3(
                lerpSpot.x,
                lerpSpot.y,
                Camera.main.transform.position.z
            );
            
            Camera.main.fieldOfView = Mathf.Lerp(
                Camera.main.fieldOfView,
                cameraFov,
                Time.deltaTime * AUTO_ZOOM_SPEED
            );
            yield return null;
        }
        Camera.main.transform.position = new Vector3(
            target.x,
            target.y,
            Camera.main.transform.position.z
        );
        Camera.main.fieldOfView = cameraFov;
    }

    void EvaluateStagingPhase() {
        ClearStagingUI();
        if (Confirmations == 0) {
            TilePlacementUserInput.SetActive(true);
            StagedTile.SetStatus(UITileStatus.CONFIGURE_TRANSFORM);
            CameraControlTo(
                new Vector3(
                    StagedTile.transform.position.x,
                    StagedTile.transform.position.y,
                    -8)
                , DEFAULT_CAMERA_FOV
            );
        }
        if (Confirmations == 1) {
            //**** TODO: check if they have terraformers to place
            //if so, set it up
            TilePlacementUserInput.SetActive(true);
            StagedTile.SetStatus(UITileStatus.CONFIGURE_TERRAFORMER);
            CameraControlTo(
                new Vector3(
                    StagedTile.transform.position.x,
                    StagedTile.transform.position.y,
                    -8),
                ZOOMED_CAMERA_FOV
            );
            // ************* if not, skip to the next step
        }
        if (Confirmations == 2 && StagedTile.GetStatus() != UITileStatus.PLACED) {
            Confirmations = 0;
            if (StagedTile.GamepieceAssignments.Count > 0) {
                // Attach a for-real game piece to the tile's assigned anchor
                // Subtract it from the player's inventory
                foreach(GamepieceTileAssignment assignment in StagedTile.GamepieceAssignments) {
                    Debug.Log("CA CHING");
                }
            }
            StagedTile.SetStatus(UITileStatus.PLACED);
            TheGrid.PlaceTile(StagedTile.registeredTile, StagedTile.gridPosition);


            // extend this area here.
            // may need to be multi-step and synchronous
            
            // Evaluate any newly scored points
            // Change player's turn
            
            // this is basically kicking to the next turn
            PlaceTileInHand(TheBag.DrawTile());
            CameraControlTo(Camera.main.transform.position, DEFAULT_CAMERA_FOV);
        }
    }

    void CancelStagingInput() {
        Destroy(StagedTile.gameObject);
        Confirmations = 0;
        StagedTile = null;
        UpdateClickGrid();
        ClearStagingUI();
    }

    public void OnUserCameraMovement() {
        if (CameraOperator != null)
            StopCoroutine(CameraOperator);
    }

    public void UIINGRESS_OnPlayerAccept() {
        if (StagedTile == null) return;
        Confirmations++;

        EvaluateStagingPhase();
    }

    public void UIINGRESS_OnPlayerBackStep() {
        if (StagedTile == null) return;
        Confirmations--;

        if (Confirmations > -1) {
            EvaluateStagingPhase();
        } else {
            CancelStagingInput();
        }
    }

    public void RotateTileInHand() {
        if (TemporarilyGlobalTileInHand == null) return;
        TemporarilyGlobalTileInHand.Rotate();

        InHandTileImg.transform.rotation = Quaternion.Euler(
            new Vector3(0, 0, -90 * TemporarilyGlobalTileInHand.Rotation + 21.42f)
        );
    }

    public void RotateStagedTile() {
        if (StagedTile == null) return;
        StagedTile.RotateWorkableOnly();
    }

    public void OnDotClick(GridPosition coords)
    {
        if (TemporarilyGlobalTileInHand == null) return;

        StageUnityTileAt(TemporarilyGlobalTileInHand, coords);
    }

    public void UserAssignsTerraformerToAnchor(int anchorIndex) {
        if (StagedTile == null) return;
        StagedTile.AssignTerraformerToAnchor(anchorIndex);
    }
}
