using System;
using System.Collections.Generic;
using System.Linq;

public class AssembledRoad {
    int index = 0;
    bool collected = false;
    public PlayerSlot collectedBy;
    public List<TileAndPlacementGroupIndex> tilePis = new List<TileAndPlacementGroupIndex>();

    public AssembledRoad(int i) {
        index = i;
    }

    public void MarkAsCollectedBy(PlayerSlot ps) {
        collectedBy = ps;
        collected = true;
    }

    public bool IsCompleteAndUncollected() {
        return IsComplete() && !collected;
    }

    public bool IsComplete() {
        if (tilePis.Count < 1) return false;

        return tilePis.All(tpi => tpi.isConnectedAtAllRoadJoints());
    }

    public void AddTilePlacementIndex(TileAndPlacementGroupIndex tpi) {
        tilePis.Add(tpi);
    }

    public bool ContainsRoad(Road r) {
        foreach (TileAndPlacementGroupIndex tpi in tilePis) {
            if (tpi.tile.Roads.Contains(r) && tpi.groupIndexId == r.RoadGroupId) {
                return true;
            }
        }
        return false;
    }

    public void Merge(AssembledRoad ar) {
        tilePis = tilePis.Union(ar.tilePis).ToList();
    }

    public int GetUniqueTileCount() {
        return tilePis.Count;
    }
};
