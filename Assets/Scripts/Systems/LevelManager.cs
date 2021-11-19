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

    public bool ready = false;

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
    public Material waterSplashMaterial;

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
        colors[0] = t.GetPixel(0, 0);
        colors[1] = t.GetPixel(0, 20);
        colors[2] = t.GetPixel(20, 0);
        colors[3] = t.GetPixel(20, 20);

        waterSplashMaterial.color = colors[2].gamma;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject waterSplash = GameObject.Instantiate(waterSplashPrefab);
            waterSplash.transform.position = world.end;
        }
    }


    public void dealDamageToBase(int damageTaken)
    {
        GameObject waterSplash = GameObject.Instantiate(waterSplashPrefab);
        waterSplash.transform.position = world.end;

        //if (CheatManager.instance != null && CheatManager.instance.infiniteHealth)
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








