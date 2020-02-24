using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyGroup : MonoBehaviour
{
    Transform[] childTransforms;
    GameObject[] energyPickups;

    public GameObject energyPickupPrefab;

    public void Start()
    {
        transform.forward = GameManager.instance.playerController.transform.position - transform.position;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, Random.Range(0.0f, 360.0f));

        childTransforms = transform.GetComponentsInChildren<Transform>();
        energyPickups = new GameObject[childTransforms.Length];

        for (int i = 0; i < childTransforms.Length; i++)
        {
            energyPickups[i] = Instantiate(energyPickupPrefab);
            energyPickups[i].transform.position = childTransforms[i].position;
            energyPickups[i].transform.parent = transform;
        }
    }
}
