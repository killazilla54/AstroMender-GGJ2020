using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy
{
    public override void Update()
    {
        base.Update();
        if (aggroed)
        {
            MoveTowardsTransform(AggroTarget);

            if (canFire && InFrontOfPlayer(AggroTarget) && WithinFiringRange(AggroTarget))
            {
                Shoot();
            }
        }
    }
}
