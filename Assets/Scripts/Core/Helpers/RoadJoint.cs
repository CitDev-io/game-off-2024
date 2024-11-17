using System.Collections.Generic;

public class RoadJoint {
    public Tile owningTile;
    public int placementIndex;
    public List<CardinalDirection> NormalizedDirections = new List<CardinalDirection>();
    public List<AssembledRoad> assembledRoads = new List<AssembledRoad>();
    
    public RoadJoint(Tile t, int pi, List<CardinalDirection> dirs_normalized, List<AssembledRoad> ars) {
        owningTile = t;
        placementIndex = pi;
        NormalizedDirections = dirs_normalized;
        assembledRoads = ars;
    }
};
