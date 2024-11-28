using System.Collections.Generic;
using System.Linq;

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
    public abstract string GetStatusString();
}

public enum SecretObjectiveRank {
    RECRUIT,
    DIRTLING,
    LANDSCRAPER,
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
    int STARTING_AMOUNT = 0;

    public override ScoringEvent GetScoringEvent() {
        if (_getClaimedRoadsCount() < STARTING_AMOUNT + ROADS_NEEDED) {
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
        return _scoreboard.Stats[_player].ScoredPOITypes[EdgeType.ROAD];
    }

    internal override void GatherImprintOnInit() {
        STARTING_AMOUNT = _getClaimedRoadsCount();
    }

    public override string GetStatusString() {
        return "(" + (_getClaimedRoadsCount() - STARTING_AMOUNT) + "/" + (STARTING_AMOUNT + ROADS_NEEDED) + ")";
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
    int STARTINGAMOUNT = 0;

    public override ScoringEvent GetScoringEvent() {
        if (_getClaimedCitiesCount() < STARTINGAMOUNT + CITIES_NEEDED) {
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
        return _scoreboard.Stats[_player].ScoredPOITypes[EdgeType.CITY];
    }

    internal override void GatherImprintOnInit() {
        STARTINGAMOUNT = _getClaimedCitiesCount();
    }

    public override string GetStatusString() {
        return "(" + (_getClaimedCitiesCount() - STARTINGAMOUNT) + "/" + CITIES_NEEDED + ")";
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
    int STARTINGAMOUNT = 0;

    public override ScoringEvent GetScoringEvent() {
        if (_getPlacedFarmCount() < STARTINGAMOUNT + FARMS_NEEDED) {
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
        STARTINGAMOUNT = _getPlacedFarmCount();
    }

    public override string GetStatusString() {
        return "(" + (_getPlacedFarmCount() - STARTINGAMOUNT) + "/" + FARMS_NEEDED + ")";
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
    int STARTINGAMOUNT = 0;

    public override ScoringEvent GetScoringEvent() {
        if (_getClaimedObeliskCount() < STARTINGAMOUNT + OBELISKS_NEEDED) {
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

    int _getClaimedObeliskCount() {
        return _scoreboard.Stats[_player].TerraformersPlacedOnPOIType[EdgeType.OBELISK];
    }

    internal override void GatherImprintOnInit() {
        STARTINGAMOUNT = _getClaimedObeliskCount();
    }

    public override string GetStatusString() {
        return "(" + (_getClaimedObeliskCount() - STARTINGAMOUNT) + "/" + OBELISKS_NEEDED + ")";
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

    public override string GetStatusString() {
        return "(" + _getStreakCount() + "/" + STREAK_LENGTH + ")";
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

    public override string GetStatusString() {
        return "(" + _getStreakCount() + "/" + STREAK_LENGTH + ")";
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

    int CITIES_NEEDED = 1;
    int STARTINGAMOUNT = 0;

    public override ScoringEvent GetScoringEvent() {
        if (_getCountOfCitiesCollected() < CITIES_NEEDED + STARTINGAMOUNT) {
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
            .Where(city => city.collectedBy == _player && city.GetUniqueTileCount() >= 3)
            .Count();
    }

    internal override void GatherImprintOnInit()
    {
        STARTINGAMOUNT = _getCountOfCitiesCollected();
    }

    public override string GetStatusString() {
        return "(" + (_getCountOfCitiesCollected() - STARTINGAMOUNT) + "/" + CITIES_NEEDED + ")";
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

    public override string GetStatusString() {
        return "(" + _getCountOfRoadsCollected() + "/" + GOAL_COUNT + ")";
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
    int STARTINGAMOUNT = 0;

    public override ScoringEvent GetScoringEvent() {
        if (_getCountOfRoadsHelped() < STARTINGAMOUNT + ROADS_NEEDED) {
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
        STARTINGAMOUNT = _getCountOfRoadsHelped();
    }

    public override string GetStatusString() {
        return "(" + (_getCountOfRoadsHelped() - STARTINGAMOUNT) + "/" + ROADS_NEEDED + ")";
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

    public override string GetStatusString() {
        return "";
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

    int ObjectiveStartAmount = 0;

    public override ScoringEvent GetScoringEvent() {
        bool isComplete = _getCompletedRoadsCitiesObelisks() >= ObjectiveStartAmount + 3;
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
        ObjectiveStartAmount = _getCompletedRoadsCitiesObelisks();
    }

    public override string GetStatusString() {
        return "(" + (_getCompletedRoadsCitiesObelisks() - ObjectiveStartAmount) + "/3)";
    }

    int _getCompletedRoadsCitiesObelisks() {
        int roadsCollected = _boardInventory.AssembledRoads
            .Count(f => f.collectedBy == _player);
        int citiesCollected = _boardInventory.AssembledCities
            .Count(f => f.collectedBy == _player);
        int obelisksCollected = _boardInventory.AssembledObelisks
            .Count(f => f.collectedBy == _player);

        return roadsCollected + citiesCollected + obelisksCollected;
    }
}

public class SO_T1_SharePOI : SecretObjective {
    public SO_T1_SharePOI() {
        SpritePath = "Images/GroundBlue";
        ObjectiveName = "Share a Road or City with the Enemy";
        ObjectiveOrders = "Combine a claimed road or city with the enemy";
        SuccessText = "Your work has been extraordinary! Keep the bad guys guessing!";
        Rank = SecretObjectiveRank.DIRTLING;
        Tier = 0;
    }

    int ObjectiveStartAmount = -1;
    int TimesRequired = 1;

    public override ScoringEvent GetScoringEvent() {
        bool isComplete = _getCountOfRoadsCitiesShared() >= ObjectiveStartAmount + TimesRequired;
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

    int _getCountOfRoadsCitiesShared() {
        int sharedRoads = _boardInventory.AssembledRoads
            .Count(r => r.GetUniqueTerraformerOwners() > 1);
        int sharedCities = _boardInventory.AssembledCities
            .Count(c => c.GetUniqueTerraformerOwners() > 1);
        UnityEngine.Debug.Log("Shared Roads: " + sharedRoads + " Shared Cities: " + sharedCities);
        return sharedRoads + sharedCities;
    }

    internal override void GatherImprintOnInit()
    {
        ObjectiveStartAmount = _getCountOfRoadsCitiesShared();
    }

    public override string GetStatusString() {
        return "(" + (_getCountOfRoadsCitiesShared() - ObjectiveStartAmount) + "/" + TimesRequired + ")";
    }
}

public class SO_T2_AnyComplete : SecretObjective {
    public SO_T2_AnyComplete() {
        SpritePath = "Images/GroundBlue";
        ObjectiveName = "Complete any 4 Roads/Cities/Obelisks";
        ObjectiveOrders = "Place the final tile to complete 4 in any combination: Roads, Cities, or Obelisks";
        SuccessText = "Your work has been extraordinary! Keep the bad guys guessing!";
        Rank = SecretObjectiveRank.LANDSCRAPER;
        Tier = 0;
    }

    int ObjectiveStartAmount = -1;

    public override ScoringEvent GetScoringEvent() {
        bool isComplete = _getCompletedRoadsCitiesObelisks() - ObjectiveStartAmount >= 4;
        if (!isComplete) {
            return null;
        }

        var scoreEarned = new Dictionary<PlayerSlot, int> {
            {_player, 7}
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
                _scoreboard.Stats[_player].LandscraperObjectiveCompleted++;
            },
            new List<Tile>(),
            new List<GamepieceTileAssignment>(),
            ScoringEventType.SECRET_OBJECTIVE,
            scoreEarned,
            "Secret Objective Completed: " + ObjectiveName
        );
    }

    int _getCompletedRoadsCitiesObelisks() {
        int roadsCollected = _boardInventory.AssembledRoads
            .Count(f => f.collectedBy == _player);
        int citiesCollected = _boardInventory.AssembledCities
            .Count(f => f.collectedBy == _player);
        int obelisksCollected = _boardInventory.AssembledObelisks
            .Count(f => f.collectedBy == _player);

        return roadsCollected + citiesCollected + obelisksCollected;
    }

    internal override void GatherImprintOnInit()
    {
        ObjectiveStartAmount = _getCompletedRoadsCitiesObelisks();
    }

    public override string GetStatusString() {
        return "(" + (_getCompletedRoadsCitiesObelisks() - ObjectiveStartAmount) + "/4)";
    }
}

public class SO_T2_PointsScoredTurn : SecretObjective {
    public SO_T2_PointsScoredTurn() {
        SpritePath = "Images/RoadBlue";
        ObjectiveName = "Score 8 Points in a Turn";
        ObjectiveOrders = "Score 8 points in a turn";
        SuccessText = "Your work has been extraordinary! Keep the bad guys guessing!";
        Rank = SecretObjectiveRank.LANDSCRAPER;
        Tier = 0;
    }

    public override ScoringEvent GetScoringEvent() {
        bool isComplete = _scoreboard.Stats[_player].Score - _scoreboard.Stats[_player].ScoreAtLastTurnStart >= 8;
        if (!isComplete) {
            return null;
        }

        var scoreEarned = new Dictionary<PlayerSlot, int> {
            {_player, 7}
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
                _scoreboard.Stats[_player].LandscraperObjectiveCompleted++;
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

    public override string GetStatusString() {
        return "";
    }
}


public class SO_T2_RoadCitySize : SecretObjective {
    public SO_T2_RoadCitySize() {
        SpritePath = "Images/RoadBlue";
        ObjectiveName = "Complete a 4+ Tile Road or City";
        ObjectiveOrders = "Complete a 4+ Tile Road or City";
        SuccessText = "Your work has been extraordinary! Keep the bad guys guessing!";
        Rank = SecretObjectiveRank.LANDSCRAPER;
        Tier = 0;
    }

    int STARTINGAMOUNT = 0;
    int SIZE_THRESHOLD = 4;
    int HOW_MANY_TO_COLLECT = 1;

    public override ScoringEvent GetScoringEvent() {
        if (_getCountOfRoadsCitiesOfSizeOrLarger(SIZE_THRESHOLD) < STARTINGAMOUNT + HOW_MANY_TO_COLLECT) {
            return null;
        }
        var scoreEarned = new Dictionary<PlayerSlot, int> {
            {_player, 7}
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
                _scoreboard.Stats[_player].LandscraperObjectiveCompleted++;
            },
            new List<Tile>(),
            new List<GamepieceTileAssignment>(),
            ScoringEventType.SECRET_OBJECTIVE,
            scoreEarned,
            "Secret Objective Completed: " + ObjectiveName
        );
    }

    int _getCountOfRoadsCitiesOfSizeOrLarger(int size) {
        int roadsMeetingReqs = _boardInventory.AssembledRoads
            .Where(r => r.collectedBy == _player && r.GetUniqueTileCount() >= size)
            .Count();
        int citiesMeetingReqs = _boardInventory.AssembledCities
            .Where(c => c.collectedBy == _player && c.GetUniqueTileCount() >= size)
            .Count();

        return roadsMeetingReqs + citiesMeetingReqs;
    }

    internal override void GatherImprintOnInit()
    {
        STARTINGAMOUNT = _getCountOfRoadsCitiesOfSizeOrLarger(SIZE_THRESHOLD);
    }

    public override string GetStatusString() {
        return "(" + (_getCountOfRoadsCitiesOfSizeOrLarger(SIZE_THRESHOLD) - STARTINGAMOUNT) + "/" + HOW_MANY_TO_COLLECT + ")";
    }
}

public class SO_T2_CityWithShield : SecretObjective {
    public SO_T2_CityWithShield() {
        SpritePath = "Images/CityBlue";
        ObjectiveName = "Complete a City with a Shield";
        ObjectiveOrders = "Complete a City with a Shield";
        SuccessText = "Your work has been extraordinary! Keep the bad guys guessing!";
        Rank = SecretObjectiveRank.LANDSCRAPER;
        Tier = 0;
    }

    int HOW_MANY_TO_COLLECT = 1;
    int STARTINGAMOUNT = 0;

    public override ScoringEvent GetScoringEvent() {
        if (_getCountOfCitiesCollectedWithShield() < STARTINGAMOUNT + HOW_MANY_TO_COLLECT) {
            return null;
        }
        var scoreEarned = new Dictionary<PlayerSlot, int> {
            {_player, 7}
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
                _scoreboard.Stats[_player].LandscraperObjectiveCompleted++;
            },
            new List<Tile>(),
            new List<GamepieceTileAssignment>(),
            ScoringEventType.SECRET_OBJECTIVE,
            scoreEarned,
            "Secret Objective Completed: " + ObjectiveName
        );
    }

    int _getCountOfCitiesCollectedWithShield() {
        return _boardInventory.AssembledCities
            .Where(c => c.collectedBy == _player && c.tilePis.Any(tpi => tpi.tile.IsABonusTile))
            .Count();
    }

    internal override void GatherImprintOnInit()
    {
        STARTINGAMOUNT = _getCountOfCitiesCollectedWithShield();
    }

    public override string GetStatusString() {
        return "(" + (_getCountOfCitiesCollectedWithShield() - STARTINGAMOUNT) + "/" + HOW_MANY_TO_COLLECT + ")";
    }
}

public class SO_T3_ObeliskCapture : SecretObjective {
    public SO_T3_ObeliskCapture() {
        SpritePath = "Images/ObeliskBlue";
        ObjectiveName = "Claim and Complete an Obelisk";
        ObjectiveOrders = "Claim and Complete an Obelisk";
        SuccessText = "Your work has been extraordinary! Keep the bad guys guessing!";
        Rank = SecretObjectiveRank.STARSHAPER;
        Tier = 0;
    }

    int STARTINGAMOUNT = 0;
    int HOW_MANY_TO_COLLECT = 1;

    public override ScoringEvent GetScoringEvent() {
        if (_getCountofObelisksClaimedAndCaptured() < STARTINGAMOUNT + HOW_MANY_TO_COLLECT) {
            return null;
        }
        var scoreEarned = new Dictionary<PlayerSlot, int> {
            {_player, 8}
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
                _scoreboard.Stats[_player].StarShaperObjectiveCompleted++;
            },
            new List<Tile>(),
            new List<GamepieceTileAssignment>(),
            ScoringEventType.SECRET_OBJECTIVE,
            scoreEarned,
            "Secret Objective Completed: " + ObjectiveName
        );
    }

    int _getCountofObelisksClaimedAndCaptured() {
        return _boardInventory.AssembledObelisks
            .Where(o => o.tilePi.tile.GamepieceAssignments.Any(g => g.Team == _player) && o.IsComplete())
            .Count();
    }

    internal override void GatherImprintOnInit()
    {
        STARTINGAMOUNT = _getCountofObelisksClaimedAndCaptured();
    }

    public override string GetStatusString() {
        return "(" + (_getCountofObelisksClaimedAndCaptured() - STARTINGAMOUNT) + "/" + HOW_MANY_TO_COLLECT + ")";
    }
}


public class SO_T3_PointsScoredTurn : SecretObjective {
    public SO_T3_PointsScoredTurn() {
        SpritePath = "Images/RoadBlue";
        ObjectiveName = "Score 10+ Points in a Turn";
        ObjectiveOrders = "Score 10+ points in a turn";
        SuccessText = "Your work has been extraordinary! Keep the bad guys guessing!";
        Rank = SecretObjectiveRank.STARSHAPER;
        Tier = 0;
    }

    public override ScoringEvent GetScoringEvent() {
        bool isComplete = _scoreboard.Stats[_player].Score - _scoreboard.Stats[_player].ScoreAtLastTurnStart >= 10;
        if (!isComplete) {
            return null;
        }

        var scoreEarned = new Dictionary<PlayerSlot, int> {
            {_player, 8}
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
                _scoreboard.Stats[_player].StarShaperObjectiveCompleted++;
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

    public override string GetStatusString() {
        return "";
    }
}


public class SO_T3_RoadCitySize : SecretObjective {
    public SO_T3_RoadCitySize() {
        SpritePath = "Images/RoadBlue";
        ObjectiveName = "Complete a 5+ Tile Road or City";
        ObjectiveOrders = "Complete a 5+ Tile Road or City";
        SuccessText = "Your work has been extraordinary! Keep the bad guys guessing!";
        Rank = SecretObjectiveRank.STARSHAPER;
        Tier = 0;
    }

    int STARTINGAMOUNT = 0;
    int SIZE_THRESHOLD = 5;
    int HOW_MANY_TO_COLLECT = 1;

    public override ScoringEvent GetScoringEvent() {
        if (_getCountOfRoadsCitiesOfSizeOrLarger(SIZE_THRESHOLD) < STARTINGAMOUNT + HOW_MANY_TO_COLLECT) {
            return null;
        }
        var scoreEarned = new Dictionary<PlayerSlot, int> {
            {_player, 8}
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
                _scoreboard.Stats[_player].StarShaperObjectiveCompleted++;
            },
            new List<Tile>(),
            new List<GamepieceTileAssignment>(),
            ScoringEventType.SECRET_OBJECTIVE,
            scoreEarned,
            "Secret Objective Completed: " + ObjectiveName
        );
    }

    int _getCountOfRoadsCitiesOfSizeOrLarger(int size) {
        int roadsMeetingReqs = _boardInventory.AssembledRoads
            .Where(r => r.collectedBy == _player && r.GetUniqueTileCount() >= size)
            .Count();
        int citiesMeetingReqs = _boardInventory.AssembledCities
            .Where(c => c.collectedBy == _player && c.GetUniqueTileCount() >= size)
            .Count();

        return roadsMeetingReqs + citiesMeetingReqs;
    }

    internal override void GatherImprintOnInit()
    {
        STARTINGAMOUNT = _getCountOfRoadsCitiesOfSizeOrLarger(SIZE_THRESHOLD);
    }

    public override string GetStatusString() {
        return "(" + (_getCountOfRoadsCitiesOfSizeOrLarger(SIZE_THRESHOLD) - STARTINGAMOUNT) + "/" + HOW_MANY_TO_COLLECT + ")";
    }
}

public class SO_T3_ShareWin : SecretObjective {
    public SO_T3_ShareWin() {
        SpritePath = "Images/GroundBlue";
        ObjectiveName = "Complete and Win a Shared Road or City";
        ObjectiveOrders = "Complete and Win a Shared Road or City";
        SuccessText = "Your work has been extraordinary! Keep the bad guys guessing!";
        Rank = SecretObjectiveRank.STARSHAPER;
        Tier = 0;
    }

    int ObjectiveStartAmount = -1;
    int TimesRequired = 1;

    public override ScoringEvent GetScoringEvent() {
        bool isComplete = _getCountOfSharedPOIWins() >= ObjectiveStartAmount + TimesRequired;
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
                _scoreboard.Stats[_player].StarShaperObjectiveCompleted++;
            },
            new List<Tile>(),
            new List<GamepieceTileAssignment>(),
            ScoringEventType.SECRET_OBJECTIVE,
            scoreEarned,
            "Secret Objective Completed: " + ObjectiveName
        );
    }

    int _getCountOfSharedPOIWins() {
        int roadWins = _boardInventory.AssembledRoads
            .Where(
                ar => ar.IsShared()
                && ar.IsComplete()
                && ar.WinningTerraformerOwners().Count == 1
                && ar.WinningTerraformerOwners()[0] == _player)
            .Count();

        int cityWins = _boardInventory.AssembledCities
            .Where(
                ac => ac.IsShared()
                && ac.IsComplete()
                && ac.WinningTerraformerOwners().Count == 1
                && ac.WinningTerraformerOwners()[0] == _player)
            .Count();

        return roadWins + cityWins;
    }

    internal override void GatherImprintOnInit()
    {
        ObjectiveStartAmount = _getCountOfSharedPOIWins();
    }

    public override string GetStatusString() {
        return "(" + (_getCountOfSharedPOIWins() - ObjectiveStartAmount) + "/" + TimesRequired + ")";
    }
}