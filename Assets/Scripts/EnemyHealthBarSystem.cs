using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBarSystem : MonoBehaviour
{
    public static EnemyHealthBarSystem instance;

    private List<EnemyHealthBar> healthBars;

    private void OnEnable()
    {
        instance = this;

        healthBars = new List<EnemyHealthBar>();
        healthBars.Add(transform.GetChild(0).GetComponent<EnemyHealthBar>());
        healthBars[0].enabled = false;
        healthBars[0].gameObject.SetActive(false);
    }

    public void RegisterNewHealthBar (EnemyHealthComponent enemyHealth)
    {
        if (healthBars == null)
        {
            healthBars = new List<EnemyHealthBar>();
        }

        for (int i = 0; i < healthBars.Count; i++)
        {
            if (!healthBars[i].enabled)
            {
                SetUpNewHealthbar(healthBars[i], enemyHealth);
                return;
            }
        }

        GameObject temp = Instantiate(healthBars[0].gameObject, transform);
        EnemyHealthBar newHealthBar = temp.GetComponent<EnemyHealthBar>();
        healthBars.Add(newHealthBar);
        SetUpNewHealthbar(newHealthBar, enemyHealth);

    }

    private void SetUpNewHealthbar(EnemyHealthBar healthBar, EnemyHealthComponent enemyHealth)
    {
        healthBar.gameObject.SetActive(true);
        healthBar.enabled = true;
        healthBar.SetTargetEnemy(enemyHealth);
    }
}
