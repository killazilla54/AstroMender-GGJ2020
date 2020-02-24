using System.Collections;
using System.Collections.Generic;
using GGJ2020.Projectiles;
using UnityEngine;

public class DumbEnemy : MonoBehaviour
{
    public CollisionWrapper aggroCollisionWrapper, getHurtCollisionWrapper;

    public EnemyHealthComponent enemyHealth;

    private Transform playerTransform;

    private Vector3 aggroStartPosition, smoothLookAt, randomOffset, cachedForward;

    public float distance, moveSpeed, aggroSpeed, shuffleTime, randomRange = 5f, minCooldown, maxCooldown;

    private float aggroTimer, shuffleTimer, fireTimer;

    private int missShots;

    private void Start()
    {
        aggroCollisionWrapper.AssignFunctionToTriggerEnterDelegate(HandleAggro);
        getHurtCollisionWrapper.AssignFunctionToTriggerEnterDelegate(HandleGetHurt);
    }

    private void HandleGetHurt(Collider other)
    {
        ProjectilePool.Instance.ReturnProjectile(ProjectilePool.ProjectileTypes.Player, other.gameObject);
        enemyHealth.TakeDamage(CombatBalancing.damagePerBullet);
    }

    private void HandleAggro(Collider other)
    {
        if(playerTransform != null) return;
        playerTransform = other.transform.root;
        aggroStartPosition = transform.position;
        distance += Random.Range(-3f, 3f);
        randomOffset = Random.insideUnitCircle * randomRange;
        smoothLookAt = transform.position + transform.forward;
        cachedForward = playerTransform.forward;
        aggroTimer = 1f;
        fireTimer = Random.Range(minCooldown, maxCooldown) + 2f;
        missShots = Random.Range(3,6);
    }

    public void Shoot()
    {
        // Grab a bullet
        GameObject bullet = ProjectilePool.Instance.GetProjectile(ProjectilePool.ProjectileTypes.Enemy);
        // Make the bullet spawn at the attacker
        bullet.transform.position = transform.position;

        //spawn bullet and look at player
        missShots--;
        if (missShots <= 0)
        {
            missShots = Random.Range(3,6);
            bullet.transform.LookAt(playerTransform.position + playerTransform.forward * 2f);
        }
        else
        {
            bullet.transform.LookAt(playerTransform.position - playerTransform.forward * 2f);
        }
        
        // Reset the fire timer before allowing firing again
        fireTimer = Random.Range(minCooldown, maxCooldown);
    }

    private void FixedUpdate()
    {
        if (playerTransform != null)
        {
            smoothLookAt = Vector3.Lerp(smoothLookAt, playerTransform.position, Time.deltaTime * 2f);
            transform.LookAt(smoothLookAt);

            if (shuffleTimer < shuffleTime)
            {
                shuffleTimer += Time.deltaTime;
                if(shuffleTimer >= shuffleTime)
                {
                    randomOffset = Random.insideUnitCircle * randomRange;
                    cachedForward = playerTransform.forward;
                    shuffleTimer = 0f;
                }
            }

            if(fireTimer > 0f)
            {
                fireTimer -= Time.deltaTime;
                if (fireTimer <= 0f)
                {
                    Shoot();
                }
            }
        }

        if (aggroTimer > 0f)
        {
            aggroTimer -= Time.deltaTime * aggroSpeed;
            transform.position = Vector3.Slerp(aggroStartPosition, TargetPosition, 1f - aggroTimer);
        }
        else if (playerTransform != null)
        {
            transform.position = Vector3.Lerp(transform.position, TargetPosition, Time.fixedDeltaTime * moveSpeed);
        }
    }

    private Vector3 TargetPosition
    {
        get
        {
            return playerTransform.position + cachedForward * distance + playerTransform.TransformDirection(randomOffset);
        }
    }
}
