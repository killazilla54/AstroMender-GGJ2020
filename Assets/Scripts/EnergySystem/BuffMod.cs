using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="PlayerMods/Buff")]
public class BuffMod : ShipMod
{
    public string effectName;
    public float effectModifier;
    public float energyModifier;
    public int weight;
    //public int Rarity/min level to show
    public BuffMod(ShipModEnum.ModType type, string effectName, 
        int tier, float effectModifier, float energyModifier, int weight) 
        : base(type,tier){
        this.effectModifier = effectModifier;
        this.weight = weight;
    }
}
