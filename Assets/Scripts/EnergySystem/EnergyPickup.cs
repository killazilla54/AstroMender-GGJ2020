using GGJ2020.ObjectPooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPickup : MonoBehaviour
{
    public float energyAmount;

    public int tier;

    private float pickupTimer;

    private float startingScale;

    private Vector3 startPosition, targetPosition;

    private Transform playerTransform;

    private float magnetismSpeed = 30f;

    public AnimationCurve scaleCurve;

    public float animationSpeed = 2f;

    private Vector3 randomRot;

    private void Start()
    {
        randomRot = Random.insideUnitSphere;
    }

    private void FixedUpdate()
    {
        if (pickupTimer > 0f)
        {
            pickupTimer -= Time.fixedDeltaTime * animationSpeed;
            transform.localScale = Vector3.LerpUnclamped(Vector3.one * startingScale, Vector3.zero, scaleCurve.Evaluate(1f-pickupTimer));
            transform.position = Vector3.Slerp(startPosition, playerTransform.position + targetPosition, 1f-pickupTimer);
            transform.Rotate(Vector3.right * (1.2f-pickupTimer) * 720f * Time.fixedDeltaTime);

            if (pickupTimer <= 0f)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            transform.Rotate(randomRot * 20f * Time.fixedDeltaTime);
        }
    }

    public bool PickedUp
    {
        get
        {
            return pickupTimer > 0f;
        }
    }

    public void TriggerPickup(Transform target)
    {
        pickupTimer = 1f;
        startPosition = transform.position;
        startingScale = transform.localScale.x;
        targetPosition = Random.insideUnitSphere;
        playerTransform = target;
    }
}
