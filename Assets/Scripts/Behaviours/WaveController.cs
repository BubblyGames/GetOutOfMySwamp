using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//TODO: End wave system
[RequireComponent(typeof(EnemySpawner))]
public class WaveController : MonoBehaviour
{
    public static WaveController instance;

    public int activeEnemies = 0;

    public Wave[] waves;

    public float timeBetweenWaves = 5f;
    public float timeBeforeRoundStarts = 3f;
    public float timeVariable;

    public bool isWaveActive;
    public bool isBetweenWaves;
    public bool allWavesCleared;

    public int waveCount; // Wave its being played

    ///public Text waveText;

    EnemySpawner enemySpawner;

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
        enemySpawner = GetComponent<EnemySpawner>();
    }

    public void Start()
    {
        isWaveActive = false;
        isBetweenWaves = true;

        waveCount = 0;

        timeVariable = Time.time + (.5f * timeBeforeRoundStarts);
        LevelManager.instance.OnGameLost += StopWave;
        LevelManager.instance.OnGameLost += LevelCompleted;

    }

    private void LevelCompleted()
    {
        if (LevelStats.instance.currentBaseHealthPoints > 0)
        {
            allWavesCleared = true;
        }
    }

    void Update()
    {
        if (allWavesCleared)
        {
            isBetweenWaves = false;
            isWaveActive = false;

        }
        else if (isBetweenWaves)
        {
            if (Time.time >= timeVariable)
            {
                isBetweenWaves = false;
                isWaveActive = true;
                StartCoroutine("SpawnWave");
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

    public void ReduceActiveEnemies()
    {

        activeEnemies--;
    }

    IEnumerator SpawnWave()
    {
        Wave currentWave = new Wave();
        if (waveCount >= waves.Length)
        {
            LevelManager.instance.levelCompleted();
        }
        else
        {
            currentWave = waves[waveCount];
        }


        for (int i = 0; i < currentWave.enemyAmount; i++)
        {
            int pathId = UnityEngine.Random.Range(0, CubeWorldGenerator.worldGeneratorInstance.nPaths-1);
            enemySpawner.SpawnEnemy(currentWave.enemyId, CubeWorldGenerator.worldGeneratorInstance.paths[pathId]);
            yield return new WaitForSeconds(1f / currentWave.spawnRate);
        }


    }

    void StopWave()
    {
        StopCoroutine("SpawnWave");
    }
}
