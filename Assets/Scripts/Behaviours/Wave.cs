
using UnityEngine;

/*This class saves information about Waves*/
[System.Serializable]
public class Wave
{
    public string enemyId;
    public int enemyAmount; // Number of enemys will be spawned
    public float spawnRate; //Speed between enemy spawns

}
