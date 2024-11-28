public class AssembledObelisk {
    int index = 0;
    bool collected = false;
    public TileAndPlacementGroupIndex tilePi;
    public PlayerSlot collectedBy = PlayerSlot.NEUTRAL;

    public AssembledObelisk(int i, TileAndPlacementGroupIndex tpi) {
        index = i;
        tilePi = tpi;
    }

    public void MarkAsCollectedBy(PlayerSlot ps) {
        collected = true;
        collectedBy = ps;
    }

    public bool IsCompleteAndUncollected() {
        return IsComplete() && !collected;
    }

    public bool IsComplete() {
        return tilePi.tile.NormalizedSurvey.ExtendedNeighborhoodCount() == 8;
    }

    public bool IsOccupied() {
        return tilePi.tile.obelisk?.GroupId == tilePi.groupIndexId;
    }
};
