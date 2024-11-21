using System.Collections.Generic;
public delegate void TileRotated();

public class Tile
{
    public TileRotated OnTileRotated;
    public Tile(string name, List<MicroEdge> edges, List<Road> roads, int[] placements, Obelisk o = null)
    {
        Name = name;
        Edges = edges;
        Roads = roads;
        Placements = placements;
        obelisk = o;
    }

    public string Name;
    public TileSurvey NormalizedSurvey = new TileSurvey();
    public List<MicroEdge> Edges = new List<MicroEdge>();
    public List<Road> Roads = new List<Road>();
    public int[] Placements = new int[]{};
    public int Rotation = 0;
    public Obelisk obelisk;
    public List<GamepieceTileAssignment> GamepieceAssignments = new List<GamepieceTileAssignment>();

    public void Rotate()
    {
        Rotation = (Rotation + 1) % 4;
        OnTileRotated?.Invoke();
    }

    public bool FitsWithOtherToThe(Tile otherTile, CardinalDirection direction)
    {
        CardinalDirection oppositeDirection = (CardinalDirection)(((int)direction + 2) % 4);
        EdgeType thisEdge = GetEdgeTypeByNormalizedDir(direction);
        EdgeType otherEdge = otherTile.GetEdgeTypeByNormalizedDir(oppositeDirection);
        return thisEdge == otherEdge;
    }

    public void AssignTerraformerToAnchor(int anchor, PlayerSlot slot) {
        GamepieceAssignments.Clear(); // TODO: May need to preserve old ones in weird scenarios
        GamepieceAssignments.Add(new GamepieceTileAssignment {
            Anchor = anchor,
            Team = slot,
            Type = GamepieceType.TERRAFORMER
        });
    }

    public void ClearGamepiecePlacement() {
        GamepieceAssignments.Clear();
    }
    /*
        0   1           6   7
        -   -           -   -
     7  -   -  2    5   -   -  0
     6  -   -  3    4   -   -  1
        -   -           -   -
        5   4           3   2
        rotation 0     rotation 1


        4   5           2   3
        -   -           -   -
    3  -   -  6    1   -   -  4
    2  -   -  7    0   -   -  5
        -   -           -   -
        1   0           7   6
        rotation 2     rotation 3
    */

    public CardinalDirection LocalToNormalizedDirection(CardinalDirection direction)
    {
        return (CardinalDirection)(((int)direction + Rotation) % 4);
    }

    public CardinalDirection NormalizedToLocalDirection(CardinalDirection? direction)
    {
        if (direction == null)
        {
            return CardinalDirection.NORTH;
        }
        return (CardinalDirection)(((int)direction + 4 - Rotation) % 4);
    }

    public MicroEdgeSpot DecodeDirectionToTrueMicroEdgeSpot(CardinalDirection direction)
    {
        switch (NormalizedToLocalDirection(direction))
        {
            case CardinalDirection.NORTH:
                return MicroEdgeSpot.NORTH_LEFT;
            case CardinalDirection.EAST:
                return MicroEdgeSpot.EAST_TOP;
            case CardinalDirection.SOUTH:
                return MicroEdgeSpot.SOUTH_RIGHT;
            case CardinalDirection.WEST:
                return MicroEdgeSpot.WEST_BOTTOM;
            default:
                return MicroEdgeSpot.NORTH_LEFT;
        }
    }

    public EdgeType GetEdgeTypeByNormalizedDir(CardinalDirection direction) {
        if (Roads.Exists(r =>LocalToNormalizedDirection(r.localizedDirection) == direction)) {
            return EdgeType.ROAD;
        }

        MicroEdgeSpot spotToCheck = DecodeDirectionToTrueMicroEdgeSpot(direction);
        return FindMicroEdgeFromLocalizedEdgeSpot(spotToCheck).type == MicroEdgeType.FARM
                ? EdgeType.FARM
                : EdgeType.CITY;
    }

    public MicroEdge FindMicroEdgeFromLocalizedEdgeSpot(MicroEdgeSpot spot)
    {
        return Edges[(int) spot];
    }

    #nullable enable
    public RoadCheck? GetRoadCheck(CardinalDirection dir) {
        CardinalDirection decodedDirection = NormalizedToLocalDirection(dir);
        
        Road? road = Roads.Find(r => r.localizedDirection == decodedDirection);

        if (road == null) {
            return null;
        }
        var matches = Roads.FindAll(r => r.RoadGroupId == road.RoadGroupId);
        CardinalDirection? attachment = null;
        bool terminates = matches.Count == 1;
        if (!terminates) {
            attachment = matches.Find(r => r.localizedDirection != decodedDirection)?.localizedDirection;
        }
        return new RoadCheck(dir, terminates, NormalizedToLocalDirection(attachment));
    }
}
