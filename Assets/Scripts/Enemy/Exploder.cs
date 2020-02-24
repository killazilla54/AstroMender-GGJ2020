using UnityEngine;
using GGJ2020.Projectiles;
using GGJ2020.Util;

public class Exploder : Enemy
{
    public float explodeTime = 4;
    public float explodeDistance = 10;
    public float sfxTime = 1;
    public SoundPlayer beepSFX;

    private float explodeTimer;
    private float sfxTimer;
    private bool wasAggroed;

    public override void Start()
    {
        base.Start();
        explodeTimer = 0;
        sfxTimer = 0;
        wasAggroed = false;
    }

    public override void Update()
    {
        base.Update();
        if (aggroed)
            wasAggroed = true;

        if (wasAggroed)
        {
            float distance = Vector3.Distance(AggroTarget.position, this.transform.position);
            if (distance > explodeDistance && explodeTimer == 0)
                MoveTowardsTransform(AggroTarget);
            else
            {
                if (explodeTimer == 0)
                    speed *= .9f;

                MoveTowardsTransform(AggroTarget);
                
                if((sfxTimer += Time.deltaTime) > sfxTime)
                {
                    sfxTimer = 0;
                    sfxTime *= .75f;
                    beepSFX?.PlaySong(0);
                }

                if ((explodeTimer += Time.deltaTime) > explodeTime)
                    Die();
            }
        }
    }

    public override void Die()
    {
        base.Die();
        GameObject g = ProjectilePool.Instance.GetProjectile(ProjectilePool.ProjectileTypes.Explosion);
        if (g != null)
            g.transform.position = this.transform.position;
    }
}
