using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UI_ObjectiveOverlayManager : MonoBehaviour
{
   public GameBoardManager wired_GameBoardManager;
   public UI_Organizer_Objective wired_Objective1;
   public UI_Organizer_Objective wired_Objective2;
   public UI_Organizer_Objective wired_Objective3;
   public UI_Organizer_Objective wired_Objective4;


   public void UpdateObjectivesForPlayer(PlayerSlot playerSlot)
   {
      List<SecretObjective> objectives = 
        wired_GameBoardManager
        .GridGameInstance
        .scoreboard
        .Stats[playerSlot].Objectives;
      
      wired_Objective1.Hide();
      wired_Objective2.Hide();
      wired_Objective3.Hide();
      wired_Objective4.Hide();

      foreach ((SecretObjective objective, int index) in objectives.Select((value, index) => (value, index)))
      {
        UI_Organizer_Objective organizer;
         switch (index)
         {
            case 0:
               organizer = wired_Objective1;
               break;
            case 1:
               organizer = wired_Objective2;
               break;
            case 2:
               organizer = wired_Objective3;
               break;
            case 3:
               organizer = wired_Objective4;
               break;
            default:
                organizer = wired_Objective1;
                break;
        }
        organizer.Wired_Icon.sprite = Resources.Load<Sprite>(objective.SpritePath);
        organizer.Wired_ObjectiveText.text = objective.ObjectiveName;
        organizer.Show();
      }
   }
}
