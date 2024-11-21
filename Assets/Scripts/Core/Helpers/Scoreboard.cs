using System.Collections.Generic;
using System.Linq;

public class Scoreboard {
    public Dictionary<PlayerAssignment, int> CurrentScores = new Dictionary<PlayerAssignment, int>();
    PlayerAssignment CurrentTurnPlayer;

    public Scoreboard(PlayerManifest pm) {
        foreach (PlayerAssignment pa in pm.players) {
            CurrentScores.Add(pa, 0);
        }
        CurrentTurnPlayer = CurrentScores.Keys
            .OrderBy(k => (int) k.slot)
            .First();
    }

    public PlayerAssignment GetCurrentTurnPlayer() {
        return CurrentTurnPlayer;
    }

    public PlayerAssignment GetNextTurnPlayer() {
        List<PlayerAssignment> players = CurrentScores.Keys
            .OrderBy(k => (int) k.slot)
            .ToList();
        int currentIndex = players.IndexOf(CurrentTurnPlayer);
        int nextIndex = (currentIndex + 1) % players.Count;
        return players[nextIndex];
    }
    // would be internal
    public void AdvanceToNextTurn() {
        CurrentTurnPlayer = GetNextTurnPlayer();
    }
}
