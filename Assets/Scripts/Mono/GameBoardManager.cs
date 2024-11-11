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
    Dictionary<GridPosition, UI_DotSpot> ClickGrid = new Dictionary<GridPosition, UI_DotSpot>();
    public GameObject TilePlacementUserInput;

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
        TilePlacementUserInput.SetActive(true);
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
        
        return StagedTile;
    }

    void ExitStagingMode() {
        TilePlacementUserInput.SetActive(false);
    }

    void InitializeGame() {
        TheGrid = new TileGrid(TileFactory.CreateTile(TileType.D));
        var StarterTile = StageUnityTileAt(TheGrid.grid[7, 7], new GridPosition(7, 7));
        StarterTile.SetStatus(UITileStatus.PLACED);
        ExitStagingMode();
    }

    void InitializeClickGrid() {
        for (int x = 0; x < 15; x++) {
            for (int y = 0; y < 15; y++) {
                GameObject dot = Instantiate(DotPrefab, new Vector3(x - 7, y - 7, 0), Quaternion.identity);
                dot.transform.parent = transform;

                GridPosition coords = new GridPosition(x, y);
                UI_DotSpot dotSpot = dot.GetComponent<UI_DotSpot>();
                dotSpot.SetCoords(coords);
                ClickGrid[coords] = dotSpot;
            }
        }   
    }

    public void UIINGRESS_OnPlayerAccept() {
        if (StagedTile == null) return;
        StagedTile.SetStatus(UITileStatus.PLACED);
        TheGrid.PlaceTile(StagedTile.registeredTile, StagedTile.gridPosition);
        PlaceTileInHand(TheBag.DrawTile());
        ExitStagingMode();
    }

    public void UIINGRESS_OnPlayerCancel() {
        Destroy(StagedTile.gameObject);
        StagedTile = null;
        UpdateClickGrid();
        ExitStagingMode();
    }
}
