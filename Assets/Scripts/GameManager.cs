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

public class GameManager : MonoBehaviour
{

    public static GameManager instance;


    //THINGS TO MOVE TO FUUTRE GAMEMANAGER
    [SerializeField]
    public EnemyLibrary enemyLibrary;

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
    public GameObject waterSplashPrefab;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        world = GetComponent<CubeWorldGenerator>();
        waveController = GetComponent<WaveController>();
        scoreSystem = GetComponent<ScoreSystem>();
        levelStats = GetComponent<LevelStats>();
        buildManager = GetComponent<BuildManager>();
        shop = GetComponent<Shop>();

        center.transform.position = Vector3.one * ((world.size-1) / 2f); //set center tu middle of the cube
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "World") {
                    SpawnWeapon(hit);
                }
                else
                {
                    //Interact with existing weapons
                }
            }
        }
    }


    public void dealDamageToBase(int damageTaken)
    {
        GameObject.Instantiate(waterSplashPrefab).transform.position = world.end;
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

    private void SpawnWeapon(RaycastHit hit)
    {
        Vector3 pos = hit.point;
        pos -= hit.normal / 2;

        Vector3Int intPos = new Vector3Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z));

        CellInfo cell = world.GetCell(intPos);

        if (cell.blockType == BlockType.Swamp) return;

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
            cell = world.GetCell(intPos);
        }

        if (cell.blockType == BlockType.Rock)
        {
            pos += normal;
        }

        intPos += normal;

        if (!BuildManager.buildManagerInstance.canBuild)
            return;

        BuildManager.buildManagerInstance.BuildStructureOn(intPos);

    }
}








