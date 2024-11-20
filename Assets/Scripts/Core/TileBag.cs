using System;
using System.Collections.Generic;

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

    public void ReturnTile(Tile tile) {
        if (tiles.Count > 0) {
            tiles.Add(tile);
        } else {
            AddTileTimes((TileType) Enum.Parse(typeof(TileType), "D"), 1);
        }
    }

    public Tile DrawTile() {
        Random rand = new Random();
        int index = rand.Next(0, tiles.Count);
        Tile tile = tiles[index];
        tiles.RemoveAt(index);
        return tile;
    }
}
