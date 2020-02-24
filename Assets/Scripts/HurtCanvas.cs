using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HurtCanvas : MonoBehaviour
{
    public Transform hurtScaler;

    public RawImage rawImage1, rawImage2;

    private Rect rect1, rect2;

    public void TakeDamage()
    {
        hurtScaler.localScale = Vector3.one;
        rawImage1.color = new Color(1f,1f,1f,0.2f);
        rawImage2.color = new Color(0f,0f,0f,0.3f);
        rect1 = rawImage1.uvRect;
        rect2 = rawImage2.uvRect;
    }

    private void Update()
    {
        hurtScaler.localScale = Vector3.Lerp(hurtScaler.localScale, Vector3.one * 1.2f, Time.deltaTime);
        rawImage1.color = Color.Lerp(rawImage1.color, Color.clear, Time.deltaTime * 2f);
        rawImage2.color = Color.Lerp(rawImage2.color, Color.clear, Time.deltaTime * 2f);

        rect1.position += Vector2.right * Time.deltaTime * 10f;
        rect2.position += Vector2.up * Time.deltaTime * 10f;
    }
}
