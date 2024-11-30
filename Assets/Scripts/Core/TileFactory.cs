using System.Collections.Generic;

public class TileFactory {
    public static Tile CreateTileWithRotations(TileType type, int Rotations) {
        Tile tile = CreateTile(type);
        for (int i = 0; i < Rotations; i++) {
            tile.Rotate();
        }
        return tile;
    }
    public static Tile CreateTile(TileType type) {
        switch(type) {
            case TileType.A:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1)
                    },
                    new List<Road> {
                        new Road(CardinalDirection.SOUTH, 0),
                    },
                    new int[] { 7, 5, 4 },
                    new Obelisk(2),
                    false
                );
            case TileType.B:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.FARM, 0),
                        new MicroEdge(MicroEdgeType.FARM, 0),
                        new MicroEdge(MicroEdgeType.FARM, 0),
                        new MicroEdge(MicroEdgeType.FARM, 0),
                        new MicroEdge(MicroEdgeType.FARM, 0),
                        new MicroEdge(MicroEdgeType.FARM, 0),
                        new MicroEdge(MicroEdgeType.FARM, 0),
                        new MicroEdge(MicroEdgeType.FARM, 0)
                    },
                    new List<Road>(),
                    new int[] { 5, 4 },
                    new Obelisk(1),
                    false
                );
            case TileType.C:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.CITY, 0),
                        new MicroEdge(MicroEdgeType.CITY, 0),
                        new MicroEdge(MicroEdgeType.CITY, 0),
                        new MicroEdge(MicroEdgeType.CITY, 0),
                        new MicroEdge(MicroEdgeType.CITY, 0),
                        new MicroEdge(MicroEdgeType.CITY, 0),
                        new MicroEdge(MicroEdgeType.CITY, 0),
                        new MicroEdge(MicroEdgeType.CITY, 0)
                    },
                    new List<Road>(),
                    new int[] { 4 },
                    null,
                    false
                );
            case TileType.D:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.FARM, 3),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 3)
                    },
                    new List<Road> {
                        new Road(CardinalDirection.WEST, 0),
                        new Road(CardinalDirection.EAST, 0)
                    },
                    new int[] { 4, 1, 7, 10 },
                    null,
                    false
                );
            case TileType.E:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.CITY, 0),
                        new MicroEdge(MicroEdgeType.CITY, 0),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1)
                    },
                    new List<Road>(),
                    new int[] { 1, 12 },
                    null,
                    false
                );
            case TileType.F:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.CITY, 0),
                        new MicroEdge(MicroEdgeType.CITY, 0),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.CITY, 0),
                        new MicroEdge(MicroEdgeType.CITY, 0)
                    },
                    new List<Road>(),
                    new int[] { 4, 1, 7 },
                    null,
                    true    // shield
                );
            case TileType.G:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.FARM, 0),
                        new MicroEdge(MicroEdgeType.FARM, 0),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1)
                    },
                    new List<Road>(),
                    new int[] { 1, 4, 7 },
                    null,
                    false
                );
            case TileType.H:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.CITY, 0),
                        new MicroEdge(MicroEdgeType.CITY, 0),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.CITY, 2),
                        new MicroEdge(MicroEdgeType.CITY, 2),
                    },
                    new List<Road>(),
                    new int[] { 5, 4, 3 },
                    null,
                    false
                );
            case TileType.I:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.CITY, 0),
                        new MicroEdge(MicroEdgeType.CITY, 0),
                        new MicroEdge(MicroEdgeType.CITY, 2),
                        new MicroEdge(MicroEdgeType.CITY, 2),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                    },
                    new List<Road>(),
                    new int[] { 1, 4, 5 },
                    null,
                    false
                );
            case TileType.J:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.CITY, 0),
                        new MicroEdge(MicroEdgeType.CITY, 0),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                    },
                    new List<Road> {
                        new Road(CardinalDirection.SOUTH, 3),
                        new Road(CardinalDirection.EAST, 3)
                    },
                    new int[] { 1, 3, 8, 7 },
                    null,
                    false
                );
            case TileType.K:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.FARM, 3),
                        new MicroEdge(MicroEdgeType.FARM, 3),
                        new MicroEdge(MicroEdgeType.FARM, 3),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 3),
                    },
                    new List<Road> {
                        new Road(CardinalDirection.SOUTH, 0),
                        new Road(CardinalDirection.WEST, 0)
                    },
                    new int[] { 3, 1, 6, 5 },
                    null,
                    false
                );
            case TileType.L:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.CITY, 3),
                        new MicroEdge(MicroEdgeType.CITY, 3),
                        new MicroEdge(MicroEdgeType.FARM, 4),
                        new MicroEdge(MicroEdgeType.FARM, 5),
                        new MicroEdge(MicroEdgeType.FARM, 5),
                        new MicroEdge(MicroEdgeType.FARM, 6),
                        new MicroEdge(MicroEdgeType.FARM, 6),
                        new MicroEdge(MicroEdgeType.FARM, 4),
                    },
                    new List<Road> {
                        new Road(CardinalDirection.SOUTH, 0),
                        new Road(CardinalDirection.EAST, 1),
                        new Road(CardinalDirection.WEST, 2)
                    },
                    new int[] { 7, 5, 3, 1, 9, 8, 6 },
                    null,
                    false
                );
            case TileType.M:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.CITY, 0),
                        new MicroEdge(MicroEdgeType.CITY, 0),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.CITY, 0),
                        new MicroEdge(MicroEdgeType.CITY, 0)
                    },
                    new List<Road>(),
                    new int[] { 9, 12 },
                    null,
                    true
                );
            case TileType.N:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.FARM, 0),
                        new MicroEdge(MicroEdgeType.FARM, 0),
                        new MicroEdge(MicroEdgeType.FARM, 0),
                        new MicroEdge(MicroEdgeType.FARM, 0),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1)
                    },
                    new List<Road>(),
                    new int[] { 12, 9 },
                    null,
                    false
                );
            case TileType.O:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.FARM, 3),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 3),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                    },
                    new List<Road> {
                        new Road(CardinalDirection.SOUTH, 0),
                        new Road(CardinalDirection.EAST, 0)
                    },
                    new int[] { 5, 0, 8, 13 },
                    null,
                    true
                );
            case TileType.P:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.FARM, 3),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 3),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                    },
                    new List<Road> {
                        new Road(CardinalDirection.SOUTH, 0),
                        new Road(CardinalDirection.EAST, 0)
                    },
                    new int[] { 5, 0, 8, 13 },
                    null,
                    false
                );
            case TileType.Q:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.FARM, 0),
                        new MicroEdge(MicroEdgeType.FARM, 0),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                    },
                    new List<Road>(),
                    new int[] { 7, 4 },
                    null,
                    true
                );
            case TileType.R:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.FARM, 0),
                        new MicroEdge(MicroEdgeType.FARM, 0),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                    },
                    new List<Road>(),
                    new int[] { 7, 4 },
                    null,
                    false
                );
            case TileType.S:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.FARM, 0),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                    },
                    new List<Road> {
                        new Road(CardinalDirection.SOUTH, 3)
                    },
                    new int[] { 13, 4, 14, 7 },
                    null,
                    true    //shield
                );
            case TileType.T:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.FARM, 0),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                    },
                    new List<Road> {
                        new Road(CardinalDirection.SOUTH, 3)
                    },
                    new int[] { 13, 4, 14, 7 },
                    null,
                    false
                );
            case TileType.U:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                    },
                    new List<Road> {
                        new Road(CardinalDirection.SOUTH, 0),
                        new Road(CardinalDirection.NORTH, 0)
                    },
                    new int[] { 4, 3, 5 },
                    null,
                    false
                );
            case TileType.V:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                    },
                    new List<Road> {
                        new Road(CardinalDirection.SOUTH, 0),
                        new Road(CardinalDirection.WEST, 0)
                    },
                    new int[] { 3, 10, 6 },
                    null,
                    false
                );
            case TileType.W:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.FARM, 5),
                        new MicroEdge(MicroEdgeType.FARM, 5),
                        new MicroEdge(MicroEdgeType.FARM, 5),
                        new MicroEdge(MicroEdgeType.FARM, 4),
                        new MicroEdge(MicroEdgeType.FARM, 4),
                        new MicroEdge(MicroEdgeType.FARM, 3),
                        new MicroEdge(MicroEdgeType.FARM, 3),
                        new MicroEdge(MicroEdgeType.FARM, 5),
                    },
                    new List<Road> {
                        new Road(CardinalDirection.SOUTH, 0),
                        new Road(CardinalDirection.WEST, 1),
                        new Road(CardinalDirection.EAST, 2)
                    },
                    new int[] { 7, 3, 5, 6, 8, 0 },
                    null,
                    false
                );
            case TileType.X:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.FARM, 5),
                        new MicroEdge(MicroEdgeType.FARM, 6),
                        new MicroEdge(MicroEdgeType.FARM, 6),
                        new MicroEdge(MicroEdgeType.FARM, 7),
                        new MicroEdge(MicroEdgeType.FARM, 7),
                        new MicroEdge(MicroEdgeType.FARM, 4),
                        new MicroEdge(MicroEdgeType.FARM, 4),
                        new MicroEdge(MicroEdgeType.FARM, 5),
                    },
                    new List<Road> {
                        new Road(CardinalDirection.SOUTH, 0),
                        new Road(CardinalDirection.WEST, 1),
                        new Road(CardinalDirection.EAST, 2),
                        new Road(CardinalDirection.NORTH, 3)
                    },
                    new int[] { 7, 3, 5, 1, 6, 0, 2, 8 },
                    null,
                    false
                );
            default:
                return new Tile(
                    "blanksies",
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.FARM, 0),
                        new MicroEdge(MicroEdgeType.FARM, 0),
                        new MicroEdge(MicroEdgeType.FARM, 0),
                        new MicroEdge(MicroEdgeType.FARM, 0),
                        new MicroEdge(MicroEdgeType.FARM, 0),
                        new MicroEdge(MicroEdgeType.FARM, 0),
                        new MicroEdge(MicroEdgeType.FARM, 0),
                        new MicroEdge(MicroEdgeType.FARM, 0)
                    },
                    new List<Road>(),
                    new int[] { 4 },
                    null,
                    false
                );
        }
    }
}