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
    
    public ScoringEvent(
        Action scoringAction,
        List<Tile> relatedTiles,
        List<GamepieceTileAssignment> relatedGamepieces,
        ScoringEventType eventType,
        Dictionary<PlayerSlot, int> netScoreChangeByPlayer,
        string description
    ) {
        ScoringAction = scoringAction;
        RelatedTiles = relatedTiles;
        RelatedGamepieces = relatedGamepieces;
        EventType = eventType;
        NetScoreChangeByPlayer = netScoreChangeByPlayer;
        Description = description;
    }

}

public enum ScoringEventType {
    ROADCOMPLETED,
    OBELISKCOMPLETED,
    CITYCOMPLETED
}

public enum PlayerSlot {
    PLAYER1,
    PLAYER2,
    PLAYER3,
    PLAYER4
}