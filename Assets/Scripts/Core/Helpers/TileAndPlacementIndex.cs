using System.Collections.Generic;
using System.Linq;

public class TileAndPlacementIndex {
    public Tile tile;
    public int placementIndex;
    public TileAndPlacementIndex(Tile t, int pi) {
        tile = t;
        placementIndex = pi;
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
