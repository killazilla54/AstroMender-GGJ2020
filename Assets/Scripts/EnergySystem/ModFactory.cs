using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModFactory
{
    public ShipMod BuildMod(ShipModEnum.ModType type){
        ShipMod newMod = null;
        switch (type){
            case ShipModEnum.ModType.MovementCore:
                newMod = new ShipMod(type,1);
            break;
            case ShipModEnum.ModType.ShooterCore:
                newMod = new ShipMod(type,2);
            break;
            case ShipModEnum.ModType.DodgeRollCore:
                newMod = new ShipMod(type,3);
            break;
            case ShipModEnum.ModType.BoostCore:
                newMod = new ShipMod(type,4);
            break;
        }
        return newMod;
    }
}
