using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteppedRotator : MonoBehaviour
{

    public float WaitTime = 2f;
    public float InnerWaitTime = 0;

    public float rotationAngle = 45f;
    public float currentAngle = 0;

    public Transform targetRotator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(InnerWaitTime < WaitTime)
        {
            InnerWaitTime+= .1f;
        }
        else
        {
            currentAngle+=rotationAngle;
            targetRotator.rotation = Quaternion.Euler(0, currentAngle, 0);
            if(currentAngle >= 360 )
            {
                currentAngle = 0;
            }
            InnerWaitTime = 0;
        }
    }
}
