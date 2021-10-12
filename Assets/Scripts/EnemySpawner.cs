using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public EnemySpawner()
    {

    }

    public void SpawnEnemy(GameObject enemyprefab, GameObject spawnPoint)
    {
        WaveController.waveControllerInstance.AddToActiveEnemies();
        Instantiate(enemyprefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
    }
}
