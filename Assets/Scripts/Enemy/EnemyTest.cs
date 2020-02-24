using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyTest : Enemy
{
    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (aggroed)
        {
            MoveTowardsTransform(AggroTarget);
        }
    }
}
