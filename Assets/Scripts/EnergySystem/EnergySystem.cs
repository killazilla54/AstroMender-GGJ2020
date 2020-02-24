using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

//Kevin O'Neil
public class EnergySystem : MonoBehaviour
{
    [SerializeField]
    private float energy;
    [SerializeField]
    private int curEnergyTier;
    [SerializeField]
    private float energyTierUpAmount;
    [SerializeField]
    private float firstTier;
    [SerializeField]
    private float minEnergy; //when you tier up, minimum energy to prevent immediate tier down

    //variables
    [SerializeField]
    private float energyPerFrame;
    [SerializeField]
    private float shootCost;
    [SerializeField]
    private float boostCost;
    [SerializeField]
    private float shieldCost; //recharge?
    [SerializeField]
    private float dodgeRollCost;
    [SerializeField]
    private float movementCost;
    [SerializeField]
    private bool healthAutoTierUp;

    private ModInventory inventory;

    public static EnergySystem instance;

    public delegate void EnergyEvent(int tier);
    public EnergyEvent EnergyTierIncreasedEvent, EnergyTierDecreasedEvent, EnergyCollectedEvent;
    public delegate void BuffEvent(int tier);
    public BuffEvent BuffEquippedEvent;

    public const int BUFF_LEVEL_THRESHOLD = 5;

    //Holder for Buff Selection so UI can have it
    private void OnEnable()
    {
        instance = this;
        inventory = new ModInventory();
        //This will usually be level 1, but left open in case we do saves or 
        //multiple levels where the player starts at a higher power tier.
        for (int i = 1; i < curEnergyTier; i++)
        {
            inventory.EquipNextMod(i);
        }
    }

    public void SubscribeToEnergyTierIncreasedEvent(EnergyEvent e)
    {
        EnergyTierIncreasedEvent += e;
    }

    public void SubscribeToEnergyTierDecreasedEvent(EnergyEvent e)
    {
        EnergyTierDecreasedEvent += e;
    }

    public void SubscribeToBuffEquippedEvent(BuffEvent e){
        BuffEquippedEvent += e;
    }

    public void SubscribeToEnergyCollectedEvent(EnergyEvent e){
        EnergyCollectedEvent += e;
    }

    public float EnergyTierUpAmount ()
    {
        if(curEnergyTier == 1) return firstTier;
        return energyTierUpAmount;
    }

    // Update is called once per frame
    void Update()
    {
        float newEnergy = energy + energyPerFrame;
        if (healthAutoTierUp && newEnergy > energyTierUpAmount){
            EnergyTierUp();
        } else {//capAtMax
            energy = Mathf.Min(energyTierUpAmount, newEnergy);
        }
    }

    public bool SpendEnergy(float cost){
        float newEnergy = energy - cost;
        
        energy = Mathf.Clamp(energy-cost, 0, energyTierUpAmount);
        
        if (energy == 0)
        {
            curEnergyTier--;
            if(curEnergyTier == 0) 
            {
                SceneManager.LoadScene(2);
            }

            if(curEnergyTier == 1)
            {
                energy = 25f;
            }
            else
            {
                energy = energyTierUpAmount - minEnergy * 2f;
            }

            SoundPool.instance.PlaySound(SoundClips.instance.PowerDown, transform.position, false);

            if (EnergyTierDecreasedEvent != null)
            {
                EnergyTierDecreasedEvent(curEnergyTier);
            }
        }
        return true;
    }

    public float GetCurrentEnergyAmount(){
        return energy;
    }

    public bool ShootGun(){
        //Check if mod is equipped
        if(inventory.IsModEquippedActiveAtTier(ShipModEnum.ModType.ShooterCore, curEnergyTier)){
            float shootCostModifier = inventory.GetTotalEnergyModifierForType(ShipModEnum.ModType.AttackRate, curEnergyTier);
            //Attack damage isn't technically done here, 
            //but we can pretend little robots are triggered to add more juice to the lazer
            shootCostModifier += inventory.GetTotalEnergyModifierForType(ShipModEnum.ModType.AttackDamage, curEnergyTier);
            float adjustedShootCost = shootCost + shootCostModifier;
            bool success = SpendEnergy(adjustedShootCost);
            return success;
        }
        //Mod not equipped or active 
        return false;
    }

    public bool Boost(){
        //Debug.Log("Boost Command Received");
        if(inventory.IsModEquippedActiveAtTier(ShipModEnum.ModType.BoostCore, curEnergyTier)){
            float adjustedBoostCost = (boostCost + inventory.GetTotalEnergyModifierForType(ShipModEnum.ModType.BoostModifer,curEnergyTier)) * Time.fixedDeltaTime;
            bool success = SpendEnergy(adjustedBoostCost);
            //Debug.Log("Boost attempt result: " + success);
            return success;
        }
        //Mod not equipped or active
        return false;
    }

    public bool DodgeRoll(){
        // Debug.Log("DodgeRoll Command Received");
        if(inventory.IsModEquippedActiveAtTier(ShipModEnum.ModType.DodgeRollCore, curEnergyTier)){
            float adjustedDodgeCost = dodgeRollCost + inventory.GetTotalEnergyModifierForType(ShipModEnum.ModType.DodgeRollModifer,curEnergyTier);
            bool success = SpendEnergy(adjustedDodgeCost);
            // Debug.Log("DodgeRoll attempt result: " + success);
            return success;
        }
        //Mod not equipped or active
        return false;
    }

    //Replace with BarrelRoll
    public bool RechargeShields(){
        // Debug.Log("Shield Recharge Command Received");
        bool success = SpendEnergy(shieldCost);
        // Debug.Log("Shield attempt result: " + success);
        return success;
    }

    public bool MoveShip(float factor) {
        // Debug.Log("Move Ship Command Received");
        if(inventory.IsModEquippedActiveAtTier(ShipModEnum.ModType.MovementCore, curEnergyTier)){
            float adjustedMoveCost = movementCost + inventory.GetTotalEnergyModifierForType(ShipModEnum.ModType.MovementModifer,curEnergyTier);
            bool success = SpendEnergy(adjustedMoveCost * factor);
            // Debug.Log("Move Ship attempt result: " + success);
            return success;
        }
        return false;
    }

    public void RefillEnergy(float energyAmount) {
        energy = energy + energyAmount;
        EnergyCollectedEvent((int)energyAmount);
        //Do we need an overflow check for amounts > energyTierUpAmount?
        if (curEnergyTier == 1 && energy > firstTier)
        {
            EnergyTierUp();
        }
        else if (energy > energyTierUpAmount)
        {
            EnergyTierUp();
        }
        
    }

    public void EnergyTierUp() {
        curEnergyTier++;
        energy = minEnergy;
        SoundPool.instance.PlaySound(SoundClips.instance.PowerUp, transform.position, false);
        inventory.EquipNextMod(curEnergyTier);

        if(EnergyTierIncreasedEvent != null)
        {
            EnergyTierIncreasedEvent(curEnergyTier);
        }
    }

    /* For calculating what effective tier is.  This is different from maxTier.
    * Max tier would show all unlocked but disabled mods, while this activeTier would
    * be used to determine what unlocked mods are currently active based on charged 
    * energy tiers.
    */
    // public int CalculateActiveTier(){
    //     int tierByCurrentEnergy = Mathf.FloorToInt(energy / energyTierUpAmount) + 1;
    //     return tierByCurrentEnergy;
    // }

    public void BuildLootTable(List<BuffMod> lootTableConfig){
        inventory.buildLootTable(lootTableConfig);
    }

    public float GetAttackRateBuff(){
        return inventory.GetTotalBuffForType(ShipModEnum.ModType.AttackRate, curEnergyTier);
    }

    public List<BuffMod> GenerateBuffChoicesForLevelUp(){
        return inventory.GenerateBuffOptions(curEnergyTier);
    }

    public void EquipBuffSelection(int index){
        bool wasEquipped = inventory.EquipBuff(index);
        if(wasEquipped){
            BuffEquippedEvent(curEnergyTier);
        }
    }
}
