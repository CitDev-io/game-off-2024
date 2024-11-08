public class RoadCheck {
    public CardinalDirection direction;
    public bool Terminates;
    public CardinalDirection? Attachment;

    public RoadCheck(
        CardinalDirection checkDx,
        bool terminates,
        CardinalDirection? attachment
    ) {
        direction = checkDx;
        Terminates = terminates;
        Attachment = attachment;
    }
}
