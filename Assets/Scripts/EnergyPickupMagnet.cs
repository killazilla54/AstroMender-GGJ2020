using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPickupMagnet : MonoBehaviour
{
    public ParticleSystem tier3;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Energy"))
        {
            other.GetComponent<EnergyPickup>().TriggerPickup(transform);
            if(other.GetComponent<EnergyPickup>().tier == 3)
                tier3.Play(true);
        }
    }
}
