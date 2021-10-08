using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemy;//TODO: select which enemy is going to be spawned
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
        RoundController.roundControllerInstance.AddToActiveEnemies(enemy.GetComponent<EnemyBehaviour>()); // 
        yield return new WaitForSeconds(3f);
    }
}
