using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowScript : MonoBehaviour
{
    public PlayerController playerController;

    public float followDistance, followHeightOffset, heightOffset, smoothFollowSpeed;

    private Vector3 aimOffsetVector, smoothAimPosition;

    private void Start()
    {
        smoothAimPosition = playerController.AimPosition;
    }

    private void FixedUpdate()
    {
        Vector3 targetPosition = playerController.transform.position - playerController.transform.forward*followDistance;

        if (Mathf.Abs(playerController.AimHeight) > 0.3f)
        {
            heightOffset = Mathf.Sign(playerController.AimHeight);
        }
        heightOffset = 1f;
        Vector3 targetAimOffset = Vector3.up * followHeightOffset * heightOffset;

        smoothAimPosition = Vector3.Lerp(smoothAimPosition, playerController.AimPosition, Time.deltaTime * smoothFollowSpeed);

        transform.LookAt(smoothAimPosition);

        aimOffsetVector = Vector3.Lerp(aimOffsetVector, targetAimOffset, Time.deltaTime);
        transform.position = targetPosition + aimOffsetVector;
        // transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10f);
    }
}
