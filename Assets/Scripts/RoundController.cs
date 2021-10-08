using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundController : MonoBehaviour
{
    public static RoundController roundControllerInstance;

    public List<EnemyBehaviour> activeEnemies;

    public float timeBetweenWaves = 5f;
    public float timeBeforeRoundStarts = 3f ;
    public float timeVariable;

    public bool isRoundActive;
    public bool isBetweenWaves;
    public bool isRoundStart;

    public int wave;

    EnemySpawner enemySpawner;

    public RoundController()
    {
        isRoundActive = false;
        isBetweenWaves = false;
        isRoundStart = true;

        wave = 1;

        timeVariable = Time.time + timeBeforeRoundStarts;

        enemySpawner = new EnemySpawner();
    }

    public void AddToActiveEnemies(EnemyBehaviour enemy)
    {
        activeEnemies.Add(enemy);
    }

    // Update is called once per frame
    void Update()
    {
        if (isRoundStart)
        {
            if (Time.time >= timeVariable)
            {
                isRoundStart = false;
                isRoundActive = true;
                enemySpawner.StartSpawning();
                return;
            }
        }
        else if (isBetweenWaves)
        {
            if (Time.time >= timeVariable)
            {
                isBetweenWaves = false;
                isRoundActive = true;
                enemySpawner.StartSpawning();
                return;
            }
        }
        else if (isRoundActive)
        {
            if (activeEnemies.Count <= 0)
            { 
                isBetweenWaves = true;
                isRoundActive = false;

                timeVariable = Time.time + timeBetweenWaves;
                wave++;
            }
        }
    }
}
