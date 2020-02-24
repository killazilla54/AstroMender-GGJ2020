using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public RectTransform healthbar, healthbarFollow;

    private RectTransform thisRect, parentRect;

    public Image skullDistanceIcon;

    public Image[] bonusHealthBoxes;
    private Outline[] bonusHealthBoxesOutlines;
    private CanvasGroup[] bonusHealthBoxCanvasGroups;

    private CanvasGroup thisCanvasGroup;

    private Transform targetTransform;

    private EnemyHealthComponent healthComponent;

    private void Start()
    {
        bonusHealthBoxesOutlines = new Outline[bonusHealthBoxes.Length];
        bonusHealthBoxCanvasGroups = new CanvasGroup[bonusHealthBoxes.Length];
        for (int i = 0; i < bonusHealthBoxes.Length; i++)
        {
            bonusHealthBoxesOutlines[i] = bonusHealthBoxes[i].GetComponent<Outline>();
            bonusHealthBoxCanvasGroups[i] = bonusHealthBoxes[i].GetComponent<CanvasGroup>();
        }

        thisRect = GetComponent<RectTransform>();
        parentRect = transform.parent.GetComponent<RectTransform>();

        thisCanvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetTotalHealth()
    {
        Debug.Log("Spawning enemy tier " + healthComponent.enemyTier);
        for (int i = 0; i < bonusHealthBoxes.Length; i++)
        {
            if (i < healthComponent.enemyTier)
            {
                bonusHealthBoxes[i].gameObject.SetActive(true);
                bonusHealthBoxes[i].color = Color.white;
                bonusHealthBoxCanvasGroups[i].alpha = 1f;
                bonusHealthBoxes[i].rectTransform.localEulerAngles = Vector3.zero;
                bonusHealthBoxes[i].rectTransform.localScale = Vector3.one;
            }
            else
            {
                bonusHealthBoxes[i].gameObject.SetActive(false);
            }
        }
    }

    public void HandleTakeDamage(int damage)
    {
        healthbar.localScale = new Vector3(healthComponent.CurrentHealthBarPercent,1f,1f);
        healthbarFollow.localScale = new Vector3(healthbarFollow.localScale.x, 2f, 1f);
    }

    public void HandleLoseTier(int tier)
    {
        for (int i = 0; i < bonusHealthBoxes.Length; i++)
        {
            if(i < tier)
            {
                bonusHealthBoxes[i].color = Color.black;
                bonusHealthBoxesOutlines[i].effectColor = Color.white;
            }
        }
        if (healthComponent.CurrentHealthBarPercent == 0f) {
            healthbar.localScale = Vector3.one;
        }
        else
        {
            healthbar.localScale = new Vector3(healthComponent.CurrentHealthBarPercent,1f,1f);
        }
        healthbarFollow.localScale = healthbar.localScale;
    }

    private void Update()
    {
        if (targetTransform == null)
        {
            thisCanvasGroup.alpha = Mathf.Lerp(thisCanvasGroup.alpha, 0f, Time.deltaTime * 4f);
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * 0f, Time.deltaTime * 2f);
            if (thisCanvasGroup.alpha < 0.05f)
            {
                this.enabled = false;
                gameObject.SetActive(false);
            }
            return;
        }

        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(targetTransform.position);
        if(viewportPosition.z < 0 || viewportPosition.z > 80f)
        {
            thisCanvasGroup.alpha = 0f;
            
            if (viewportPosition.z > 0) 
            {
                skullDistanceIcon.color = Color.Lerp(skullDistanceIcon.color, Color.gray, Time.deltaTime);
                skullDistanceIcon.transform.localScale = Vector3.one * (0.8f/transform.localScale.x);
            }
            else
            {
                skullDistanceIcon.color = Color.clear;
            }
        }
        else
        {
            skullDistanceIcon.color = Color.Lerp(skullDistanceIcon.color, Color.clear, Time.deltaTime * 4f);
            thisCanvasGroup.alpha = Mathf.Lerp(thisCanvasGroup.alpha, 1f, Time.deltaTime * 2f);
        }
        viewportPosition = new Vector2(viewportPosition.x, -(1f-viewportPosition.y));
        thisRect.anchoredPosition = Vector2.Scale(viewportPosition, parentRect.sizeDelta);

        healthbarFollow.localScale = Vector3.Lerp(healthbarFollow.localScale, healthbar.localScale, Time.deltaTime * 3f);
        float distance = Vector3.Distance(Camera.main.transform.position, targetTransform.position);
        transform.localScale = Vector3.one * Mathf.Lerp(0.75f, 0.5f, (distance - 50f)/120f);

        for (int i = 0; i < bonusHealthBoxes.Length; i++)
        {
            if (i >= this.healthComponent.CurrentHealthbars - 1)
            {
                // Debug.Log(i+1 + ": " + this.healthComponent.CurrentHealthbars);
                bonusHealthBoxes[i].color = Color.Lerp(bonusHealthBoxes[i].color, Color.white, Time.deltaTime * 2f);
                bonusHealthBoxesOutlines[i].effectColor = Color.Lerp(bonusHealthBoxesOutlines[i].effectColor, Color.black, Time.deltaTime * 2f);
                bonusHealthBoxes[i].rectTransform.localEulerAngles = Vector3.forward * Mathf.Lerp(bonusHealthBoxes[i].rectTransform.eulerAngles.z, 360f, Time.deltaTime * 2f);
                bonusHealthBoxes[i].rectTransform.localScale = Vector3.Lerp(bonusHealthBoxes[i].rectTransform.localScale, Vector3.one*2f, Time.deltaTime * 2f);
                bonusHealthBoxCanvasGroups[i].alpha = Mathf.Lerp(bonusHealthBoxCanvasGroups[i].alpha, 0f, Time.deltaTime*2f);
            }
        }
    }

    public void SetTargetEnemy(EnemyHealthComponent healthComponent)
    {
        this.healthComponent = healthComponent;
        SetTotalHealth();
        targetTransform = healthComponent.transform;
        healthbar.localScale = Vector3.one;
        this.healthComponent.AddTakeDamageListener(HandleTakeDamage);
        this.healthComponent.AddLoseTierListener(HandleLoseTier);
        skullDistanceIcon.color = Color.clear;
    }
}
