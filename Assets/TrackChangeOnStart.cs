using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackChangeOnStart : MonoBehaviour
{
    public string MusicName = "Start_Music";
    public bool CrossFade = false;

    void Start()
    {
        GameObject.FindObjectOfType<GameController_DDOL>().PlayMusic(MusicName, CrossFade);
    }
}
