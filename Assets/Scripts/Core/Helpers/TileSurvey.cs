#nullable enable
public class TileSurvey {
    public Tile? NORTH;
    public Tile? EAST;
    public Tile? SOUTH;
    public Tile? WEST;

    // non-cardinal enum alternates
    public Tile? NORTHWEST;
    public Tile? NORTHEAST;
    public Tile? SOUTHWEST;
    public Tile? SOUTHEAST;



    public Tile? TileInDirection(CardinalDirection dir) {
        return dir switch {
            CardinalDirection.NORTH => NORTH,
            CardinalDirection.EAST => EAST,
            CardinalDirection.SOUTH => SOUTH,
            CardinalDirection.WEST => WEST,
            _ => null
        };
    }

    public int ExtendedNeighborhoodCount() {
        int neighbors = 0;
        if (NORTH != null) neighbors++;
        if (SOUTH != null) neighbors++;
        if (EAST != null) neighbors++;
        if (WEST != null) neighbors++;
        if (NORTHEAST != null) neighbors++;
        if (NORTHWEST != null) neighbors++;
        if (SOUTHEAST != null) neighbors++;
        if (SOUTHWEST != null) neighbors++;

        return neighbors;
    }

    public void ApplyToOtherSurveys(Tile t) {
        if (NORTH != null) {
            NORTH.NormalizedSurvey.SOUTH = t;
        }
        if (EAST != null) {
            EAST.NormalizedSurvey.WEST = t;
        }
        if (SOUTH != null) {
            SOUTH.NormalizedSurvey.NORTH = t;
        }
        if (WEST != null) {
            WEST.NormalizedSurvey.EAST = t;
        }
        if (NORTHWEST != null) {
            NORTHWEST.NormalizedSurvey.SOUTHEAST = t;
        }
        if (NORTHEAST != null) {
            NORTHEAST.NormalizedSurvey.SOUTHWEST = t;
        }
        if (SOUTHWEST != null) {
            SOUTHWEST.NormalizedSurvey.NORTHEAST = t;
        }
        if (SOUTHEAST != null) {
            SOUTHEAST.NormalizedSurvey.NORTHWEST = t;
        }
    }
}
