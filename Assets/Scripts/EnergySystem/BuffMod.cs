using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Kevin O'Neil
[CreateAssetMenu(menuName="PlayerMods/Buff")]
/**
    This is the class for defining a mod used as a buff.
    This is not one of the core systems (static effects) but instead
    can be built in the Editor as a scriptable object and added to a loot table
    for random upgrades at higher levels.
*/
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
