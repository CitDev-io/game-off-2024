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
                    new Obelisk(2)
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
                    new Obelisk(1)
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
                    new int[] { 4 }
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
                    new int[] { 4, 1, 7, 10 }
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
                    new int[] { 1, 12 }
                );
            case TileType.F:    // todo: SHIELD
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
                    new int[] { 4, 1, 7 }
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
                    new int[] { 1, 4, 7 }
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
                    new int[] { 5, 4, 3 }
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
                    new int[] { 1, 4, 5 }
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
                    new int[] { 1, 3, 8, 7 }
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
                    new int[] { 4, 1, 6, 5 }
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
                    new int[] { 7, 5, 3, 1, 9, 8, 6 }
                );
            case TileType.M:    // todo: shield
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
                    new int[] { 0, 12 }
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
                    new int[] { 12, 0 }
                );
            case TileType.O:        // todo: SHIELD
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
                    new int[] { 5, 0, 8, 13 }
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
                    new int[] { 5, 0, 8, 13 }
                );
            case TileType.Q:        // todo: SHIELD
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
                    new int[] { 7, 4 }
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
                    new int[] { 7, 4 }
                );
            case TileType.S:        // todo: SHIELD
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
                    new int[] { 13, 4, 14, 7 }
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
                    new int[] { 13, 4, 14, 7 }
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
                    new int[] { 4, 3, 5 }
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
                    new int[] { 3, 10, 6 }
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
                    new int[] { 7, 3, 5, 6, 8, 0 }
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
                    new int[] { 7, 3, 5, 1, 6, 0, 2, 8 }
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
                    new int[] { 4 }
                );
        }
    }
}