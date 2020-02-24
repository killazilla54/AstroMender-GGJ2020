using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GGJ2020.ObjectPooling;
using HarmonyQuest.Util;

public class EnemyHealthComponent : MonoBehaviour
{
    public int enemyTier;

    //THIS IS MULTIPLIED BY ENEMY TIER AS WELL
    public int bonusHealth;

    private int health, maxHealth;

    public delegate void TakeDamageEvent(int damage);
    TakeDamageEvent takeDamageDelegate, loseHealthTierDelegate;

    public GameObject explosionPrefab;

    public GameObject[] energyPickupTiers;

    public int MaxHealth
    {
        get
        {
            return maxHealth;
        }
    }

    public float CurrentHealthBarPercent
    {
        get
        {
            return ((float)health/maxHealth)*(enemyTier+1)%1f;
        }
    }

    public int CurrentHealthbars
    {
        get
        {
            return Mathf.CeilToInt(((float)health/maxHealth)*(1+enemyTier));
        }
    }

    private void Start()
    {
        enemyTier = CombatBalancing.curEnemyTier;
        health = CombatBalancing.GetMaxHealthAtTier(enemyTier+1) + bonusHealth * (enemyTier+1);
        maxHealth = health;
        Debug.Log("Registered new enemy with " + maxHealth);
        EnemyHealthBarSystem.instance.RegisterNewHealthBar(this);
    }

    public void TakeDamage(int damage) {
        int currentTiers = CurrentHealthbars;

        health -= damage;

        SoundPool.instance.PlaySound(SoundClips.instance.EnemyHit, transform.position, false, false, 0.2f);

        if(CurrentHealthbars < currentTiers)
        {
            if(loseHealthTierDelegate != null)
            {
                loseHealthTierDelegate(CurrentHealthbars);
            }
        }
        else if (takeDamageDelegate != null)
        {
            takeDamageDelegate(damage);
        }

        if (health <=0){
            Death();
        }
    }

    public void AddTakeDamageListener (TakeDamageEvent damageEvent)
    {
        takeDamageDelegate += damageEvent;
    }

    public void AddLoseTierListener (TakeDamageEvent damageEvent)
    {
        loseHealthTierDelegate += damageEvent;
    }

    public void Death()
    {
        int randomSound = Random.Range(0,3);
        switch(randomSound)
        {
            case 0:
                SoundPool.instance.PlaySound(SoundClips.instance.EnemyExplosion, transform.position, false, false, 0.5f);
            break;
            case 1:
                SoundPool.instance.PlaySound(SoundClips.instance.EnemyExplosion2, transform.position, false, false, 0.5f);
            break;
            case 2:
                SoundPool.instance.PlaySound(SoundClips.instance.EnemyExplosion3, transform.position, false, false, 0.5f);
            break;
        }

        GameObject temp = Instantiate(explosionPrefab, transform.position, transform.rotation);
        Destroy(temp, 2f);
        // Destroy(this.transform.root.gameObject);
        GameManager.instance.procObjectManager.Deallocate(transform.root.gameObject);

        CombatBalancing.IncrementEnemiesKilled();

        GameObject tempEnergy = Instantiate(energyPickupTiers[Mathf.Min(enemyTier, 2)], transform.position - transform.forward * 2f, Quaternion.identity);
    }
}
