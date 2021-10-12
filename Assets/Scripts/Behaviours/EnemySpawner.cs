using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public EnemySpawner()
    {

    }

    public void SpawnEnemy(GameObject enemyprefab, Path path)
    {
        WaveController.waveControllerInstance.AddToActiveEnemies();
        Instantiate(enemyprefab, path.GetStep(0), Quaternion.identity).GetComponent<EnemyBehaviour>().SetPath(path);
    }
}
