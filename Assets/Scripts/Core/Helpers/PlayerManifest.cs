using System.Collections.Generic;

public class PlayerManifest {
    public List<PlayerAssignment> players = new List<PlayerAssignment>();
    public PlayerManifest(PlayerAlignmentType pat) {
        switch (pat) {
            case PlayerAlignmentType.HUMAN_V_CPU_PRESET:
                players.Add(new PlayerAssignment { slot = PlayerSlot.PLAYER1, type = PlayerType.HUMAN });
                players.Add(new PlayerAssignment { slot = PlayerSlot.PLAYER2, type = PlayerType.CPU });
                break;
            case PlayerAlignmentType.CPU_V_CPU_PRESET:
                players.Add(new PlayerAssignment { slot = PlayerSlot.PLAYER1, type = PlayerType.CPU });
                players.Add(new PlayerAssignment { slot = PlayerSlot.PLAYER2, type = PlayerType.CPU });
                break;
            case PlayerAlignmentType.HUMAN_V_HUMAN_LOCAL_PRESET:
                players.Add(new PlayerAssignment { slot = PlayerSlot.PLAYER1, type = PlayerType.HUMAN });
                players.Add(new PlayerAssignment { slot = PlayerSlot.PLAYER2, type = PlayerType.HUMAN });
                break;
            case PlayerAlignmentType.CUSTOM:
                break;
        }
    }
}
