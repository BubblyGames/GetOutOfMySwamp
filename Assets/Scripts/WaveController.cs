using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//TODO: End wave system
[RequireComponent(typeof(EnemySpawner))]
public class WaveController : MonoBehaviour
{
    public static WaveController waveControllerInstance;


    public int activeEnemies;

    public Wave[] waves;

    public float timeBetweenWaves = 5f;
    public float timeBeforeRoundStarts = 3f ;
    public float timeVariable;

    public bool isRoundActive;
    public bool isBetweenWaves;
    public bool isRoundStart;

    public int waveCount; // Wave its being played

    public Text waveText;

    EnemySpawner enemySpawner;

    private void Awake()
    {
        waveControllerInstance = this;
    }



    public void Start()
    {
        isRoundActive = false;
        isBetweenWaves = false;
        isRoundStart = true;

        waveCount = 0;

        timeVariable = Time.time + timeBeforeRoundStarts;

        enemySpawner = GetComponent<EnemySpawner>();
    }


    void Update()
    {
        if (isRoundStart)
        {
            if (Time.time >= timeVariable)
            {
                isRoundStart = false;
                isRoundActive = true;
                StartCoroutine(SpawnWave());
                return;
            }
        }
        else if (isBetweenWaves)
        {
            if (Time.time >= timeVariable)
            {
                isBetweenWaves = false;
                isRoundActive = true;
                StartCoroutine(SpawnWave());
                return;
            }
        }
        else if (isRoundActive)
        {
            if (activeEnemies <= 0)
            { 
                isBetweenWaves = true;
                isRoundActive = false;

                timeVariable = Time.time + timeBetweenWaves;
                waveCount++;
            }
        }
    }

    public void AddToActiveEnemies()
    {
        activeEnemies++; ;
    }

    IEnumerator SpawnWave()
    {
        Wave currentWave = waves[waveCount];

        for (int i = 0; i < currentWave.enemyAmount; i++)
        {
            enemySpawner.SpawnEnemy(currentWave.enemyPrefab, WorldGenerator.worldGeneratorInstance.enemySpawnPoint);
            yield return new WaitForSeconds(1f / currentWave.spawnRate);
        }

        if (waveCount == waves.Length)
        {
            //After the last wave show end
            //TODO: add end of level
        }
    }
}
