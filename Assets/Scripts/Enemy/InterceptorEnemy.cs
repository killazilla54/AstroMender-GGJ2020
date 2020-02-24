using UnityEngine;

public class InterceptorEnemy : Enemy
{
    public enum InterceptorState { Approach, Attack, LoopAround};
    InterceptorState interceptorState = InterceptorState.Approach;

    public float interceptDistanceFromPlayer = 5.0f;
    private Vector3 interceptDestination;

    public override void Update()
    {
        base.Update();
        if (aggroed)
        {
            if (interceptorState == InterceptorState.Attack)
            {
                interceptDestination = AggroTarget.position + Vector3.up * interceptDistanceFromPlayer;
                MoveTowardsVector(interceptDestination);

                if (canFire && WithinFiringRange(AggroTarget))
                {
                    Shoot();
                }
                if (Vector3.Distance(interceptDestination, transform.position) < 7.5f)
                {
                    speedMultiplier = 1.0f;
                    interceptorState = InterceptorState.LoopAround;
                }
            }
            else if (interceptorState == InterceptorState.LoopAround)
            {
                Vector3 targetDir = AggroTarget.position - transform.position;

                // The step size is equal to speed times frame time.
                float step = rotateSpeed * Time.deltaTime;

                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);

                // Move our position a step closer to the target.
                transform.rotation = Quaternion.LookRotation(newDir);

                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

                MoveInDirection(transform.forward, false);

                Vector3 targetPositionSameYPos = new Vector3(AggroTarget.position.x, transform.position.y, AggroTarget.position.z);
                Vector3 targetDirSameYPos = targetPositionSameYPos - transform.position;

                if (Vector3.Angle(transform.forward, targetDirSameYPos) < 10.0f)
                {
                    speedMultiplier = 0.7f;
                    interceptorState = InterceptorState.Attack;
                }
            }
            else
            {
                MoveTowardsTransform(AggroTarget);
                if (WithinFiringRange(AggroTarget))
                {
                    speedMultiplier = 0.7f;
                    interceptorState = InterceptorState.Attack;
                }
            }
        }
        else
        {
            speedMultiplier = 1.0f;
            interceptorState = InterceptorState.Approach;
            MoveInDirection(transform.forward);
        }
    }
}
