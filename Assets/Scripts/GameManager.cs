using System.Collections;
using System.Collections.Generic;
using HarmonyQuest.Util;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PlayerController playerController;
    public EnemyManager enemyManager;
    public ProcObjectManager procObjectManager;
    public UIManager uiManager;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        enemyManager = FindObjectOfType<EnemyManager>();
        uiManager = FindObjectOfType<UIManager>();
    }
}
