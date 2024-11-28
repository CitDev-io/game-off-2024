using System;
using System.Collections.Generic;
using System.Linq;

public class AssembledCity {
    int index = 0;
    bool collected = false;
    public List<TileAndPlacementGroupIndex> tilePis = new List<TileAndPlacementGroupIndex>();
    public PlayerSlot collectedBy = PlayerSlot.NEUTRAL;
    public AssembledCity(int i) {
        index = i;
    }

    public void MarkAsCollectedBy(PlayerAssignment assignment) {
        collectedBy = assignment.slot;
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

    public bool IsShared() {
        int countOfTerraformerOwners = tilePis
            .SelectMany(tpi => tpi.tile.GamepieceAssignments)
            .Where(gpa => gpa.Type == GamepieceType.TERRAFORMER)
            .Select(gpa => gpa.Team)
            .Distinct().Count();

        return countOfTerraformerOwners > 1;
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
