using System.Collections.Generic;
using System.Linq;

public class Scoreboard {
    public Dictionary<PlayerAssignment, int> CurrentScores = new Dictionary<PlayerAssignment, int>();
    public Dictionary<PlayerSlot, int> CurrentTerraformerCount = new Dictionary<PlayerSlot, int>();
    PlayerAssignment CurrentTurnPlayer;

    int TERRAFORMER_STARTING_COUNT = 7;

    public Scoreboard(PlayerManifest pm) {
        foreach (PlayerAssignment pa in pm.players) {
            CurrentTerraformerCount.Add(pa.slot, TERRAFORMER_STARTING_COUNT);
            CurrentScores.Add(pa, 0);
        }
        CurrentTurnPlayer = CurrentScores.Keys
            .OrderBy(k => (int) k.slot)
            .First();
    }

    public void CollectGamepiece(GamepieceType type, PlayerSlot slot) {
        if (type == GamepieceType.TERRAFORMER) {
            CurrentTerraformerCount[slot]++;
        }
    }

    public void RemoveGamepieceFromStash(GamepieceType type, PlayerSlot slot) {
        if (type == GamepieceType.TERRAFORMER) {
            CurrentTerraformerCount[slot]--;
        }
    }

    public PlayerAssignment GetCurrentTurnPlayer() {
        return CurrentTurnPlayer;
    }

    public int GetScoreForPlayerSlot(PlayerSlot ps) {
        return CurrentScores.First(kvp => kvp.Key.slot == ps).Value;
    }
    
    public void AddScoreForPlayerSlot(PlayerSlot ps, int scoreIncrease) {
        PlayerAssignment pa = CurrentScores.Keys.First(k => k.slot == ps);
        CurrentScores[pa] += scoreIncrease;
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
