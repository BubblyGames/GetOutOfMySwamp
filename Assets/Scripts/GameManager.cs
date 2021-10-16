using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;



/*This manager inicialice the game */
//DONE: SPLIT MAP GENERATION AND PATH GENERATION INTO COMPONENTS 
[RequireComponent(typeof(CubeWorldGenerator))]
[RequireComponent(typeof(WaveController))]

public class GameManager : MonoBehaviour
{

    public static GameManager gameInstance;


    //References
    private CubeWorldGenerator world;
    private ScoreSystem scoreSystem;
    private WaveController waveController;
    private LevelStats levelStats;
    private BuildManager buildManager;
    private Shop shop;

    //Actions
    public event Action OnGameStarted, OnGameLost, OnGameCompleted, OnDamageTaken, OnScoreIncremented;
    //TODO: increment score when killing enemys.

    public Text text;
    public LayerMask floorLayer;
    public Transform center;


    private void Awake()
    {
        gameInstance = this;
        world = GetComponent<CubeWorldGenerator>();
        waveController = GetComponent<WaveController>();
        scoreSystem = GetComponent<ScoreSystem>();
        levelStats = GetComponent<LevelStats>();
        buildManager = GetComponent<BuildManager>();
        shop = GetComponent<Shop>();

        center.transform.position = Vector3.one * (world.size / 2); //set center tu middle of the cube
    }

    private void Start()
    {
        OnGameStarted?.Invoke();
    }

    private void Update()
    {
        text.text = Mathf.Round((1 / Time.deltaTime)).ToString(); //FpS text

        if (Input.GetMouseButtonDown(0))
        {
            SpawnWeapon();
        }
    }


    public void dealDamageToBase(int damageTaken)
    {
        if (!LevelStats.levelStatsInstance.infinteHP)
        {
            LevelStats.levelStatsInstance.ReceiveDamage(damageTaken);
        }
        if (LevelStats.levelStatsInstance.currentBaseHealthPoints <= 0)
        {
            //Game Over
            OnGameLost?.Invoke();
            Debug.Log("Game Over");

            // Show Game Over Screen
            //Go to menu


        }
    }

    public void levelCompleted()
    {
        Debug.Log("levelCompleted");
        OnGameCompleted?.Invoke();

    }

    private void SpawnWeapon()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit))
        {
            Vector3Int pos = new Vector3Int(Mathf.RoundToInt(hit.point.x), Mathf.RoundToInt(hit.point.y), Mathf.RoundToInt(hit.point.z));

            CellInfo cell = world.GetCell(pos);

            Vector3 rayNormal = hit.normal;
            Vector3Int normal = new Vector3Int();

            float x = Mathf.Abs(rayNormal.x);
            float y = Mathf.Abs(rayNormal.y);
            float z = Mathf.Abs(rayNormal.z);

            if (x >= y && x >= z)
            {
                if (rayNormal.x > 0)
                {
                    normal.x = 1;
                }
                else
                {
                    normal.x = -1;
                }
            }
            else if (y >= x && y >= z)
            {
                if (rayNormal.y > 0)
                {
                    normal.y = 1;
                }
                else
                {
                    normal.y = -1;
                }
            }
            else
            {
                if (rayNormal.z > 0)
                {
                    normal.z = 1;
                }
                else
                {
                    normal.z = -1;
                }
            }

            if (cell.blockType == BlockType.Grass)
            {
                pos += normal;
                cell = world.GetCell(pos);
            }

            if (cell.blockType == BlockType.Rock)
            {
                pos += normal;
            }


            if (!BuildManager.buildManagerInstance.canBuild)
                return;

            BuildManager.buildManagerInstance.BuildStructureOn(pos);

        }
    }
}








