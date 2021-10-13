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


    public int activeEnemies = 0;

    public Wave[] waves;

    public float timeBetweenWaves = 5f;
    public float timeBeforeRoundStarts = 3f;
    public float timeVariable;

    public bool isWaveActive;
    public bool isBetweenWaves;


    public int waveCount; // Wave its being played

    ///public Text waveText;

    EnemySpawner enemySpawner;

    private void Awake()
    {
        waveControllerInstance = this;
        enemySpawner = GetComponent<EnemySpawner>();
    }



    public void Start()
    {
        isWaveActive = false;
        isBetweenWaves = true;

        waveCount = 0;

        timeVariable = Time.time + (.5f * timeBeforeRoundStarts);

    }


    void Update()
    {
        if (isBetweenWaves)
        {
            if (Time.time >= timeVariable)
            {
                isBetweenWaves = false;
                isWaveActive = true;
                StartCoroutine(SpawnWave());
                return;
            }
        }
        else if (isWaveActive)
        {
            if (activeEnemies <= 0)
            {
                isBetweenWaves = true;
                isWaveActive = false;

                timeVariable = Time.time + timeBetweenWaves;
                waveCount++;
            }
        }
    }

    public void AddToActiveEnemies()
    {
        activeEnemies++;
    }

    IEnumerator SpawnWave()
    {
        Wave currentWave = waves[waveCount];

        for (int i = 0; i < currentWave.enemyAmount; i++)
        {
            int pathId = UnityEngine.Random.Range(0, CubeWorldGenerator.worldGeneratorInstance.nPaths);
            while (CubeWorldGenerator.worldGeneratorInstance.paths[pathId] == null)
            {
                pathId = UnityEngine.Random.Range(0, CubeWorldGenerator.worldGeneratorInstance.nPaths);
            }
            enemySpawner.SpawnEnemy(currentWave.enemyPrefab, CubeWorldGenerator.worldGeneratorInstance.paths[pathId]);
            yield return new WaitForSeconds(1f / currentWave.spawnRate);
        }

        if (waveCount == waves.Length)
        {
            //After the last wave show end
            //TODO: add end of level
        }
    }
}
