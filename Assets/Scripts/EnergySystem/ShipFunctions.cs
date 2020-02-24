using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShipFunctions : MonoBehaviour
{

    public EnergySystem energySystem;
    public Text energyText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q)){
            energySystem.ShootGun();
        } else if(Input.GetKeyDown(KeyCode.W)){
            energySystem.Boost();
        } else if(Input.GetKeyDown(KeyCode.E)){
            energySystem.RechargeShields();
        } else if(Input.GetKeyDown(KeyCode.R)){
            energySystem.RefillEnergy(25f);
        } else if (Input.GetKeyDown(KeyCode.T)){
            energySystem.EnergyTierUp();
        }
        // Debug.Log("Current Energy: "+ energySystem.GetCurrentEnergyAmount());
        // energyText.text = "Energy: "+ energySystem.GetCurrentEnergyAmount()+" / "+ energySystem.GetMaxEnergy();
    }

    
}
