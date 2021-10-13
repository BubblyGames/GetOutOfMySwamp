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

    [SerializeField] private int startBaseHealthPoints; //Starting Health points the defenders base have
    [SerializeField] private int currentBaseHealthPoints; //Current Health points the defenders base have
    [SerializeField] private int currentMoney = 0; //Amount of money the player can spend

    public GameObject weaponPrefab;
    //public GameObject enemyPrefab;

    //References
    private CubeWorldGenerator world;
    private ScoreSystem scoreSystem;
    private WaveController waveController;

    //Actions
    public event Action OnGameStarted, OnGameLost, OnScoreIncremented;
    //TODO: increment score when killing enemys.

    public Text text;
    public LayerMask floorLayer;
    public Transform center;
    // Start is called before the first frame update

    private void Awake()
    {
        gameInstance = this;
        world = GetComponent<CubeWorldGenerator>();
        waveController = GetComponent<WaveController>();
        scoreSystem = GetComponent<ScoreSystem>();

        center.transform.position = Vector3.one * (world.size / 2); //set center tu middle of the cube
        currentBaseHealthPoints = startBaseHealthPoints; // initialzing 
    }

    private void Start()
    {
        OnGameStarted?.Invoke();
    }

    private void Update()
    {
        //transform.Rotate(Vector3.forward,Time.deltaTime*10);

        text.text = (1 / Time.deltaTime).ToString(); //FpS text

        if (Input.GetMouseButtonDown(0))
        {
            SpawnWeapon();
        }
    }

    public void addMoney(int quantity)
    {
        currentMoney += quantity;
    }

    public void dealDamageToBase(int damageDealt)
    {

        currentBaseHealthPoints -= damageDealt;
        if (currentBaseHealthPoints <= 0)
        {
            //Game Over
            OnGameLost?.Invoke();
            Debug.Log("Game Over");

            // Show Game Over Screen
            //Go to menu


        }
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
                if(rayNormal.x > 0) {
                    normal.x = 1;
                } else
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

            GameObject.Instantiate(weaponPrefab, pos, Quaternion.identity);
        }
    }
}








