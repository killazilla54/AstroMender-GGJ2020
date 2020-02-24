using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextBlink : MonoBehaviour
{

    public float WaitTime = 2f;
    public float InnerWaitTime = 0;

    public MeshRenderer myRenderer;

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
            myRenderer.enabled = !myRenderer.enabled;
            InnerWaitTime = 0;
        }
    }
}
