using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveInfo : MonoBehaviour
{
    public Wave[] waves;

    internal List<bool> enemiesInLevel;
    internal List<bool> defensesInLevel;

    [Header("Types of enemies in this level")]
    public bool basicEn;
    public bool HeavyEn;
    public bool HordeEn;
    public bool SkateEn;
    public bool AerealEn;


    [Header("Types of defenses in this level")]
    public bool basicDef;
    public bool HeavyDef;
    public bool MoneyDef;
    public bool PoisonDef;
    public bool AerealDef;
    public bool Bomb;

    private void Awake()
    {
        enemiesInLevel = new List<bool>();
        defensesInLevel = new List<bool>();
        enemiesInLevel.Add(basicEn);
        enemiesInLevel.Add(HeavyEn);
        enemiesInLevel.Add(AerealEn);
        enemiesInLevel.Add(HordeEn);
        enemiesInLevel.Add(SkateEn);

        defensesInLevel.Add(basicDef);
        defensesInLevel.Add(HeavyDef);
        defensesInLevel.Add(AerealDef);
        defensesInLevel.Add(PoisonDef);
        defensesInLevel.Add(MoneyDef);
        defensesInLevel.Add(Bomb);
    }
}
