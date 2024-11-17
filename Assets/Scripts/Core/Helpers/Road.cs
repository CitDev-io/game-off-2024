public class Road {
    public CardinalDirection localizedDirection;
    public int RoadGroupId;

    public Road(CardinalDirection direction, int roadGroupId)
    {
        this.localizedDirection = direction;
        RoadGroupId = roadGroupId;
    }
}
