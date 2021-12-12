
using UnityEngine;

/*This class saves information about Waves*/
[System.Serializable]
public class Wave
{
    public Pack[] packs;
    //public float spawnRate; //Speed between enemy spawns

}

[System.Serializable]
public class Pack
{
    public float spawnTime;
    public EnemyType enemyType;
    public int enemyAmount; // Number of enemys will be spawned
    public float spawnRate = 1;

    //[HideInInspector]
    public bool spawned;
}