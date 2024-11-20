using System.Collections.Generic;
using System.Linq;

public class TileAndPlacementGroupIndex {
    public Tile tile;
    public int placementIndex;
    public TileAndPlacementGroupIndex(Tile t, int pi) {
        tile = t;
        placementIndex = pi;
    }

    public bool isConnectedAtAllCityJoints() {
        foreach (CardinalDirection dir in System.Enum.GetValues(typeof(CardinalDirection))) {
            bool isACityJoint = tile.GetEdgeTypeByNormalizedDir(dir) == EdgeType.CITY;
            MicroEdgeSpot mespot = tile.DecodeDirectionToTrueMicroEdgeSpot(dir);
            bool isInThisGroup = tile
                .FindMicroEdgeFromLocalizedEdgeSpot(mespot).EdgeGroupId == placementIndex;
            bool isNotConnected = tile.NormalizedSurvey.TileInDirection(dir) == null;
            if (isInThisGroup && isACityJoint && isNotConnected) {
                return false;
            }
        }
        return true;       
    }

    public bool isConnectedAtAllRoadJoints() {
        List<Road> roadsOnTPI = tile.Roads
            .FindAll(r => r.RoadGroupId == placementIndex);
        List<CardinalDirection> NormalizedJointDirs = roadsOnTPI
            .Select(r => tile.LocalToNormalizedDirection(r.localizedDirection))
            .ToList();
        foreach (CardinalDirection dir in NormalizedJointDirs) {
            if (tile.NormalizedSurvey.TileInDirection(dir) == null) {
                return false;
            }
        }
        return true;
    }
}
