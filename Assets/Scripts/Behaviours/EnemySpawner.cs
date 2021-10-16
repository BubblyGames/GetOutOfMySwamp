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
        if (instance ==null)
        {
            instance = this;
        }
        else
        {   
            Destroy(this); // destroy excess instances
        }
    }

    public void SpawnEnemy(string enemyId, Path path)
    {
        WaveController.waveControllerInstance.AddToActiveEnemies();
        GameObject enemyPrefab = GameManager.instance.enemyLibrary.GetPrefabByIdentificator(enemyId);
        Instantiate(enemyPrefab, path.GetStep(0), Quaternion.identity).GetComponent<EnemyBehaviour>().SetInitialState(path);
    }
}
