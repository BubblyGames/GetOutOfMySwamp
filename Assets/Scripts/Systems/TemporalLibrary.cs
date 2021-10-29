using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporalLibrary : MonoBehaviour
{
    public static TemporalLibrary instance;

    [Header("this is only for debugging level scenes")]
    [Header("This wont make it to the final build")]
    public EnemyLibrary enemyLibrary;

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
    }
}
