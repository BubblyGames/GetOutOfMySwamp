using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;



/*This manager inicialice the game */
//TODO: RENAME TO LEVEL MANAGER
[RequireComponent(typeof(CubeWorldGenerator))]
[RequireComponent(typeof(WaveController))]

public class LevelManager : MonoBehaviour
{

    public static LevelManager instance;

    //References
    [HideInInspector]
    public CubeWorldGenerator world;
    private ScoreSystem scoreSystem;
    private WaveController waveController;
    private LevelStats levelStats;
    private BuildManager buildManager;
    private Shop shop;

    //Actions
    public static event Action OnGameStart, OnGameLost, OnGameCompleted;
    public event Action<int> OnDamageTaken;
    public event Action<int, int> OnEnemyKilled;

    //TODO: increment score when killing enemys.

    public Text text;
    public LayerMask floorLayer;
    public GameObject waterSplashPrefab;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        world = GetComponent<CubeWorldGenerator>();
        waveController = GetComponent<WaveController>();
        scoreSystem = GetComponent<ScoreSystem>();
        levelStats = GetComponent<LevelStats>();
        buildManager = GetComponent<BuildManager>();
        shop = GetComponent<Shop>();

    }

    private void Start()
    {
        OnGameStart?.Invoke();
    }

    private void Update()
    {
        text.text = Mathf.Round((1 / Time.deltaTime)).ToString(); //FpS 

    }


    public void dealDamageToBase(int damageTaken)
    {
        GameObject.Instantiate(waterSplashPrefab).transform.position = world.end;
        if (!LevelStats.instance.infinteHP)
        {
            //LevelStats.levelStatsInstance.ReceiveDamage(damageTaken);
            OnDamageTaken?.Invoke(damageTaken);
        }
        if (LevelStats.instance.CurrentBaseHealthPoints <= 0)
        {
            //Game Over
            OnGameLost?.Invoke();
            Debug.Log("Game Over");

            // Show Game Over Screen
            GameOver();
        }
    }

    public void GameOver()
    {
        UIController.instance.Toggle();
    }

    public void LevelCompleted()
    {
        OnGameCompleted?.Invoke();
        UIController.instance.Toggle();

    }
}








