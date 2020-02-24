using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class MenuController : MonoBehaviour
{
    private Player player; // The Rewired Player
    int playerId = 0;

    bool paused;
    // Start is called before the first frame update
    void Start()
    {
        player = Rewired.ReInput.players.GetPlayer(playerId);
        paused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.GetButtonDown("Pause")){
            if(!paused){
                Time.timeScale = 0f;
            } else {
                Time.timeScale = 1f;
            }
            paused = !paused;
        } 
    }
}
