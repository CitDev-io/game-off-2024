using System;
using System.Collections.Generic;
using System.Linq;

public class AssembledRoad {
    int index = 0;
    bool collected = false;
    public PlayerSlot collectedBy = PlayerSlot.NEUTRAL;
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

    public bool IsShared() {
        int countOfTerraformerOwners = tilePis
            .SelectMany(tpi => tpi.tile.GamepieceAssignments)
            .Where(gpa => gpa.Type == GamepieceType.TERRAFORMER)
            .Select(gpa => gpa.Team)
            .Distinct().Count();

        return countOfTerraformerOwners > 1;
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

    public List<PlayerSlot> WinningTerraformerOwners() {
        var TeamWonList = tilePis
            .SelectMany(tpi => tpi.tile.GamepieceAssignments.Where(gpa => gpa.Anchor == tpi.tile.Placements[tpi.groupIndexId]))
            .Where(gpa => gpa.Type == GamepieceType.TERRAFORMER)
            .Select(gpa => gpa.Team);
        
        // get all teams with the highest population
        return TeamWonList
            .GroupBy(team => team)
            .OrderByDescending(group => group.Count())
            .Where(group => group.Count() == TeamWonList.GroupBy(team => team).Max(group => group.Count()))
            .Select(group => group.Key)
            .ToList();
    }

    public int GetUniqueTerraformerOwners() {
        return tilePis
            .SelectMany(tpi => tpi.tile.GamepieceAssignments.Where(gpa => gpa.Anchor == tpi.tile.Placements[tpi.groupIndexId]))
            .Where(gpa => gpa.Type == GamepieceType.TERRAFORMER)
            .Select(gpa => gpa.Team)
            .Distinct().Count();
    }
};
