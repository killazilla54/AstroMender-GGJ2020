using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Kevin O'Neil
public class ModInventory
{
    public Dictionary<ShipModEnum.ModType,ShipMod> equipedMods;
    public List<BuffMod> equippedBuffs;

    [SerializeField]
    public Stack<ShipMod> availableMods;
    private List<BuffMod> availableBuffsForUserSelection;

    private int highestUnlockedTier;

    private BuffLootTable lootTable;

    //Some structure to hold collections of mods. And a way to access them

    public ModInventory(){

        //Allocate memory
        equipedMods = new Dictionary<ShipModEnum.ModType, ShipMod>();
        availableMods = new Stack<ShipMod>();
        equippedBuffs = new List<BuffMod>();
        ModFactory factory = new ModFactory();
        lootTable = new BuffLootTable();

        //Build initial inventory of core system mods.
        ShipMod movementMod = factory.BuildMod(ShipModEnum.ModType.MovementCore);
        equipedMods.Add(movementMod.ShipModType,movementMod);
        ShipMod boostMod = factory.BuildMod(ShipModEnum.ModType.BoostCore);
        availableMods.Push(boostMod);
        ShipMod dodgeRollMod = factory.BuildMod(ShipModEnum.ModType.DodgeRollCore);
        availableMods.Push(dodgeRollMod);
        ShipMod shootMod = factory.BuildMod(ShipModEnum.ModType.ShooterCore);
        availableMods.Push(shootMod);
        Debug.Log("Available mods size: " + availableMods.Count);

        //Movement Mod must be enabled at start.
        highestUnlockedTier = 1;
    }

    public void EquipNextMod(int tier){
        if(tier <= highestUnlockedTier){
            return; //We've been here already, so no new unlock
        }
        //Otherwise:
        highestUnlockedTier = tier;
        if(availableMods.Count != 0) {
            ShipMod nextMod = availableMods.Pop();
            Debug.Log("equiping next Mod: " + nextMod.ShipModType.ToString());
            equipedMods.Add(nextMod.ShipModType, nextMod);
        } else {
            availableBuffsForUserSelection = GenerateBuffOptions(tier);
        }
    }

    //Check for UI
    public bool IsModEquipped(ShipModEnum.ModType type){
        //Can simply this to one line.
        ShipMod checkedType;
        bool found = equipedMods.TryGetValue(type, out checkedType);
        return found;
    }

    public bool IsModEquippedActiveAtTier(ShipModEnum.ModType type, int tierLevel){
        ShipMod checkedType = null;
        bool found = equipedMods.TryGetValue(type, out checkedType);
        if(!found){
            return false;
        } else if(checkedType != null) {
            return checkedType.TierLevel <= tierLevel;
        } else {
            Debug.LogError("Passed type is not equipped. This should not happen...What have you done???");
            return false;
        }
    }

    /*
     *  Returns total buff effect for type that are equipped and powered.
     *  Returns 0 if no buffs enabled.
     */
    public float GetTotalBuffForType(ShipModEnum.ModType type, int currentTier){
        float buffTotal = 0;
        foreach(BuffMod buff in equippedBuffs){
            if(type == buff.ShipModType && currentTier >= buff.TierLevel){
                //Matching type and we have the energy to use
                buffTotal += buff.effectModifier;
            }
        }
        return buffTotal;
    }

    /*
     *  Returns total energy modifier for type that are equipped and powered.
     *  Returns 0 if no buffs enabled.
     */
    public float GetTotalEnergyModifierForType(ShipModEnum.ModType type, int currentTier){
        float costModifier = 0;
        foreach(BuffMod buff in equippedBuffs){
            if(type == buff.ShipModType && currentTier >= buff.TierLevel){
                //Matching type and we have the energy to use
                costModifier += buff.energyModifier;
            }
        }
        return costModifier;
    }

    /*
        This call should come from the controller handling user input for 
        Level up mod selection.  Will add the selected mod to the player inventory.
    */
    public bool EquipBuff(int index){
        if(availableBuffsForUserSelection == null) return false;

        Debug.Log("Equiping Buff #"+index);
        BuffMod mod = availableBuffsForUserSelection[index];
        equippedBuffs.Add(mod);
        availableBuffsForUserSelection = null;
        return true;
    }

    /*
        For use when player levels up and all core systems are unlocked.
        Generates 3 random buff mods to be returned to the UI for user selection.
    */
    public List<BuffMod> GenerateBuffOptions(int currentTier){//pass buff Loot table?
        //Randomness?
        Debug.Log("Generating some buffs");
        ModFactory factory = new ModFactory();
        List<BuffMod> choices = new List<BuffMod>();
        
        BuffMod mod1 = lootTable.RollBuff();
        Debug.Log(mod1);
        choices.Add(mod1);
        BuffMod mod2 = lootTable.RollBuff();
        Debug.Log(mod2);
        choices.Add(mod2);
        BuffMod mod3 = lootTable.RollBuff();
        Debug.Log(mod3);
        choices.Add(mod3);

        return choices;
    }

    public void buildLootTable(List<BuffMod> modConfig){
        Debug.Log(modConfig.Count);
        if(modConfig == null){
            Debug.LogWarning("Loot Table is empty, buffs are disabled");
            lootTable.buildLootTable(new List<BuffMod>());
            return;
        }
        lootTable.buildLootTable(modConfig);
    }
}
