using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemy;
    Coroutine spawnCoroutine;
    public EnemySpawner()
    {

    }
    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
    }

    public void StartSpawning()
    {
        spawnCoroutine = StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        RoundController.roundControllerInstance.AddToActiveEnemies(); // 
        yield return WaitForSeconds(3f);
    }
}
