using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Control;

namespace RPG.Cinematics
{
public class CinematicControlRemover1 : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<PlayableDirector>().played += DisableControl;
        GetComponent<PlayableDirector>().stopped += EnableControl;
        player = GameObject.FindWithTag("Player");
    }
        void DisableControl (PlayableDirector pd)
        {
           
           player.GetComponent<ActionScheduler>().CancelCurrentAction();
           player.GetComponent<PlayerController>().enabled = false;
        }
        void EnableControl (PlayableDirector pd)
        {
             player.GetComponent<PlayerController>().enabled = true;
        }
}
}
