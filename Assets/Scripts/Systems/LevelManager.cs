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
    public static event Action OnGameStart, OnGameLost, OnGameCompleted, OnWaveCleared;
    public static event Action<int> OnDamageTaken, OnStructureUpgraded;


    //TODO: increment score when killing enemys.
    public LayerMask floorLayer;
    public GameObject waterSplashPrefab;
    public Material waterSplashMaterial;

    public Color[] colors;

    public GameObject meteorPrefab;

    public static bool ready = false;

    Image fadeImage;
    GameObject loadingIcon;

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
        fadeImage = GameObject.Find("FadeImage").GetComponent<Image>();
        loadingIcon = fadeImage.gameObject.transform.GetChild(0).gameObject;

        loadingIcon.SetActive(true);
        fadeImage.enabled = true;

        if (GameManager.instance != null)
        {
            Debug.Log("Loading level: " + GameManager.instance.currentWorldId);
            WorldInfo worldInfo = GameManager.instance.GetCurrentWorld();
            fadeImage.color = worldInfo.themeInfo.backGroundColor;
        }
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

    public void SpawnMeteor()
    {
        CellInfo cell = world.GetRandomCellWithRay();
        GameObject.Instantiate(meteorPrefab, cell.GetPos(), Quaternion.identity);
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

    public void StartGame()
    {
        loadingIcon.SetActive(false);
        StartCoroutine(StartGameCoroutine());
    }

    IEnumerator StartGameCoroutine()
    {
        ready = true;
        Color color = fadeImage.color;

        yield return new WaitForSeconds(0.5f);

        for (float t = 1; t > 0; t -= Time.deltaTime)
        {
            fadeImage.color = new Color(color.r, color.g, color.b, Mathf.Lerp(0, 1, t));
            yield return null;
        }
        fadeImage.enabled = false;

        yield return null;
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

    public void WaveCleared()
    {
        OnWaveCleared?.Invoke();
    }

    public void StructureGotUpgraded(int level)
    {
        OnStructureUpgraded?.Invoke(level);
    }
}








