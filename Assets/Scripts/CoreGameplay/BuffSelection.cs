using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class BuffSelection : MonoBehaviour
{
    Player player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(0);
    }

    // Update is called once per frame
    void Update()
    {
        
        //If inputs
        if(player.GetButtonDown("BuffOption1")){
            EnergySystem.instance.EquipBuffSelection(0);
        } else if(player.GetButtonDown("BuffOption2")){
            EnergySystem.instance.EquipBuffSelection(1);
        } else if(player.GetButtonDown("BuffOption3")){
            EnergySystem.instance.EquipBuffSelection(2);
        }
        //Call Energy System select buff (pass index, values 0-2 clockwise starting left)
    }
}
