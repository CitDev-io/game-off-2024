using System;
using System.Collections.Generic;
using System.Linq;

public delegate void PlayerAssignmentDelegate(PlayerAssignment p);

public class CoreCartridge {
    public Tile[,] grid;
    public BoardInventory inventory = new BoardInventory();
    TileBag TheBag = new TileBag();
    public Scoreboard scoreboard;
    public GameSettings gs = new GameSettings();
    public PlayerAssignmentDelegate OnPlayerTurnChange;
    bool TilePlacedThisTurn = false;
    public Tile CurrentTile;

    public CoreCartridge(Tile startingTile, GameSettings gs)
    {
        // can add tilebag to this
        grid = new Tile[gs.OddGameBoardWidth, gs.OddGameBoardWidth];


        PlaceTile(startingTile, new GridPosition(gs.BoardMidPoint, gs.BoardMidPoint));
        scoreboard = new Scoreboard(gs.playerManifest);

        // set the table
        CurrentTile = DrawNewTile();
        TilePlacedThisTurn = false;

        // hook in the AI spy
        OnPlayerTurnChange += OnTurnStart_AISpy;
    }

    void OnTurnStart_AISpy(PlayerAssignment pa) {
        if (pa.type != PlayerType.CPU) {
            return;
        }

        // its the CPU's turn. lock things up from player input, do the logic and wait for ack
    }

    // player calls this: acknowledges turn details and we move on.
    public void Ack() {
        if (!TilePlacedThisTurn && scoreboard.GetCurrentTurnPlayer().type == PlayerType.HUMAN) {
            // do need this enforced on CPU turn when it has logic
            throw new Exception("Player must place a tile before ending turn");
        }

        NextTurn();
    }

    void NextTurn() {
        TilePlacedThisTurn = false;
        scoreboard.AdvanceToNextTurn();
        CurrentTile = DrawNewTile();
        OnPlayerTurnChange?.Invoke(scoreboard.GetCurrentTurnPlayer());
    }

/* SCORING STUFF */
    public List<ScoringEvent> GetScoringEvents() {
        List<ScoringEvent> Events = new List<ScoringEvent>();

        Events.AddRange(SCORE_CompletedRoads);
        Events.AddRange(SCORE_CompletedObelisks);
        Events.AddRange(SCORE_CompletedCities);

        return Events;
    }

    public List<PlayerSlot> GetPlayersWithMostTerraformers(
        List<GamepieceTileAssignment> GTAs
    ) {
        var mostCommonTypes = GTAs
            .Where(obj => obj.Type == GamepieceType.TERRAFORMER)
            .GroupBy(obj => obj.Team)
            .Select(group => new { Team = group.Key, Count = group.Count() })
            .OrderByDescending(x => x.Count)
            .ToList();

        int maxCount = mostCommonTypes.FirstOrDefault()?.Count ?? 0;

        // Filter for types with the maximum count
        var result = mostCommonTypes
            .Where(x => x.Count == maxCount)
            .Select(x => x.Team)
            .ToList();

        return result;
    }

    Predicate<AssembledRoad> ARFilter_CompleteUncollected = (ar) => ar.IsCompleteAndUncollected();

    Predicate<AssembledCity> ACFilter_CompleteUncollected = (ac) => ac.IsCompleteAndUncollected();

    ScoringEvent ACConvert_CompletedCityToScoringEvent(AssembledCity ac) {
        List<Tile> RelatedTiles = ac.tilePis
            .Select(tpi => tpi.tile)
            .ToList();

        
        List<GamepieceTileAssignment> RelatedGamepieces = ac.tilePis
            .SelectMany(tpi => tpi.tile.GetAllGamepiecesInGroupId(tpi.groupIndexId))
            .ToList();

        int shieldBonus = 0; // TODO: !

        Dictionary<PlayerSlot, int> NetScoreChangeByPlayer = new();
        
        List<PlayerSlot> PlayersWithMostTerraformers = GetPlayersWithMostTerraformers(RelatedGamepieces);

        int ScoreEarned = ac.GetUniqueTileCount() * 2;
        string Earners = string.Join(
            " and ",
            PlayersWithMostTerraformers.Select(s => s.ToString())
        );

        string Description = $"City Complete: {Earners} earned {ScoreEarned} points!";
        if (PlayersWithMostTerraformers.Count == 0) {
            Description = "City Complete!";
        }
        foreach(PlayerSlot ps in PlayersWithMostTerraformers) {
            NetScoreChangeByPlayer.Add(ps, ScoreEarned);
        }
        Action ScoringAction = () => { 
            // do NOT reference local variables if you can help it. trying read-only jic
            foreach(PlayerSlot ps in PlayersWithMostTerraformers) {
                // WILO
                // give them some pointses!!
            }
            // return terraformers
            // update player inventories for tf's
            ac.MarkAsCollected();
        };
        
        return new ScoringEvent(
            ScoringAction,
            RelatedTiles,
            RelatedGamepieces,
            ScoringEventType.CITYCOMPLETED,
            NetScoreChangeByPlayer,
            Description
        );
    }

    ScoringEvent AOConvert_CompletedObeliskToScoringEvent(AssembledObelisk ao) {
        List<Tile> RelatedTiles = new List<Tile>{
            ao.tilePi.tile,
            ao.tilePi.tile.NormalizedSurvey.NORTH,
            ao.tilePi.tile.NormalizedSurvey.SOUTH,
            ao.tilePi.tile.NormalizedSurvey.EAST,
            ao.tilePi.tile.NormalizedSurvey.NORTHWEST,
            ao.tilePi.tile.NormalizedSurvey.NORTHEAST,
            ao.tilePi.tile.NormalizedSurvey.SOUTHWEST,
            ao.tilePi.tile.NormalizedSurvey.SOUTHEAST,
            ao.tilePi.tile.NormalizedSurvey.WEST
        };

        List<GamepieceTileAssignment> RelatedGamepieces = ao
            .tilePi
            .tile
            .GamepieceAssignments;


        Dictionary<PlayerSlot, int> NetScoreChangeByPlayer = new();
        
        List<PlayerSlot> PlayersWithMostTerraformers = GetPlayersWithMostTerraformers(RelatedGamepieces);

        int ScoreEarned = 8;
        string Earners = string.Join(
            " and ",
            PlayersWithMostTerraformers.Select(s => s.ToString())
        );

        string Description = $"Obelisk Complete: {Earners} earned {ScoreEarned} points!";
        if (PlayersWithMostTerraformers.Count == 0) {
            Description = "Obelisk Complete!";
        }
        foreach(PlayerSlot ps in PlayersWithMostTerraformers) {
            NetScoreChangeByPlayer.Add(ps, ScoreEarned);
        }
        Action ScoringAction = () => { 
            // do NOT reference local variables if you can help it. trying read-only jic
            foreach(PlayerSlot ps in PlayersWithMostTerraformers) {
                // WILO
                // give them some pointses!!
            }
            // return terraformers
            // update player inventories for tf's
            ao.MarkAsCollected();
        };
        
        return new ScoringEvent(
            ScoringAction,
            RelatedTiles,
            RelatedGamepieces,
            ScoringEventType.OBELISKCOMPLETED,
            NetScoreChangeByPlayer,
            Description
        );
    }

    ScoringEvent ARConvert_CompletedRoadToScoringEvent(AssembledRoad ar) {
        List<Tile> RelatedTiles = ar.tilePis
            .Select(tpi => tpi.tile)
            .ToList();

        List<GamepieceTileAssignment> RelatedGamepieces = ar.tilePis
            .SelectMany(tpi => tpi.tile.GamepieceAssignments)
            .ToList();

        Dictionary<PlayerSlot, int> NetScoreChangeByPlayer = new();
        
        List<PlayerSlot> PlayersWithMostTerraformers = GetPlayersWithMostTerraformers(RelatedGamepieces);

        int ScoreEarned = ar.GetUniqueTileCount() * 1;
        string Earners = string.Join(
            " and ",
            PlayersWithMostTerraformers.Select(s => s.ToString())
        );

        string Description = $"Road Complete: {Earners} earned {ScoreEarned} points!";
        if (PlayersWithMostTerraformers.Count == 0) {
            Description = "Road Complete!";
        }
        foreach(PlayerSlot ps in PlayersWithMostTerraformers) {
            NetScoreChangeByPlayer.Add(ps, ScoreEarned);
        }
        Action ScoringAction = () => { 
            // do NOT reference local variables if you can help it. trying read-only jic
            foreach(PlayerSlot ps in PlayersWithMostTerraformers) {
                // WILO
                // give them some pointses!!
            }
            // return terraformers
            // update player inventories for tf's
            ar.MarkAsCollected();
        };
        
        return new ScoringEvent(
            ScoringAction,
            RelatedTiles,
            RelatedGamepieces,
            ScoringEventType.ROADCOMPLETED,
            NetScoreChangeByPlayer,
            Description
        );
    }

    public List<ScoringEvent> SCORE_CompletedCities =>
        inventory.AssembledCities
            .FindAll(ACFilter_CompleteUncollected)
            .Select(ACConvert_CompletedCityToScoringEvent)
            .ToList();

    public List<ScoringEvent> SCORE_CompletedObelisks =>
        inventory.AssembledObelisks
            .FindAll(o => o.IsCompleteAndUncollected())
            .Select(AOConvert_CompletedObeliskToScoringEvent)
            .ToList();

    public List<ScoringEvent> SCORE_CompletedRoads =>
        inventory.AssembledRoads
            .FindAll(ARFilter_CompleteUncollected)
            .Select(ARConvert_CompletedRoadToScoringEvent).ToList();



/* END SCORING STUFF */
    public Tile DrawNewTile() {
        Tile tile = null;
        while (tile == null) {
            Tile checkTile = TheBag.DrawTile();
            var eligiblePositions = GetEligiblePositionsAllRotations(checkTile);
            if (eligiblePositions.Count > 0) {
                tile = checkTile;
            } else {
                TheBag.ReturnTile(checkTile);
            }
        }
        return tile;
    }

    public bool CanPlaceTile(Tile tileToPlace, GridPosition pos) {
        if (GetTileAtGridPosition(pos) != null) return false;

        TileSurvey survey = SurveyPosition(pos);
        if (survey.NORTH != null
            && !tileToPlace.FitsWithOtherToThe(survey.NORTH, CardinalDirection.NORTH)) {
            return false;
        }

        if (survey.SOUTH != null
            && !tileToPlace.FitsWithOtherToThe(survey.SOUTH, CardinalDirection.SOUTH)) {
            return false;
        }

        if (survey.EAST != null
            && !tileToPlace.FitsWithOtherToThe(survey.EAST, CardinalDirection.EAST)) {
            return false;
        }

        if (survey.WEST != null
            && !tileToPlace.FitsWithOtherToThe(survey.WEST, CardinalDirection.WEST)) {
            return false;
        }

        return true;
    }

    public bool PlaceTile(Tile tileToPlace, GridPosition pos) {
        if (!CanPlaceTile(tileToPlace, pos)) {
            throw new Exception("Invalid Tile Placement Attempted");
        }

        if (TilePlacedThisTurn) {
            throw new Exception("Only one tile can be placed per turn");
        }

        TilePlacedThisTurn = true;

        grid[pos.x, pos.y] = tileToPlace;

        // TODO: check if gp are ok
        bool gamepiecePlacementsAreValid = true;

        if (!gamepiecePlacementsAreValid) {
            grid[pos.x, pos.y] = null;
            return false;
        }

        TileSurvey tileSurvey = SurveyPosition(pos);
        tileToPlace.NormalizedSurvey = tileSurvey;
        tileSurvey.ApplyToOtherSurveys(tileToPlace);

        inventory.AddTileToInventory(tileToPlace);

        return true;
    }
    #nullable enable
    Tile? GetTileAtGridPosition(GridPosition pos) {
        return grid[pos.x, pos.y];
    }
    #nullable disable

    public List<Tile> GetPlacedTiles() {
        List<Tile> tiles = new List<Tile>();
        for (int x = 0; x < gs.OddGameBoardWidth; x++) {
            for (int y = 0; y < gs.OddGameBoardWidth; y++) {
                if (grid[x, y] != null) {
                    tiles.Add(grid[x, y]);
                }
            }
        }
        return tiles;
    }

    public TileSurvey SurveyPosition(GridPosition pos) {
        return SurveyPosition(pos.x, pos.y);
    }

    public TileSurvey SurveyPosition(int x, int y)
    {
        TileSurvey survey = new TileSurvey();
        bool westAvailable = x > 0;
        bool eastAvailable = x < gs.OddGameBoardWidth - 1;
        bool southAvailable = y > 0;
        bool northAvailable = y < gs.OddGameBoardWidth - 1;

        if (westAvailable) {
            survey.WEST = grid[x - 1, y];
        }
        if (eastAvailable) {
            survey.EAST = grid[x + 1, y];
        }
        if (southAvailable) {
            survey.SOUTH = grid[x, y - 1];
        }
        if (northAvailable) {
            survey.NORTH = grid[x, y + 1];
        }
        if (westAvailable && northAvailable) {
            survey.NORTHWEST = grid[x - 1, y + 1];
        }
        if (westAvailable && southAvailable) {
            survey.SOUTHWEST = grid[x - 1, y - 1];
        }
        if (eastAvailable && northAvailable) {
            survey.NORTHEAST = grid[x + 1, y + 1];
        }
        if (eastAvailable && southAvailable) {
            survey.SOUTHEAST = grid[x + 1, y - 1];
        }
        return survey;
    }

    public List<GridPosition> GetPositionsNearPlacedTiles() {
        HashSet<GridPosition> positions = new HashSet<GridPosition>();
        for (int x = 0; x < gs.OddGameBoardWidth; x++) {
            for (int y = 0; y < gs.OddGameBoardWidth; y++) {
                if (grid[x, y] != null) {
                    if (x > 0 && grid[x - 1, y] == null) {
                        positions.Add(new GridPosition(x - 1, y));
                    }
                    if (x < gs.OddGameBoardWidth - 1 && grid[x + 1, y] == null) {
                        positions.Add(new GridPosition(x + 1, y));
                    }
                    if (y > 0 && grid[x, y - 1] == null) {
                        positions.Add(new GridPosition(x, y - 1));
                    }
                    if (y < gs.OddGameBoardWidth - 1 && grid[x, y + 1] == null) {
                        positions.Add(new GridPosition(x, y + 1));
                    }
                }
            }
        }
        return positions.ToList();
    }

    public List<GridPosition> GetEligiblePositionsAllRotations(Tile tile)
    {
        List<GridPosition> eligiblePositions = new List<GridPosition>();
        List<GridPosition> nearPlacedTiles = GetPositionsNearPlacedTiles();
        for (var i=0; i<4; i++) {
            Tile checkTile = TileFactory.CreateTileWithRotations(
                (TileType) Enum.Parse(typeof(TileType), tile.Name),
                i
            );
            foreach (GridPosition position in nearPlacedTiles)
            {
                TileSurvey survey = SurveyPosition(position.x, position.y);
                if ((survey.NORTH == null || checkTile.FitsWithOtherToThe(
                    survey.NORTH,
                    CardinalDirection.NORTH))
                    &&
                    (survey.EAST == null || checkTile.FitsWithOtherToThe(survey.EAST, CardinalDirection.EAST))
                    &&
                    (survey.SOUTH == null || checkTile.FitsWithOtherToThe(survey.SOUTH, CardinalDirection.SOUTH))
                    &&
                    (survey.WEST == null || checkTile.FitsWithOtherToThe(survey.WEST, CardinalDirection.WEST))
                )
                {
                    eligiblePositions.Add(position);
                }
            }

        }
        return eligiblePositions;
    }
}
