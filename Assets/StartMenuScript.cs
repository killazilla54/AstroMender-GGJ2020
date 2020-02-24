using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class StartMenuScript : MonoBehaviour
{

    public int loadScene = 1;

    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(loadScene);
        }
    }
}
