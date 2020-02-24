using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrafeEnemy : Enemy
{
    public float strafeDistance = 90.0f;
    public float strafeCancel = 150.0f;
    bool isStrafing = false;

    //Sin values used to add some random fluctuation to enemy height.
    private float sinHeight = 0.1f;
    private float sinSpeed = 1.0f;
    private float sinTimingOffset = 1.0f;

    private float sinYPosOffset = 0.0f;

    [HideInInspector]
    public float strafeDirection = 1;

    float playerVelocityRatio = 0.4f;

    public override void Start()
    {
        base.Start();
        sinTimingOffset = Random.value * (Mathf.PI / 2);
        strafeDirection = Random.Range(0, 2) * 2 - 1;
    }

    public virtual void StrafeAroundTransform(Transform target)
    {
        if(InFrontOfPlayer(target))
        {
            speedMultiplier = 0.65f;
            playerVelocityRatio = 0.3f;
        }
        else
        {
            speedMultiplier = 1.75f;
            playerVelocityRatio = 0.05f;
        }

        if (Vector3.Distance(target.position, transform.position) < strafeDistance && isStrafing == false)
        {
            isStrafing = true;
        }
        else if (Vector3.Distance(target.position, transform.position) > strafeCancel && isStrafing == true)
        {
            isStrafing = false;
        }

        if (isStrafing == false)
        {
            MoveTowardsVector(target.position);
            // RotateTowardsDirection(target.position - transform.position);
        }
        else
        {
            MoveInDirection((Vector3.Cross(Vector3.up, target.position - transform.position) * strafeDirection).normalized * (1 - playerVelocityRatio)
                + (target.GetComponent<PlayerController>().GetVelocity()).normalized * playerVelocityRatio, false);
            RotateTowardsDirection(target.position - transform.position);
        }
        sinYPosOffset = (Mathf.Sin((Time.time + sinTimingOffset) * sinSpeed)) * sinHeight;
        transform.position = new Vector3(transform.position.x, transform.position.y + sinYPosOffset, transform.position.z);
    }

    public override void Update()
    {
        base.Update();
        if (aggroed)
        {
            StrafeAroundTransform(AggroTarget);

            if (canFire && InFrontOfPlayer(AggroTarget) && WithinFiringRange(AggroTarget))
            {
                Shoot();
            }
        }
    }
}
