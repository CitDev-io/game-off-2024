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
    Dictionary<GridPosition, UI_DotSpot> ClickGrid = new Dictionary<GridPosition, UI_DotSpot>();

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
            new Vector3(0, 0, -90 * tile.Rotation)
        );
    }

    void UpdateClickGrid() {
        foreach (UI_DotSpot dot in ClickGrid.Values) {
            dot.Disable();
        }

        foreach (GridPosition pos in TheGrid.GetEligiblePositions(TemporarilyGlobalTileInHand)) {
            ClickGrid[pos].Enable();
        }
    }

    public void RotateTileInHand() {
        if (TemporarilyGlobalTileInHand == null) return;
        TemporarilyGlobalTileInHand.Rotate();
        UpdateClickGrid();
        InHandTileImg.transform.rotation = Quaternion.Euler(
            new Vector3(0, 0, -90 * TemporarilyGlobalTileInHand.Rotation)
        );
    }

    public void OnDotClick(GridPosition coords)
    {
        if (TemporarilyGlobalTileInHand == null) return;

        if (TheGrid.CanPlaceTile(TemporarilyGlobalTileInHand, coords)) {
            TheGrid.PlaceTile(TemporarilyGlobalTileInHand, coords);
            DropUnityTileAt(TemporarilyGlobalTileInHand, coords);
            PlaceTileInHand(TheBag.DrawTile());
        }
    }

    void DropUnityTileAt(Tile tile, GridPosition coords) {
        Instantiate(UnityTilePrefab).GetComponent<UI_UnityTile>().RegisterTile(tile, new Vector3(coords.x - 7, coords.y - 7, 0f));
    }

    void InitializeGame() {
        TheGrid = new TileGrid(TileFactory.CreateTile(TileType.A));
        DropUnityTileAt(TheGrid.grid[7, 7], new GridPosition(7, 7));
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
}

public class TileBag {
    List<Tile> tiles = new List<Tile>();

    public TileBag() {
        AddTileTimes(TileType.A, 2);
        AddTileTimes(TileType.B, 4);
        AddTileTimes(TileType.C, 1);
        AddTileTimes(TileType.D, 4);
        AddTileTimes(TileType.E, 5);
        AddTileTimes(TileType.F, 2);
        AddTileTimes(TileType.G, 1);
        AddTileTimes(TileType.H, 3);
        AddTileTimes(TileType.I, 2);
        AddTileTimes(TileType.J, 3);
        AddTileTimes(TileType.K, 3);
        AddTileTimes(TileType.L, 3);
        AddTileTimes(TileType.M, 2);
        AddTileTimes(TileType.N, 3);
        AddTileTimes(TileType.O, 2);
        AddTileTimes(TileType.P, 3);
        AddTileTimes(TileType.Q, 1);
        AddTileTimes(TileType.R, 3);
        AddTileTimes(TileType.S, 2);
        AddTileTimes(TileType.T, 1);
        AddTileTimes(TileType.U, 8);
        AddTileTimes(TileType.V, 9);
        AddTileTimes(TileType.W, 4);
        AddTileTimes(TileType.X, 1);
    }

    void AddTileTimes(TileType type, int times) {
        for (int i = 0; i < times; i++) {
            tiles.Add(TileFactory.CreateTile(type));
        }
    }

    public Tile DrawTile() {
        int index = Random.Range(0, tiles.Count);
        Tile tile = tiles[index];
        tiles.RemoveAt(index);
        return tile;
    }
}
