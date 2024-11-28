using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public abstract class SecretObjective {
    public SecretObjectiveRank Rank { get; set; }
    public int Tier { get; set; }
    public string ObjectiveName { get; set; }
    public string ObjectiveOrders { get; set; }
    public string SuccessText { get; set; }
    public string SpritePath { get; set; }
    // REF INFO FROM IMPRINT
    protected BoardInventory _boardInventory;
    protected PlayerSlot _player;
    protected Scoreboard _scoreboard;

    public void ImprintForPlayer(PlayerSlot player, BoardInventory boardInventory, Scoreboard scoreboard) {
        _player = player;
        _boardInventory = boardInventory;
        _scoreboard = scoreboard;
        GatherImprintOnInit();
    }

    internal abstract void GatherImprintOnInit();
    public abstract ScoringEvent GetScoringEvent();
}

public enum SecretObjectiveRank {
    RECRUIT,
    DIRTLING,
    LANDSHAPER,
    STARSHAPER
}

public class SO_RECRUIT_Road : SecretObjective {
    public SO_RECRUIT_Road() {
        SpritePath = "Images/RoadBlue";
        ObjectiveName = "Claim and Complete a Road";
        ObjectiveOrders = "Place a Tot on a road of any length and complete it";
        SuccessText = "Your work has been extraordinary! You have successfully claimed a road! Keep up the good work!";
        Rank = SecretObjectiveRank.RECRUIT;
        Tier = 0;
    }

    int ROADS_NEEDED = 1;
    int COMPLETE_FORPLAYER_AT_COUNT = 0;

    public override ScoringEvent GetScoringEvent() {
        if (_getClaimedRoadsCount() < COMPLETE_FORPLAYER_AT_COUNT) {
            return null;
        }
        var scoreEarned = new Dictionary<PlayerSlot, int> {
                {_player, 3}
            };
        return new ScoringEvent(
            () => {
                _scoreboard.CommitScoringEvent(new ScoringEvent(
                    () => {},
                    new List<Tile>(),
                    new List<GamepieceTileAssignment>(),
                    ScoringEventType.SECRET_OBJECTIVE,
                    scoreEarned,
                    "Objective Complete!"
                ));
                _scoreboard.Stats[_player].Objectives.Remove(this);
                _scoreboard.Stats[_player].ObjectivesCompleted++;
                _scoreboard.Stats[_player].RookieObjectiveCompleted++;
            },
            new List<Tile>(),
            new List<GamepieceTileAssignment>(),
            ScoringEventType.SECRET_OBJECTIVE,
            scoreEarned,
            "Secret Objective Completed: " + ObjectiveName
        );
    }

    int _getClaimedRoadsCount() {
        return _scoreboard.Stats[_player].ClaimedPOITypes[EdgeType.ROAD];
    }

    internal override void GatherImprintOnInit() {
        COMPLETE_FORPLAYER_AT_COUNT = ROADS_NEEDED + _getClaimedRoadsCount();
    }
}

public class SO_RECRUIT_City : SecretObjective {
    public SO_RECRUIT_City() {
        SpritePath = "Images/CityBlue";
        ObjectiveName = "Claim and Complete a City";
        ObjectiveOrders = "Place a Tot on a city of any size and complete it";
        SuccessText = "Your work has been extraordinary! You have successfully claimed a city! Keep up the good work!";
        Rank = SecretObjectiveRank.RECRUIT;
        Tier = 0;
    }

    int CITIES_NEEDED = 1;
    int COMPLETE_FORPLAYER_AT_COUNT = 0;

    public override ScoringEvent GetScoringEvent() {
        if (_getClaimedCitiesCount() < COMPLETE_FORPLAYER_AT_COUNT) {
            return null;
        }
        var scoreEarned = new Dictionary<PlayerSlot, int> {
            {_player, 3}
        };
        return new ScoringEvent(
            () => {
                _scoreboard.CommitScoringEvent(new ScoringEvent(
                    () => {},
                    new List<Tile>(),
                    new List<GamepieceTileAssignment>(),
                    ScoringEventType.SECRET_OBJECTIVE,
                    scoreEarned,
                    "Objective Complete!"
                ));
                _scoreboard.Stats[_player].Objectives.Remove(this);
                _scoreboard.Stats[_player].ObjectivesCompleted++;
                _scoreboard.Stats[_player].RookieObjectiveCompleted++;
            },
            new List<Tile>(),
            new List<GamepieceTileAssignment>(),
            ScoringEventType.SECRET_OBJECTIVE,
            scoreEarned,
            "Secret Objective Completed: " + ObjectiveName
        );
    }

    int _getClaimedCitiesCount() {
        return _scoreboard.Stats[_player].ClaimedPOITypes[EdgeType.CITY];
    }

    internal override void GatherImprintOnInit() {
        COMPLETE_FORPLAYER_AT_COUNT = CITIES_NEEDED + _getClaimedCitiesCount();
    }
}

public class SO_RECRUIT_Farm : SecretObjective {
    public SO_RECRUIT_Farm() {
        SpritePath = "Images/LandBlue";
        ObjectiveName = "Claim a Field Touching a City";
        ObjectiveOrders = "Place a Tot on a field of any size touching a city";
        SuccessText = "Your work has been extraordinary! You have establish a presence on a farm! Keep up the good work!";
        Rank = SecretObjectiveRank.RECRUIT;
        Tier = 0;
    }

    int FARMS_NEEDED = 1;
    int COMPLETE_FORPLAYER_AT_COUNT = 0;

    public override ScoringEvent GetScoringEvent() {
        if (_getPlacedFarmCount() < COMPLETE_FORPLAYER_AT_COUNT) {
            return null;
        }
        var scoreEarned = new Dictionary<PlayerSlot, int> {
            {_player, 3}
        };
        return new ScoringEvent(
            () => {
                _scoreboard.CommitScoringEvent(new ScoringEvent(
                    () => {},
                    new List<Tile>(),
                    new List<GamepieceTileAssignment>(),
                    ScoringEventType.SECRET_OBJECTIVE,
                    scoreEarned,
                    "Objective Complete!"
                ));
                _scoreboard.Stats[_player].Objectives.Remove(this);
                _scoreboard.Stats[_player].ObjectivesCompleted++;
                _scoreboard.Stats[_player].RookieObjectiveCompleted++;
            },
            new List<Tile>(),
            new List<GamepieceTileAssignment>(),
            ScoringEventType.SECRET_OBJECTIVE,
            scoreEarned,
            "Secret Objective Completed: " + ObjectiveName
        );
    }

    int _getPlacedFarmCount() {
        return _boardInventory.OwnedFarms
            .Where(f => f.IsTouchingACity() && f.tilePis.Any(tpi => tpi.tile.PlayerHasTerraformerInGroup(_player, tpi.groupIndexId)))
            .Count();
    }

    internal override void GatherImprintOnInit() {
        COMPLETE_FORPLAYER_AT_COUNT = FARMS_NEEDED + _getPlacedFarmCount();
    }
}

public class SO_RECRUIT_Obelisk : SecretObjective {
    public SO_RECRUIT_Obelisk() {
        SpritePath = "Images/ObeliskBlue";
        ObjectiveName = "Claim an Obelisk";
        ObjectiveOrders = "Place a Tot on an obelisk";
        SuccessText = "Your work has been extraordinary! You have successfully claimed an obelisk! Keep up the good work!";
        Rank = SecretObjectiveRank.RECRUIT;
        Tier = 0;
    }

    int OBELISKS_NEEDED = 1;
    int COMPLETE_FORPLAYER_AT_COUNT = 0;

    public override ScoringEvent GetScoringEvent() {
        if (_getCompletedObeliskCount() < COMPLETE_FORPLAYER_AT_COUNT) {
            return null;
        }
        var scoreEarned = new Dictionary<PlayerSlot, int> {
            {_player, 3}
        };
        return new ScoringEvent(
            () => {
                _scoreboard.CommitScoringEvent(new ScoringEvent(
                    () => {},
                    new List<Tile>(),
                    new List<GamepieceTileAssignment>(),
                    ScoringEventType.SECRET_OBJECTIVE,
                    scoreEarned,
                    "Objective Complete!"
                ));
                _scoreboard.Stats[_player].Objectives.Remove(this);
                _scoreboard.Stats[_player].ObjectivesCompleted++;
                _scoreboard.Stats[_player].RookieObjectiveCompleted++;
            },
            new List<Tile>(),
            new List<GamepieceTileAssignment>(),
            ScoringEventType.SECRET_OBJECTIVE,
            scoreEarned,
            "Secret Objective Completed: " + ObjectiveName
        );
    }

    int _getCompletedObeliskCount() {
        return _scoreboard.Stats[_player].ClaimedPOITypes[EdgeType.OBELISK];
    }

    internal override void GatherImprintOnInit() {
        COMPLETE_FORPLAYER_AT_COUNT = OBELISKS_NEEDED + _getCompletedObeliskCount();
    }
}

public class SO_T1_NoTots : SecretObjective {
    public SO_T1_NoTots() {
        SpritePath = "Images/TerraformerPinkGreyed";
        ObjectiveName = "Place 3 Tiles in a row W/O Claiming";
        ObjectiveOrders = "Place 3 tiles without claiming any points of interest";
        SuccessText = "Your work has been extraordinary! Keep the bad guys guessing!";
        Rank = SecretObjectiveRank.DIRTLING;
        Tier = 0;
    }

    int STREAK_LENGTH = 3;

    public override ScoringEvent GetScoringEvent() {
        if (_getStreakCount() < STREAK_LENGTH) {
            return null;
        }
        var scoreEarned = new Dictionary<PlayerSlot, int> {
            {_player, 6}
        };
        return new ScoringEvent(
            () => {
                _scoreboard.CommitScoringEvent(new ScoringEvent(
                    () => {},
                    new List<Tile>(),
                    new List<GamepieceTileAssignment>(),
                    ScoringEventType.SECRET_OBJECTIVE,
                    scoreEarned,
                    "Objective Complete!"
                ));
                _scoreboard.Stats[_player].Objectives.Remove(this);
                _scoreboard.Stats[_player].ObjectivesCompleted++;
                _scoreboard.Stats[_player].DirtlingObjectiveCompleted++;
            },
            new List<Tile>(),
            new List<GamepieceTileAssignment>(),
            ScoringEventType.SECRET_OBJECTIVE,
            scoreEarned,
            "Secret Objective Completed: " + ObjectiveName
        );
    }

    int _getStreakCount() {
        UnityEngine.Debug.Log(_player + " : " + _scoreboard.Stats[_player].TurnsInARow_NOT_PlacingTerraformer);
        return _scoreboard.Stats[_player].TurnsInARow_NOT_PlacingTerraformer;
    }

    internal override void GatherImprintOnInit()
    {
    }
}

public class SO_T1_TotStreak : SecretObjective {
    public SO_T1_TotStreak() {
        SpritePath = "Images/TerraformerPink";
        ObjectiveName = "Place 3 Straight Tiles W/ a Terratot";
        ObjectiveOrders = "Place 3 tiles with Terratots in a row";
        SuccessText = "Your work has been extraordinary! Keep the bad guys guessing!";
        Rank = SecretObjectiveRank.DIRTLING;
        Tier = 0;
    }

    int STREAK_LENGTH = 3;

    public override ScoringEvent GetScoringEvent() {
        if (_getStreakCount() < STREAK_LENGTH) {
            return null;
        }
        var scoreEarned = new Dictionary<PlayerSlot, int> {
            {_player, 6}
        };
        return new ScoringEvent(
            () => {
                _scoreboard.CommitScoringEvent(new ScoringEvent(
                    () => {},
                    new List<Tile>(),
                    new List<GamepieceTileAssignment>(),
                    ScoringEventType.SECRET_OBJECTIVE,
                    scoreEarned,
                    "Objective Complete!"
                ));
                _scoreboard.Stats[_player].Objectives.Remove(this);
                _scoreboard.Stats[_player].ObjectivesCompleted++;
                _scoreboard.Stats[_player].DirtlingObjectiveCompleted++;
            },
            new List<Tile>(),
            new List<GamepieceTileAssignment>(),
            ScoringEventType.SECRET_OBJECTIVE,
            scoreEarned,
            "Secret Objective Completed: " + ObjectiveName
        );
    }

    int _getStreakCount() {
        return _scoreboard.Stats[_player].TurnsInARowPlacingTerraformer;
    }

    internal override void GatherImprintOnInit()
    {
    }
}

public class SO_T1_CitySize : SecretObjective {
    public SO_T1_CitySize() {
        SpritePath = "Images/CityBlue";
        ObjectiveName = "Complete a 3+ Tile City";
        ObjectiveOrders = "Complete a city of 3 or more tiles";
        SuccessText = "Your work has been extraordinary! Keep the bad guys guessing!";
        Rank = SecretObjectiveRank.DIRTLING;
        Tier = 0;
    }

    int MIN_SIZE = 3;
    int GOAL_COUNT = -1;

    public override ScoringEvent GetScoringEvent() {
        if (_getCountOfCitiesCollected() < GOAL_COUNT) {
            return null;
        }
        var scoreEarned = new Dictionary<PlayerSlot, int> {
            {_player, 6}
        };
        return new ScoringEvent(
            () => {
                _scoreboard.CommitScoringEvent(new ScoringEvent(
                    () => {},
                    new List<Tile>(),
                    new List<GamepieceTileAssignment>(),
                    ScoringEventType.SECRET_OBJECTIVE,
                    scoreEarned,
                    "Objective Complete!"
                ));
                _scoreboard.Stats[_player].Objectives.Remove(this);
                _scoreboard.Stats[_player].ObjectivesCompleted++;
                _scoreboard.Stats[_player].DirtlingObjectiveCompleted++;
            },
            new List<Tile>(),
            new List<GamepieceTileAssignment>(),
            ScoringEventType.SECRET_OBJECTIVE,
            scoreEarned,
            "Secret Objective Completed: " + ObjectiveName
        );
    }

    int _getCountOfCitiesCollected() {
        return _boardInventory.AssembledCities
            .Where(city => city.collectedBy == _player && city.GetUniqueTileCount() >= MIN_SIZE)
            .Count();
    }

    internal override void GatherImprintOnInit()
    {
        GOAL_COUNT = _getCountOfCitiesCollected() + 1;
    }
}


public class SO_T1_RoadSize : SecretObjective {
    public SO_T1_RoadSize() {
        SpritePath = "Images/RoadBlue";
        ObjectiveName = "Complete a 3+ Tile Road";
        ObjectiveOrders = "Complete a road of 3 or more tiles";
        SuccessText = "Your work has been extraordinary! Keep the bad guys guessing!";
        Rank = SecretObjectiveRank.DIRTLING;
        Tier = 0;
    }

    int MIN_SIZE = 3;
    int GOAL_COUNT = -1;

    public override ScoringEvent GetScoringEvent() {
        if (_getCountOfRoadsCollected() < GOAL_COUNT) {
            return null;
        }
        var scoreEarned = new Dictionary<PlayerSlot, int> {
            {_player, 6}
        };
        return new ScoringEvent(
            () => {
                _scoreboard.CommitScoringEvent(new ScoringEvent(
                    () => {},
                    new List<Tile>(),
                    new List<GamepieceTileAssignment>(),
                    ScoringEventType.SECRET_OBJECTIVE,
                    scoreEarned,
                    "Objective Complete!"
                ));
                _scoreboard.Stats[_player].Objectives.Remove(this);
                _scoreboard.Stats[_player].ObjectivesCompleted++;
                _scoreboard.Stats[_player].DirtlingObjectiveCompleted++;
            },
            new List<Tile>(),
            new List<GamepieceTileAssignment>(),
            ScoringEventType.SECRET_OBJECTIVE,
            scoreEarned,
            "Secret Objective Completed: " + ObjectiveName
        );
    }

    int _getCountOfRoadsCollected() {
        return _boardInventory.AssembledRoads
            .Where(r => r.collectedBy == _player && r.GetUniqueTileCount() >= MIN_SIZE)
            .Count();
    }

    internal override void GatherImprintOnInit()
    {
        GOAL_COUNT = _getCountOfRoadsCollected() + 1;
    }
}

public class SO_T1_HelpOppoRoad : SecretObjective {
    public SO_T1_HelpOppoRoad() {
        SpritePath = "Images/RoadBlue";
        ObjectiveName = "Add to an Opponent's Road";
        ObjectiveOrders = "Add a tile to an opponent's road";
        SuccessText = "Your work has been extraordinary! Keep the bad guys guessing!";
        Rank = SecretObjectiveRank.DIRTLING;
        Tier = 0;
    }

    int ROADS_NEEDED = 1;
    int COMPLETE_FORPLAYER_AT_COUNT = -1;

    public override ScoringEvent GetScoringEvent() {
        if (_getCountOfRoadsHelped() < COMPLETE_FORPLAYER_AT_COUNT) {
            return null;
        }
        var scoreEarned = new Dictionary<PlayerSlot, int> {
            {_player, 6}
        };
        return new ScoringEvent(
            () => {
                _scoreboard.CommitScoringEvent(new ScoringEvent(
                    () => {},
                    new List<Tile>(),
                    new List<GamepieceTileAssignment>(),
                    ScoringEventType.SECRET_OBJECTIVE,
                    scoreEarned,
                    "Objective Complete!"
                ));
                _scoreboard.Stats[_player].Objectives.Remove(this);
                _scoreboard.Stats[_player].ObjectivesCompleted++;
                _scoreboard.Stats[_player].DirtlingObjectiveCompleted++;
            },
            new List<Tile>(),
            new List<GamepieceTileAssignment>(),
            ScoringEventType.SECRET_OBJECTIVE,
            scoreEarned,
            "Secret Objective Completed: " + ObjectiveName
        );
    }

    int _getCountOfRoadsHelped() {
        return _scoreboard.Stats[_player].HelpedOppClaimedPOITypes[EdgeType.ROAD];
    }

    internal override void GatherImprintOnInit()
    {
        COMPLETE_FORPLAYER_AT_COUNT = _getCountOfRoadsHelped() + 1;
    }
}

public class SO_T1_PointsScoredTurn : SecretObjective {
    public SO_T1_PointsScoredTurn() {
        SpritePath = "Images/RoadBlue";
        ObjectiveName = "Score 6 Points in a Turn";
        ObjectiveOrders = "Score 6 points in a turn";
        SuccessText = "Your work has been extraordinary! Keep the bad guys guessing!";
        Rank = SecretObjectiveRank.DIRTLING;
        Tier = 0;
    }

    public override ScoringEvent GetScoringEvent() {
        bool isComplete = _scoreboard.Stats[_player].Score - _scoreboard.Stats[_player].ScoreAtLastTurnStart >= 6;
        if (!isComplete) {
            return null;
        }

        var scoreEarned = new Dictionary<PlayerSlot, int> {
            {_player, 6}
        };
        return new ScoringEvent(
            () => {
                _scoreboard.CommitScoringEvent(new ScoringEvent(
                    () => {},
                    new List<Tile>(),
                    new List<GamepieceTileAssignment>(),
                    ScoringEventType.SECRET_OBJECTIVE,
                    scoreEarned,
                    "Objective Complete!"
                ));
                _scoreboard.Stats[_player].Objectives.Remove(this);
                _scoreboard.Stats[_player].ObjectivesCompleted++;
                _scoreboard.Stats[_player].DirtlingObjectiveCompleted++;
            },
            new List<Tile>(),
            new List<GamepieceTileAssignment>(),
            ScoringEventType.SECRET_OBJECTIVE,
            scoreEarned,
            "Secret Objective Completed: " + ObjectiveName
        );
    }

    internal override void GatherImprintOnInit()
    {

    }
}

public class SO_T1_AnyComplete : SecretObjective {
    public SO_T1_AnyComplete() {
        SpritePath = "Images/GroundBlue";
        ObjectiveName = "Complete any 3 Roads/Cities/Obelisks";
        ObjectiveOrders = "Place the final tile to complete 3 in any combination: Roads, Cities, or Obelisks";
        SuccessText = "Your work has been extraordinary! Keep the bad guys guessing!";
        Rank = SecretObjectiveRank.DIRTLING;
        Tier = 0;
    }

    int ObjectiveStartAmount = -1;

    public override ScoringEvent GetScoringEvent() {
        bool isComplete = _scoreboard.Stats[_player].ObjectivesCompleted - ObjectiveStartAmount >= 3;
        if (!isComplete) {
            return null;
        }

        var scoreEarned = new Dictionary<PlayerSlot, int> {
            {_player, 6}
        };
        return new ScoringEvent(
            () => {
                _scoreboard.CommitScoringEvent(new ScoringEvent(
                    () => {},
                    new List<Tile>(),
                    new List<GamepieceTileAssignment>(),
                    ScoringEventType.SECRET_OBJECTIVE,
                    scoreEarned,
                    "Objective Complete!"
                ));
                _scoreboard.Stats[_player].Objectives.Remove(this);
                _scoreboard.Stats[_player].ObjectivesCompleted++;
                _scoreboard.Stats[_player].DirtlingObjectiveCompleted++;
            },
            new List<Tile>(),
            new List<GamepieceTileAssignment>(),
            ScoringEventType.SECRET_OBJECTIVE,
            scoreEarned,
            "Secret Objective Completed: " + ObjectiveName
        );
    }

    internal override void GatherImprintOnInit()
    {
        ObjectiveStartAmount = _scoreboard.Stats[_player].ObjectivesCompleted;
    }
}
