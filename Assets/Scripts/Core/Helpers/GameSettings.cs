public class GameSettings {
    public static readonly int DEFAULT_GRID_LENGTH = 15;
    public static readonly int DEFAULT_MIDPOINT = 7;
    public int OddGameBoardWidth = DEFAULT_GRID_LENGTH;
    public int BoardMidPoint = DEFAULT_MIDPOINT;
    public PlayerManifest playerManifest = new PlayerManifest(
        PlayerAlignmentType.HUMAN_V_HUMAN_LOCAL_PRESET
    );

    public GameSettings(
        int oddBoardWidth,
        int boardMidPoint,
        PlayerManifest pm
    ) {
        OddGameBoardWidth = oddBoardWidth;
        BoardMidPoint = boardMidPoint;
        playerManifest = pm;
    }

    public GameSettings() {

    }
}
