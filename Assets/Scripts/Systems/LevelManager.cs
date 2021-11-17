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
    public LayerMask floorLayer;
    public GameObject waterSplashPrefab;

    public Color waterColor;

    public Color[] colors;

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

        Texture2D t = (Texture2D)GetComponent<MeshRenderer>().material.mainTexture;
        waterColor = t.GetPixel(20, 0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject waterSplash = GameObject.Instantiate(waterSplashPrefab);
            waterSplash.transform.position = world.end;
            waterSplash.GetComponent<ParticleSystem>().startColor = waterColor;
        }
    }


    public void dealDamageToBase(int damageTaken)
    {
        GameObject waterSplash = GameObject.Instantiate(waterSplashPrefab);
        waterSplash.transform.position = world.end;
        waterSplash.GetComponent<ParticleSystem>().startColor = waterColor;

        if (!CheatManager.instance.infiniteHealth)
        {
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
        UIController.instance.ShowMenu(UIController.GameMenu.EndgameMenuLoose);
    }

    public void LevelCompleted()
    {
        OnGameCompleted?.Invoke();
        UIController.instance.ShowMenu(UIController.GameMenu.EndgameMenuWin);
    }

}








