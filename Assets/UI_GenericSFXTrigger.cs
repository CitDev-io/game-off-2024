using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GenericSFXTrigger : MonoBehaviour
{
    public void PlaySfx(string sfxName) {
       FindAnyObjectByType<GameController_DDOL>().PlaySound(sfxName);
    }
}
