using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffLootTableConfig : MonoBehaviour
{

    public List<BuffMod> availableMods;
    [SerializeField]
    
    void Start(){
        EnergySystem.instance.BuildLootTable(availableMods);
    }
}
