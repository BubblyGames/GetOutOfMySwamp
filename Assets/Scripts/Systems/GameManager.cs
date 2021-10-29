using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    public EnemyLibrary enemyLibrary;
    public AudioManager audioManager;


    public int currentWorldId = 0;
    //list containing different level stats and enemies waves 
    internal List<WorldInfo> worldList = new List<WorldInfo>();

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

        DontDestroyOnLoad(gameObject);
    }

    void Start() 
    {
        AudioManager.instance.Play("mainMenuSong");//Pls change asap
    }

    public void SetNextLevelWorld(int nextWorld)
    {
        currentWorldId = nextWorld;
    }

    public WorldInfo GetCurrentWorld()
    {
        return worldList[currentWorldId];
    }
}
