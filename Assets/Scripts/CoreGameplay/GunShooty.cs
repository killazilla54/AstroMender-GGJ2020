using System.Collections;
using System.Collections.Generic;
using GGJ2020.Projectiles;
using UnityEngine;

public class GunShooty : MonoBehaviour
{
    public float fireRate;

    public GameObject bulletPrefab;

    public PlayerController player;

    private float fireTimer;

    private Rigidbody rBody;

    private void Start()
    {
        rBody = transform.root.GetComponent<Rigidbody>();
        InputManager.instance.AssignFunctionToShootHeldDelegate(FireGun);
        EnergySystem.instance.SubscribeToEnergyTierDecreasedEvent(HandleEnergyDecreased);
        EnergySystem.instance.SubscribeToBuffEquippedEvent(HandleBuffEquipped);
    }

    private void HandleEnergyDecreased (int tier){
        float rateModifier = EnergySystem.instance.GetAttackRateBuff();
        fireRate += rateModifier;
    }

    private void HandleBuffEquipped(int tier){
        float rateModifier = EnergySystem.instance.GetAttackRateBuff();
        fireRate += rateModifier;
    }

    private void Update()
    {
        fireTimer += Time.deltaTime;
        
        if (Input.GetMouseButton(0))
        {
            FireGun();
        }
    }

    private void FireGun()
    {
        if (fireTimer >= fireRate)
            {
                if (EnergySystem.instance.ShootGun()) {
                    // GameObject temp = GameObject.Instantiate(bulletPrefab, transform.position, transform.rotation);
                    GameObject temp = ProjectilePool.Instance.GetProjectile(ProjectilePool.ProjectileTypes.Player);
                    temp.transform.position = transform.position;
                    temp.transform.forward = -player.aimReticle.forward;
                    temp.GetComponent<BasicBullet>().SetPlayerVelocity(rBody.velocity);
                    // temp.GetComponent<BasicBullet>().
                    fireTimer = 0f;
                }
            }
    }
}
