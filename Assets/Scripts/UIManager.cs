using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class UIManager : MonoBehaviour
{
    public Image energyBarOverlay, energyBarGain, energyBarLoss;

    public Color lighterColor, darkerColor, darkBadColor;

    public Animator moduleRepairedAnimator;

    public EnergyModule[] modules;

    //Im so sorry
    public Text scoreText;

    private void Start()
    {
        EnergySystem.instance.SubscribeToEnergyTierDecreasedEvent(HandleEnergyDecreased);
        EnergySystem.instance.SubscribeToEnergyTierIncreasedEvent(HandleEnergyIncreased);
        ScoreController.instance.SubscribeToScoreUpdatedEvent(HandleScoreUpdate);
    }

    private void HandleEnergyDecreased (int tier)
    {
        float percent = EnergySystem.instance.GetCurrentEnergyAmount() / EnergySystem.instance.EnergyTierUpAmount();
        energyBarGain.fillAmount = percent;
        energyBarOverlay.fillAmount = percent;
        energyBarLoss.fillAmount = percent;

        if (tier-1 >= 0 && tier-1 < modules.Length)
        {
            modules[tier-1].DisableModule();
        }
    }

    private IEnumerator DelayModuleRepair(int tier)
    {
        moduleRepairedAnimator.SetTrigger("SystemRepaired");
        yield return new WaitForSeconds(0.3f);
        modules[tier-2].EnableModule();
    }

    private void HandleEnergyIncreased (int tier)
    {
        float percent = EnergySystem.instance.GetCurrentEnergyAmount() / EnergySystem.instance.EnergyTierUpAmount();
        energyBarGain.fillAmount = percent;
        energyBarOverlay.fillAmount = percent;
        energyBarLoss.fillAmount = percent;
        if (tier-2 < modules.Length)
        {
            StartCoroutine(DelayModuleRepair(tier));
        }
    }

    private void Update()
    {
        float percent = EnergySystem.instance.GetCurrentEnergyAmount() / EnergySystem.instance.EnergyTierUpAmount();
        if (percent > energyBarOverlay.fillAmount)
        {
            energyBarGain.fillAmount = percent;
            energyBarOverlay.fillAmount = Mathf.Lerp(energyBarOverlay.fillAmount, energyBarGain.fillAmount, Time.deltaTime * 2f);
            energyBarLoss.fillAmount = energyBarOverlay.fillAmount;
        }
        else
        {
            energyBarOverlay.fillAmount = percent;
            energyBarLoss.fillAmount = Mathf.Lerp(energyBarLoss.fillAmount, energyBarOverlay.fillAmount, Time.deltaTime * 2f);
            energyBarGain.fillAmount = energyBarLoss.fillAmount;
        }
        if(Input.GetKeyDown(KeyCode.G))
        {
            EnergySystem.instance.RefillEnergy(10f);
        }
    }

    public void HandleScoreUpdate(int updatedScore){
        scoreText.text = updatedScore.ToString("D8");
    }

}
