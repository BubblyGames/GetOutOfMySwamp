using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StructureBlueprint
{
    public GameObject structurePrefab;
    [SerializeField]
    private int creationCost;
    public int CreationCost
    {
        get
        {
            if (structurePrefab.TryGetComponent<MoneyGatherer>(out MoneyGatherer moneyGatherer))
                return creationCost + Mathf.RoundToInt(0.65f * creationCost * LevelStats.instance.numberOfSpeculios * LevelStats.instance.numberOfSpeculios);

            return creationCost;
        }
    }
    public float upgradeMultiplicator;
    public Upgrade[] upgrades;
    public int[] sellValues;

    public string targetDescription;
    public string rangeDescription;
    public string damageDescription;
    public string fireRateDescription;

}

[System.Serializable]
public class Upgrade
{
    public int cost;
    public int maintenanceCostIncrease;
    public Stats[] stats;

}

[System.Serializable]
public class Stats
{
    public Stat statToUpgrade;
    public float upgradeAddedValue;
}

public enum Stat
{
    attackDamage,
    attackSpeed,
    range
}

