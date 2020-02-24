using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMod : ScriptableObject
{

    public ShipMod(ShipModEnum.ModType type, int tier){
        this.ShipModType = type;
        this.TierLevel = tier;
    }

    public ShipModEnum.ModType ShipModType;

    public int TierLevel{
        get;
    }
    
}
