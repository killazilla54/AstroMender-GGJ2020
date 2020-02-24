using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffLootTable
{

    private List<BuffMod> lootTable;

    //Initializes the loottable for this session.  
    //Loot table items and weights are configured in the Editor.
    //via BuffLootTableConfig.cs
    public void buildLootTable(List<BuffMod> availableMods){
        if(availableMods.Count == 0 || availableMods == null){
            Debug.LogWarning("Loot Table is not configured.  Buff system is effectively disabled until mods are configured.");
        }
        lootTable = new List<BuffMod>();
        foreach(BuffMod mod in availableMods){
            for(int i = 0; i<mod.weight;i++){
                lootTable.Add(mod);
            }
        }
    }

    //Random roll for an item from the loot table.
    //Returns the mod picked.
    public BuffMod RollBuff(){
        if(lootTable == null || lootTable.Count == 0) return null;
        int index = Random.Range(0,lootTable.Count);  
        Debug.Log("Rolled Buff at index " + index);      
        return lootTable[index];
    }    
}
