using System;
using System.Collections.Generic;
using System.Linq;

public class AssembledCity {
    int index = 0;
    bool collected = false;
    public List<TileAndPlacementGroupIndex> tilePis = new List<TileAndPlacementGroupIndex>();

    public AssembledCity(int i) {
        index = i;
    }

    public void MarkAsCollected() {
        collected = true;
    }

    public bool IsCompleteAndUncollected() {
        return IsComplete() && !collected;
    }

    public bool IsComplete() {
        if (tilePis.Count < 1) return false;

        return tilePis.All(tpi => tpi.isConnectedAtAllCityJoints());
    }

    public void AddTilePlacementIndex(TileAndPlacementGroupIndex tpi) {
        tilePis.Add(tpi);
    }

    public bool ContainsTile(Tile t) {
        foreach (TileAndPlacementGroupIndex tpi in tilePis) {
            if (tpi.tile == t) {
                return true;
            }
        }
        return false;
    }

    public void Merge(AssembledCity ac) {
        tilePis = tilePis.Union(ac.tilePis).ToList();
    }

    public int GetUniqueTileCount() {
        return tilePis.Count;
    }
};
