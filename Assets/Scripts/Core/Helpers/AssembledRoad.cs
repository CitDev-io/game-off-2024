using System;
using System.Collections.Generic;
using System.Linq;

public class AssembledRoad {
    int index = 0;
    public List<TileAndPlacementIndex> tilePis = new List<TileAndPlacementIndex>();

    public AssembledRoad(int i) {
        index = i;
    }

    public bool IsComplete() {
        if (tilePis.Count < 1) return false;

        return tilePis.All(tpi => tpi.isConnectedAtAllRoadJoints());
    }

    public void AddTilePlacementIndex(TileAndPlacementIndex tpi) {
        tilePis.Add(tpi);
    }

    public bool ContainsRoad(Road r) {
        foreach (TileAndPlacementIndex tpi in tilePis) {
            if (tpi.tile.Roads.Contains(r) && tpi.placementIndex == r.RoadGroupId) {
                return true;
            }
        }
        return false;
    }

    public void Merge(AssembledRoad ar) {
        tilePis = tilePis.Union(ar.tilePis).ToList();
    }

    public int GetTileCount() {
        return tilePis.Count;
    }
};
