using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyModule : MonoBehaviour
{
    public Image cross, icon, bg;
    public Outline outline;

    [HideInInspector]
    public bool moduleEnabled;

    private Color whiteClear = new Color(1f,1f,1f,0f);

    private Color dimicon = new Color(0.8f,0.8f,0.8f,1f);

    private void Start()
    {
        icon.color = dimicon;
    }

    public void EnableModule()
    {
        moduleEnabled = true;
        bg.color = Color.white;
        outline.effectColor = Color.green;
        transform.localScale = Vector3.one * 1.5f;
        icon.color = Color.white;
    }

    public void DisableModule()
    {
        moduleEnabled = false;
        cross.color = Color.black;
        icon.color = dimicon;
        cross.transform.localScale = Vector3.one * 3f;
        transform.localScale = Vector3.one * 0.75f;
    }

    private void Update()
    {
        if (moduleEnabled)
        {
            cross.color = Color.Lerp(cross.color, whiteClear, Time.deltaTime * 2f);
            cross.transform.localScale = Vector3.Lerp(cross.transform.localScale, Vector3.one * 10f, Time.deltaTime * 2f);
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * 2f);
            bg.color = Color.Lerp(bg.color, GameManager.instance.uiManager.darkerColor, Time.deltaTime*2f);
            outline.effectColor = Color.Lerp(outline.effectColor, GameManager.instance.uiManager.lighterColor, Time.deltaTime * 2f);
        }
        else
        {
            cross.color = Color.Lerp(cross.color, GameManager.instance.uiManager.darkerColor, Time.deltaTime * 2f);
            cross.transform.localScale = Vector3.Lerp(cross.transform.localScale, Vector3.one, Time.deltaTime * 2f);
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * 2f);
            bg.color = Color.Lerp(bg.color, Color.black, Time.deltaTime*2f);
            outline.effectColor = Color.Lerp(outline.effectColor, GameManager.instance.uiManager.darkerColor, Time.deltaTime * 2f);
        }
    }
}
