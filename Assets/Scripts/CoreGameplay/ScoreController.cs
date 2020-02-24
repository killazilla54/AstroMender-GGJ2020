using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    private int score;
    public static ScoreController instance;

    public delegate void ScoreEvent(int newScore);
    public ScoreEvent ScoreUpdatedEvent;

    // Start is called before the first frame update
    void OnEnable() {
        instance = this;
        EnergySystem.instance.SubscribeToEnergyCollectedEvent(RegisterEnergyPickup);
    }

    public void SubscribeToScoreUpdatedEvent(ScoreEvent e){
        ScoreUpdatedEvent += e;
    }

    public void RegisterEnemyDestroyed(){
        UpdateScore(10);
    }

    public void RegisterEnergyPickup(int amt){
        UpdateScore(amt);
    }

    private void UpdateScore(int amt){
        score += amt;
        ScoreUpdatedEvent(score);
    }
}
