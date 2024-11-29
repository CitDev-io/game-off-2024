using System;
using System.Collections;
using System.Collections.Generic;

public class ScoringEvent {
    public Action ScoringAction;
    public readonly List<Tile> RelatedTiles;
    public readonly List<GamepieceTileAssignment> RelatedGamepieces;
    public readonly ScoringEventType EventType;
    public readonly Dictionary<PlayerSlot, int> NetScoreChangeByPlayer;
    public readonly string Description;
    public readonly PlayerSlot PrivacyFilter = PlayerSlot.NEUTRAL;
    
    public ScoringEvent(
        Action scoringAction,
        List<Tile> relatedTiles,
        List<GamepieceTileAssignment> relatedGamepieces,
        ScoringEventType eventType,
        Dictionary<PlayerSlot, int> netScoreChangeByPlayer,
        string description,
        PlayerSlot privacyFilter = PlayerSlot.NEUTRAL
    ) {
        ScoringAction = scoringAction;
        RelatedTiles = relatedTiles;
        RelatedGamepieces = relatedGamepieces;
        EventType = eventType;
        NetScoreChangeByPlayer = netScoreChangeByPlayer;
        Description = description;
        PrivacyFilter = privacyFilter;
    }

}
