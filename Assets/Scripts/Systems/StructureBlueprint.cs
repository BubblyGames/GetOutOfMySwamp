using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StructureBlueprint
{
    public GameObject structurePrefab;
    public int creationCost;
    public float upgradeMultiplicator;
    public Upgrade[] upgrades;

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

