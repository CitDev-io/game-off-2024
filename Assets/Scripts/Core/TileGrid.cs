using System;
using System.Collections.Generic;
using System.Linq;

public class TileGrid {
    public static readonly int GRID_LENGTH = 15;
    public static readonly int MIDPOINT = 7;
    public Tile[,] grid;
    public BoardInventory inventory = new BoardInventory();

    public TileGrid(Tile startingTile)
    {
        grid = new Tile[15, 15];
        PlaceTile(startingTile, new GridPosition(MIDPOINT, MIDPOINT));
        // grid[MIDPOINT, MIDPOINT] = startingTile;
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
            // UnityEngine.Debug.Log("Cannot place tile at " + pos.x + ", " + pos.y + " for " + tileToPlace.Name);
            return false;
        }

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

        List<RoadJoint> a = inventory.GetRoadJoints(tileToPlace);
        inventory.MergeRoadJoints(a);

        // run scoring checks against inventory


        return true;
    }
    #nullable enable
    Tile? GetTileAtGridPosition(GridPosition pos) {
        return grid[pos.x, pos.y];
    }
    #nullable disable

    public List<Tile> GetPlacedTiles() {
        List<Tile> tiles = new List<Tile>();
        for (int x = 0; x < GRID_LENGTH; x++) {
            for (int y = 0; y < GRID_LENGTH; y++) {
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
        if (x > 0) {
            survey.WEST = grid[x - 1, y];
        }
        if (x < GRID_LENGTH - 1) {
            survey.EAST = grid[x + 1, y];
        }
        if (y > 0) {
            survey.SOUTH = grid[x, y - 1];
        }
        if (y < GRID_LENGTH - 1) {
            survey.NORTH = grid[x, y + 1];
        }
        return survey;
    }

    public List<GridPosition> GetPositionsNearPlacedTiles() {
        HashSet<GridPosition> positions = new HashSet<GridPosition>();
        for (int x = 0; x < GRID_LENGTH; x++) {
            for (int y = 0; y < GRID_LENGTH; y++) {
                if (grid[x, y] != null) {
                    if (x > 0 && grid[x - 1, y] == null) {
                        positions.Add(new GridPosition(x - 1, y));
                    }
                    if (x < GRID_LENGTH - 1 && grid[x + 1, y] == null) {
                        positions.Add(new GridPosition(x + 1, y));
                    }
                    if (y > 0 && grid[x, y - 1] == null) {
                        positions.Add(new GridPosition(x, y - 1));
                    }
                    if (y < GRID_LENGTH - 1 && grid[x, y + 1] == null) {
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
