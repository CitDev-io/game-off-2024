using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

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

    public void AssignTerraformerToAnchor(int anchor, PlayerSlot slot, GridPosition gridPositionForThisCluelessFuckingTile) {
        GamepieceAssignments.Clear(); // TODO: May need to preserve old ones in weird scenarios
        GamepieceAssignments.Add(new GamepieceTileAssignment {
            Anchor = anchor,
            Team = slot,
            Type = GamepieceType.TERRAFORMER,
            Position = gridPositionForThisCluelessFuckingTile
        });
    }

    public void ClearGamepiecePlacement() {
        GamepieceAssignments.Clear();
    }

    // warning: revisit for fields
    public int GetGroupIndexIdForNormalizedDirectionalSide(CardinalDirection cDir) {
        EdgeType edgeType = GetEdgeTypeByNormalizedDir(cDir);
        if (edgeType == EdgeType.ROAD) {
            Road road = Roads.Find(r => LocalToNormalizedDirection(r.localizedDirection) == cDir);
            return road.RoadGroupId;
        } else {
            MicroEdgeSpot spot = DecodeDirectionToTrueMicroEdgeSpot(cDir);
            return FindMicroEdgeFromLocalizedEdgeSpot(spot).EdgeGroupId;
        }

    }

    public bool IsGroupIdEligibleForTerraformer(int groupIndexId) {
        List<CardinalDirection> NeighborDirections = GetCardinalDirectionsForGroupIndexId(groupIndexId);
        UnityEngine.Debug.Log("Neighbor Directions: " + string.Join(", ", NeighborDirections));
        foreach(CardinalDirection NeighborCardinalAddress in NeighborDirections) {
            CardinalDirection oppositeDirection = (CardinalDirection)(((int)NeighborCardinalAddress + 2) % 4);
            Tile Neighbor = NormalizedSurvey.TileInDirection(NeighborCardinalAddress);
            
            if (Neighbor == null) {
                UnityEngine.Debug.Log("NO NEIGHBOR TO THE " + NeighborCardinalAddress.ToString());
                continue;
            }
            UnityEngine.Debug.Log("Neighbor: " + Neighbor.Name);
            List<GamepieceTileAssignment> gamepieces = Neighbor.GetAllGamepiecesInGroupId(
                Neighbor.GetGroupIndexIdForNormalizedDirectionalSide(oppositeDirection)
            );

            if (gamepieces.Exists(gpa => gpa.Type == GamepieceType.TERRAFORMER)) {
                return false;
            }
        }
        return true;
    }
    
    public List<CardinalDirection> GetCardinalDirectionsForGroupIndexId(int groupIndexId) {
        if (Roads.Exists(r => r.RoadGroupId == groupIndexId)) {
            return Roads
                .FindAll(r => r.RoadGroupId == groupIndexId)
                .Select(r => LocalToNormalizedDirection(r.localizedDirection))
                .ToList();
        }
        if (Edges.Exists(e => e.EdgeGroupId == groupIndexId)) {
            return Edges
                .FindAll(e => e.EdgeGroupId == groupIndexId)
                .Select(e => GetCardinalDirectionFromEdgeIndexPosition(e))
                .Distinct()
                .ToList();
        }
        // nothing here for farmses
        return new List<CardinalDirection>();
    }

    CardinalDirection GetCardinalDirectionFromEdgeIndexPosition(MicroEdge lostEdge) {
        int indexPosition = Edges.IndexOf(lostEdge);
        switch (indexPosition) {
            case 0:
            case 1:
                return LocalToNormalizedDirection(CardinalDirection.NORTH);
            case 2:
            case 3:
                return LocalToNormalizedDirection(CardinalDirection.EAST);
            case 4:
            case 5:
                return LocalToNormalizedDirection(CardinalDirection.SOUTH);
            case 6:
            case 7:
                return LocalToNormalizedDirection(CardinalDirection.WEST);
            default:
                return CardinalDirection.NORTH;
        }
    }

    public List<GamepieceTileAssignment> GetAllGamepiecesInGroupId(int groupIndexId) {
        return GamepieceAssignments
            .Where(gpa => gpa.Anchor == Placements[groupIndexId])
            .ToList();
    }

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
