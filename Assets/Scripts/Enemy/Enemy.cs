using System.Collections;
using System.Collections.Generic;
using GGJ2020.Projectiles;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 5;
    public float rotateSpeed = 10;
    [HideInInspector]
    public bool alwaysFaceTarget = false;
    [HideInInspector]
    public bool aggroed = false;
    public CollisionWrapper aggroCollider, hitCollider;
    [HideInInspector]
    public Transform AggroTarget;
    [HideInInspector]
    public Rigidbody rBody;

    [HideInInspector]
    public float speedMultiplier = 1.0f;

    public float minCooldown;
    public float maxCooldown;
    private float fireTimer;
    [HideInInspector]
    public bool canFire = false;
    public float firingRange;
    public float inaccuracy;

    public delegate void EnemyDestroyedEvent();
    public EnemyDestroyedEvent OnEnemyDestroyed;

    // Start is called before the first frame update
    public virtual void Start()
    {
        AggroTarget = GameManager.instance.playerController.transform;

        aggroCollider.AssignFunctionToTriggerEnterDelegate(Aggro);
        aggroCollider.AssignFunctionToTriggerExitDelegate(Deaggro);
        aggroCollider.transform.parent = null;

        hitCollider.AssignFunctionToTriggerEnterDelegate(HitTriggerEntered);
        rBody = GetComponent<Rigidbody>();
    }

    private void HitTriggerEntered(Collider other)
    {
        ProjectilePool.Instance.ReturnProjectile(ProjectilePool.ProjectileTypes.Player, other.gameObject);
        GetComponent<EnemyHealthComponent>().TakeDamage(CombatBalancing.damagePerBullet);
    }

    public virtual void Die()
    {
        //Use pooling instead of just destroying the gameobject
        Destroy(gameObject);
    }

    public virtual void Update()
    {
        aggroCollider.transform.position = transform.position;

        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0)
        {
            canFire = true;
        }
        else
        {
            canFire = false;
        }
    }

    public virtual void Aggro(Collider other)
    {
        // Debug.Log("AGGRO");
        if (other.transform == AggroTarget)
        {
            aggroed = true;
        }
    }

    public virtual void Deaggro(Collider other)
    {
        // Debug.Log("DEAGGRO");
        if (other.transform == AggroTarget)
        {
            aggroed = false;
        }
    }

    public virtual void Shoot()
    {
        // Grab a bullet
        GameObject bullet = ProjectilePool.Instance.GetProjectile(ProjectilePool.ProjectileTypes.Enemy);
        // Make the bullet spawn at the attacker
        bullet.transform.position = transform.position;
        // Make the bullet face in intended direction
        bullet.transform.forward = AggroTarget.position - transform.position;
        bullet.GetComponent<BasicBullet>().SetPlayerVelocity(AggroTarget.GetComponent<Rigidbody>().velocity);
        // Randomize accuracy
        bullet.transform.eulerAngles += new Vector3(Random.Range(-inaccuracy, inaccuracy), Random.Range(-inaccuracy, inaccuracy), Random.Range(-inaccuracy, inaccuracy));
        // Reset the fire timer before allowing firing again
        fireTimer = Random.Range(minCooldown, maxCooldown);
    }

    public virtual void MoveInDirection(Vector3 direction, bool rotateTowardsDirection = true)
    {
        rBody.velocity = direction.normalized * speed * speedMultiplier;
        if (rotateTowardsDirection)
        {
            RotateTowardsDirection(direction);
        }
    }

    public virtual void MoveTowardsVector(Vector3 target, bool rotateTowardsDirection = true)
    {
        Vector3 moveDirection = target - transform.position;
        MoveInDirection(moveDirection, rotateTowardsDirection);
    }

    public virtual void MoveTowardsTransform(Transform target, bool rotateTowardsDirection = true)
    {
        MoveTowardsVector(target.position, rotateTowardsDirection);
    }

    public virtual void RotateTowardsDirection(Vector3 direction)
    {
        //Rotate enemy to face target
        if (direction.magnitude > 0)
        {
            Vector3 targetPos = transform.position + direction;
            Vector3 targetDir = targetPos - transform.position;

            // The step size is equal to speed times frame time.
            float step = rotateSpeed * Time.deltaTime;

            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
            Debug.DrawRay(transform.position, newDir, Color.red);

            // Move our position a step closer to the target.
            transform.rotation = Quaternion.LookRotation(newDir);
        }
    }

    public bool InFrontOfPlayer(Transform target)
    {
        return Vector3.Angle(target.forward, transform.position - target.position) < 90.0f;
    }

    public bool WithinFiringRange(Transform target)
    {
        return Vector3.Distance(target.position, transform.position) < firingRange;
    }

    public void SubscribeToEnemyDestroyedEvent(EnemyDestroyedEvent e){
        OnEnemyDestroyed += e;
    }
}
