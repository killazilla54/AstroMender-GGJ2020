using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ShipModEnum 
{
    [System.Serializable]
    public enum ModType {
            MovementCore,
            ShooterCore,
            DodgeRollCore,
            BoostCore,
            AttackRate,
            AttackDamage,
            DamageTaken,
            PickupMagnetRange,
            MovementModifer,
            BoostModifer,
            DodgeRollModifer
            
        }
        //Damage Taken needs to wait for merge. Kartik implemented.
        //Energy Storage per tier / energy rate
        //Spreadshot
}
