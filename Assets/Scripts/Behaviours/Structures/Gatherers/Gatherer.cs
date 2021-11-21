using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gatherer : Structure
{
    [Header("Resource Gathering Settings")]
    [SerializeField] [Tooltip("Amount of resurce will be gathered each time")]
    protected int resourceGatheredEachCicle;
    [SerializeField] protected float timeToGatherResource; // Time when the next resources will be gathered
    [SerializeField] protected float timerToGetResource;
    [SerializeField] private float totalResourceGathered;

    public float TotalResourceGathered { get => totalResourceGathered; set => totalResourceGathered = value; }

    public override void UpgradeStrucrure()
    {
        base.UpgradeStrucrure();

        this.timeToGatherResource = this.timeToGatherResource * 0.2f;
        this.resourceGatheredEachCicle += 2;
    }

    public int GetResourceGatheredAmount()
    {
        return resourceGatheredEachCicle;
    }
}
