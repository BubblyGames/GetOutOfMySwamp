using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;
    private void Awake()
    {
        //if the instance doesnt exists create it
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // destroy excess instances
        }
    }

    public void SpawnEnemy(EnemyType enemyId, Path path)
    {
        //TODO (FINAL BUILD): swap commented line to get prefabs from game manager
        GameObject enemyPrefab;
        if (GameManager.instance)
        {
            enemyPrefab = GameManager.instance.enemyLibrary.GetPrefabByIdentificator(enemyId);
        }
        else
        {
            enemyPrefab = TemporalLibrary.instance.enemyLibrary.GetPrefabByIdentificator(enemyId);
        }

        Instantiate(enemyPrefab, path.GetStep(0), Quaternion.identity).GetComponent<EnemyBehaviour>().SetPath(path);
        WaveController.instance.AddToActiveEnemies(enemyPrefab.GetComponent<EnemyBehaviour>());
    }
}
