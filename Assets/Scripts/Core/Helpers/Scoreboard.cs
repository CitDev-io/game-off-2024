using System.Collections.Generic;
using System.Linq;

public class PlayerStatSheet {
    public PlayerStatSheet(PlayerType pt) {
        PlayerType = pt;
    }
    public PlayerType PlayerType = PlayerType.HUMAN;
    public static int TERRAFORMER_STARTING_COUNT = 7;
    public int Score = 0;
    public SecretObjectiveRank Rank = SecretObjectiveRank.RECRUIT;
    public int TerraformerCount = TERRAFORMER_STARTING_COUNT;
    public List<SecretObjective> Objectives = new();


    public int TerraformersCollected = 0;
    public int TurnsInARowPlacingTerraformer = 0;
    public int TurnsInARow_NOT_PlacingTerraformer = 0;
    public int TilesPlaced = 0;
    public int ObjectivesCompleted = 0;
    public int RookieObjectiveCompleted = 0;
    public int DirtlingObjectiveCompleted = 0;
    public int LandscraperObjectiveCompleted = 0;
    public int StarShaperObjectiveCompleted = 0;
    public int ScoreAtLastTurnStart = 0;

    public Dictionary<EdgeType, int> TerraformersPlacedOnPOIType = new() {
        {EdgeType.ROAD, 0},
        {EdgeType.CITY, 0},
        {EdgeType.FARM, 0},
        {EdgeType.OBELISK, 0}
    };

    public Dictionary<EdgeType, int> ScoredPOITypes = new() {
        {EdgeType.ROAD, 0},
        {EdgeType.CITY, 0},
        {EdgeType.FARM, 0},
        {EdgeType.OBELISK, 0}
    };

    public Dictionary<EdgeType, int> HelpedOppClaimedPOITypes = new() {
        {EdgeType.ROAD, 0},
        {EdgeType.CITY, 0},
        {EdgeType.FARM, 0},
        {EdgeType.OBELISK, 0}
    };

    public Dictionary<ScoringEventType, int> ScoreByEventType = new() {
        {ScoringEventType.ROADCOMPLETED, 0},
        {ScoringEventType.CITYCOMPLETED, 0},
        {ScoringEventType.OBELISKCOMPLETED, 0},
        {ScoringEventType.FARMSCORED, 0},
        {ScoringEventType.INCOMPLETE, 0},
        {ScoringEventType.SECRET_OBJECTIVE, 0},
        {ScoringEventType.PROMOTION, 0}
    };
}

public class Scoreboard {
    public Dictionary<PlayerSlot, PlayerStatSheet> Stats = new();
    PlayerAssignment CurrentTurnPlayer;
    public int TerraformersRecoveredThisTurn = 0;
    BoardInventory _inventory;

    public Scoreboard(PlayerManifest pm, BoardInventory inv) {
        _inventory = inv;
        foreach (PlayerAssignment pa in pm.players) {
            Stats.Add(pa.slot, new PlayerStatSheet(pa.type));
            _GiveRecruitMissionsToPlayer(pa.slot);
        }
        CurrentTurnPlayer = new PlayerAssignment {
            slot = pm.players[0].slot,
            type = pm.players[0].type
        }; 
    }

    public void ReportTilePlacementByCurrentPlayer(Tile t) {
        PlayerStatSheet pss = Stats[CurrentTurnPlayer.slot];
        if (t == null) {
            return;
        }
        // terraformer placement stats
        if (t.GamepieceAssignments.Any(gpa => gpa.Type == GamepieceType.TERRAFORMER)) {
            pss.TurnsInARowPlacingTerraformer++;
            pss.TurnsInARow_NOT_PlacingTerraformer = 0;
            // GP Drop Counts
            
            t.GamepieceAssignments
                .Where(gpa => gpa.Type == GamepieceType.TERRAFORMER)
                .ToList()
                .ForEach(gpa => {
                    pss.TerraformersPlacedOnPOIType[
                        t.GetPOITypeForGamepiece(gpa)
                    ]++;
                });
        } else {
            pss.TurnsInARow_NOT_PlacingTerraformer++;
            pss.TurnsInARowPlacingTerraformer = 0;
        }

        // tile place stats
        pss.TilesPlaced++;

        // helped oppo stats
        int groupCount = t.Placements.Count();
        for(var i=0; i < groupCount; i++) {
            if (t.Roads.Any(r => r.RoadGroupId == i)) {
                Road r = t.Roads.First(r => r.RoadGroupId == i);
                AssembledRoad ar = _inventory.AssembledRoads.FirstOrDefault(ar => ar.tilePis.Any(tpi => tpi.tile == t && tpi.groupIndexId == i));
                bool hasClaimsByOtherPlayers = ar.tilePis.Any(tpi => tpi.tile.OtherPlayerHasTerraformerInGroup(CurrentTurnPlayer.slot, tpi.groupIndexId));
                if (hasClaimsByOtherPlayers) {
                    pss.HelpedOppClaimedPOITypes[EdgeType.ROAD]++;
                }
                continue;
            }
            if (t.obelisk != null && t.obelisk.GroupId == i) {
                continue;
            }
            MicroEdgeType met = t.Edges.First(e => e.EdgeGroupId == i).type;
            switch(met) {
                case MicroEdgeType.FARM:
                    OwnedFarm of = _inventory.OwnedFarms.FirstOrDefault(of => of.tilePis.Any(tpi => tpi.tile == t && tpi.groupIndexId == i));
                    bool hasClaimsByOtherPlayers = of.tilePis.Any(tpi => tpi.tile.OtherPlayerHasTerraformerInGroup(CurrentTurnPlayer.slot, tpi.groupIndexId));
                    if (hasClaimsByOtherPlayers) {
                        pss.HelpedOppClaimedPOITypes[EdgeType.FARM]++;
                    }
                    continue;
                case MicroEdgeType.CITY:
                    AssembledCity ac = _inventory.AssembledCities.FirstOrDefault(ac => ac.tilePis.Any(tpi => tpi.tile == t && tpi.groupIndexId == i));
                    bool claims = ac.tilePis.Any(tpi => tpi.tile.OtherPlayerHasTerraformerInGroup(CurrentTurnPlayer.slot, tpi.groupIndexId));
                    if (claims) {
                        pss.HelpedOppClaimedPOITypes[EdgeType.CITY]++;
                    }
                    continue;
            }
        }
    }

    public void ReportPlayersScoredPOIType(List<PlayerSlot> pss, EdgeType et) {
        foreach (PlayerSlot ps in pss) {
            Stats[ps].ScoredPOITypes[et]++;
        }
    }

    void _GiveRecruitMissionsToPlayer(PlayerSlot ps) {
        List<SecretObjective> recruitMissions = new List<SecretObjective> {
            new SO_RECRUIT_Road(),
            new SO_RECRUIT_City(),
            new SO_RECRUIT_Obelisk(),
            new SO_RECRUIT_HelpOppoRoad()
            // new SO_RECRUIT_Farm(),
            // new SO_T2_TFCollected()
            // new SO_T2_AnyComplete(),
            // new SO_T2_PointsScoredTurn(),
            // new SO_T2_RoadCitySize(),
            // new SO_T2_CityWithShield()
        };
        recruitMissions.ForEach(so => so.ImprintForPlayer(ps, _inventory, this));
        Stats[ps].Objectives.AddRange(recruitMissions);
    }

    public List<SecretObjective> GetAllPlayerObjectives() {
        return Stats.Values.SelectMany(pss => pss.Objectives).ToList();
    }

    public void CommitScoringEvent(ScoringEvent se) {
        foreach(PlayerSlot ps in se.NetScoreChangeByPlayer.Keys) {
            AddScoreForPlayerSlot(ps, se.NetScoreChangeByPlayer[ps], se.EventType);
        }
        foreach(GamepieceTileAssignment gta in se.RelatedGamepieces) {
            CollectGamepiece(gta.Type, gta.Team);
        }
    }

    public void CollectGamepiece(GamepieceType type, PlayerSlot slot) {
        if (type == GamepieceType.TERRAFORMER) {
            Stats[slot].TerraformerCount++;
            Stats[slot].TerraformersCollected++;
            TerraformersRecoveredThisTurn++;
        }
    }

    public void RemoveGamepieceFromStash(GamepieceType type, PlayerSlot slot) {
        if (type == GamepieceType.TERRAFORMER) {
            Stats[slot].TerraformerCount--;
        }
    }

    public PlayerAssignment GetCurrentTurnPlayer() {
        return CurrentTurnPlayer;
    }

    public int GetScoreForPlayerSlot(PlayerSlot ps) {
        return Stats[ps].Score;
    }
    
    public void AddScoreForPlayerSlot(PlayerSlot ps, int scoreIncrease, ScoringEventType sourceType) {
        Stats[ps].Score += scoreIncrease;
        Stats[ps].ScoreByEventType[sourceType] += scoreIncrease;
    }

    public PlayerAssignment GetNextTurnPlayer() {
        List<PlayerSlot> players = Stats.Keys
            .OrderBy(k => k)
            .ToList();
        int currentIndex = players.IndexOf(CurrentTurnPlayer.slot);
        int nextIndex = (currentIndex + 1) % players.Count;
        return new PlayerAssignment{
            slot = players[nextIndex],
            type = Stats[players[nextIndex]].PlayerType
        };
    }
    // would be internal
    public void AdvanceToNextTurn() {
        CurrentTurnPlayer = GetNextTurnPlayer();
        foreach(PlayerSlot ps in Stats.Keys) {
            Stats[ps].ScoreAtLastTurnStart = Stats[ps].Score;
            
        }
        TerraformersRecoveredThisTurn = 0;
    }
}
