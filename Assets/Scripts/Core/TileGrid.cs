using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class TileGrid {
    public static readonly int GRID_LENGTH = 15;
    public static readonly int MIDPOINT = 7;
    public Tile[,] grid;

    public TileGrid(Tile startingTile)
    {
        grid = new Tile[15, 15];
        grid[MIDPOINT, MIDPOINT] = startingTile;
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
            return false;
        }

        grid[pos.x, pos.y] = tileToPlace;
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

    public List<GridPosition> GetEligiblePositions(Tile tile)
    {
        List<GridPosition> eligiblePositions = new List<GridPosition>();
        List<GridPosition> nearPlacedTiles = GetPositionsNearPlacedTiles();
        foreach (GridPosition position in nearPlacedTiles)
        {
            TileSurvey survey = SurveyPosition(position.x, position.y);
            if ((survey.NORTH == null || tile.FitsWithOtherToThe(
                survey.NORTH,
                CardinalDirection.NORTH))
                &&
                (survey.EAST == null || tile.FitsWithOtherToThe(survey.EAST, CardinalDirection.EAST))
                &&
                (survey.SOUTH == null || tile.FitsWithOtherToThe(survey.SOUTH, CardinalDirection.SOUTH))
                &&
                (survey.WEST == null || tile.FitsWithOtherToThe(survey.WEST, CardinalDirection.WEST))
            )
            {
                eligiblePositions.Add(position);
            }
        }
        return eligiblePositions;
    }
}
