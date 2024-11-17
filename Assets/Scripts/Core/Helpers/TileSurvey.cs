#nullable enable
public class TileSurvey {
    public Tile? NORTH;
    public Tile? EAST;
    public Tile? SOUTH;
    public Tile? WEST;

    public Tile? TileInDirection(CardinalDirection dir) {
        return dir switch {
            CardinalDirection.NORTH => NORTH,
            CardinalDirection.EAST => EAST,
            CardinalDirection.SOUTH => SOUTH,
            CardinalDirection.WEST => WEST,
            _ => null
        };
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
    }
}
