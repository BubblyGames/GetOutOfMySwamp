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

    public int actualLevel;
    internal WorldInfo worldInfo = null;

    //list containing different level stats and enemies waves 
    internal List<WorldInfo> worldList = new List<WorldInfo>();
    public Wave[][] wavesLevelsList;

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
        DontDestroyOnLoad(instance);
    }

    void Start() 
    {
        AudioManager.instance.Play("mainMenuSong");
    }

    public void SetNextLevelWorld()
    {
        actualLevel++;
        worldInfo=worldList[actualLevel];
    }
}
