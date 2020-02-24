using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatBalancing
{
    public static int damagePerBullet           = 10;
    public static int bulletsToKillBaseEnemy    = 10;
    public static int enemyBulletDamage         = 5;

    public static float yRange                  = 50f;

    private static int enemiesKilled;

    public static int curEnemyTier;

    public static void IncrementEnemiesKilled()
    {
        enemiesKilled++;
        if (enemiesKilled > enemiesKilledToTierUp * (1+curEnemyTier))
        {
            enemiesKilled = 0;
            if(curEnemyTier < 3)
            {
                curEnemyTier++;
            }
        }
    }

    public static int enemiesKilledToTierUp = 10;

    public static int GetMaxHealthAtTier(int tier)
    {
        return damagePerBullet * bulletsToKillBaseEnemy * tier;
    }
}
