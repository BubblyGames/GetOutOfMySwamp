using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CheatManager : MonoBehaviour
{
    public static CheatManager instance;

    public bool infiniteHealth = false;
    public bool infiniteMoney = false;
    public bool enableEnemySpawn = true;

    public EnemyType enemy;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
}

#if UNITY_EDITOR
[CustomEditor(typeof(CheatManager))]
public class CheatManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CheatManager cheatManager = (CheatManager)target;
        DrawDefaultInspector();
        if (GUILayout.Button("Spawn Enemy"))
        {
            WaveController.instance.TestSpawnEnemy(CheatManager.instance.enemy);
        }
    }
}
#endif
