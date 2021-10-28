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

    internal List<WorldInfo> worldList = new List<WorldInfo>();
    public int actualLevel;
    internal WorldInfo worldInfo = null;

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
        AudioManager.instance.Play("mainMenuSong");
    }

    public void SetNextLevelWorld()
    {
        actualLevel++;
        worldInfo=worldList[actualLevel];
    }
}
