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
            case TileType.A: // todo: monastery
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
                    }
                );
            case TileType.B: // todo: monastery
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
                    new List<Road>()
                );
            case TileType.C:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1)
                    },
                    new List<Road>()
                );
            case TileType.D:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 1)
                    },
                    new List<Road> {
                        new Road(CardinalDirection.WEST, 0),
                        new Road(CardinalDirection.EAST, 0)
                    }
                );
            case TileType.E:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1)
                    },
                    new List<Road>()
                );
            case TileType.F:    // todo: SHIELD
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1)
                    },
                    new List<Road>()
                );
            case TileType.G:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1)
                    },
                    new List<Road>()
                );
            case TileType.H:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.CITY, 2),
                        new MicroEdge(MicroEdgeType.CITY, 2),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1)
                    },
                    new List<Road>()
                );
            case TileType.I:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.CITY, 2),
                        new MicroEdge(MicroEdgeType.CITY, 2),
                    },
                    new List<Road>()
                );
            case TileType.J:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                    },
                    new List<Road> {
                        new Road(CardinalDirection.SOUTH, 0),
                        new Road(CardinalDirection.EAST, 0)
                    }
                );
            case TileType.K:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
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
                    }
                );
            case TileType.L:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 3),
                        new MicroEdge(MicroEdgeType.FARM, 3),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                    },
                    new List<Road> {
                        new Road(CardinalDirection.SOUTH, 0),
                        new Road(CardinalDirection.EAST, 1),
                        new Road(CardinalDirection.WEST, 2)
                    }
                );
            case TileType.M:    // todo: shield
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                    },
                    new List<Road>()
                );
            case TileType.N:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                    },
                    new List<Road>()
                );
            case TileType.O:        // todo: SHIELD
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                    },
                    new List<Road> {
                        new Road(CardinalDirection.SOUTH, 0),
                        new Road(CardinalDirection.EAST, 0)
                    }
                );
            case TileType.P:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                    },
                    new List<Road> {
                        new Road(CardinalDirection.SOUTH, 0),
                        new Road(CardinalDirection.EAST, 0)
                    }
                );
            case TileType.Q:        // todo: SHIELD
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                    },
                    new List<Road>()
                );
            case TileType.R:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                    },
                    new List<Road>()
                );
            case TileType.S:        // todo: SHIELD
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                    },
                    new List<Road> {
                        new Road(CardinalDirection.SOUTH, 0)
                    }
                );
            case TileType.T:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                        new MicroEdge(MicroEdgeType.CITY, 1),
                    },
                    new List<Road> {
                        new Road(CardinalDirection.SOUTH, 0)
                    }
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
                    }
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
                    }
                );
            case TileType.W:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 3),
                        new MicroEdge(MicroEdgeType.FARM, 3),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                    },
                    new List<Road> {
                        new Road(CardinalDirection.SOUTH, 0),
                        new Road(CardinalDirection.WEST, 1),
                        new Road(CardinalDirection.EAST, 2)
                    }
                );
            case TileType.X:
                return new Tile(
                    type.ToString(),
                    new List<MicroEdge> {
                        new MicroEdge(MicroEdgeType.FARM, 1),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 2),
                        new MicroEdge(MicroEdgeType.FARM, 3),
                        new MicroEdge(MicroEdgeType.FARM, 3),
                        new MicroEdge(MicroEdgeType.FARM, 4),
                        new MicroEdge(MicroEdgeType.FARM, 4),
                        new MicroEdge(MicroEdgeType.FARM, 1),
                    },
                    new List<Road> {
                        new Road(CardinalDirection.SOUTH, 0),
                        new Road(CardinalDirection.WEST, 1),
                        new Road(CardinalDirection.EAST, 2),
                        new Road(CardinalDirection.NORTH, 3)
                    }
                );
            default:
                return new Tile(
                    "blanksies",
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
                    new List<Road>()
                );
        }
    }
}