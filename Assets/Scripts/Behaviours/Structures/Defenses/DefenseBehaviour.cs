using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*Basic Defense Class, all kind of defenses from towers to land mines, etc etc, wille inherit from this*/
public abstract class DefenseBehaviour : Structure
{
    [Tooltip("This layer will be used to check for enemys")]
    protected int layerMask = 1 << 7;

    [SerializeField]
    [Tooltip("True if can hit enemies that are flying")]
    protected bool canHitSkyEnemies;

    [Header("Defense Stats")]

    [SerializeField]
    [Tooltip("Number of shoots per second")]
    protected float fireRate = 1f;

    [Tooltip("Time when the next shot will be shot")]
    protected float fireCountdown = 0;

    [SerializeField]
    [Tooltip("Damage an attack will deal")]
    protected int damage;

    [Tooltip("Speed of the bullet")]
    protected float bulletSpeed = 1f;

    [SerializeField]
    [Tooltip("The radius of the sphere in which the defense detects an enemy")]
    internal float attackRange = 5f;

    [SerializeField]
    [Tooltip("How much gold will be spent each second in keeping this defense active")]
    protected int maintenanceCost = 1;
    protected float maintenanceCountdown = 1f;

    protected bool isWorking = true;
    [SerializeField]
    protected Image notWorkingImage;

    public override void UpgradeStrucrure()
    {

        if (!isMaxLevel)
        {
            foreach (Stats stats in Blueprint.upgrades[level].stats)
            {
                switch (stats.statToUpgrade)
                {
                    case Stat.attackDamage:
                        this.damage += (int)stats.upgradeAddedValue;
                        break;
                    case Stat.attackSpeed:
                        this.fireRate += stats.upgradeAddedValue;
                        break;
                    case Stat.range:
                        this.attackRange += stats.upgradeAddedValue;
                        break;
                    default:
                        break;
                }
            }
            maintenanceCost += Blueprint.upgrades[level].maintenanceCostIncrease;
        }
        base.UpgradeStrucrure();

    }
    protected virtual void Attack() { }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
#endif

    private void FixedUpdate()
    {
        maintenanceCountdown -= Time.deltaTime;
        if (maintenanceCountdown <= 0f)
        {
            TowerMaintenance();
            maintenanceCountdown = 1f;
        }
    }

    private void TowerMaintenance()
    {
        if (CheatManager.instance.infiniteMoney)
            return;

        if (LevelStats.instance.currentMoney >= maintenanceCost)
        {
            LevelStats.instance.SpendMoney(maintenanceCost);
            isWorking = true;
            if (notWorkingImage != null)
                notWorkingImage.enabled = false;
        }
        else
        {
            isWorking = false;
            if (notWorkingImage != null)
                notWorkingImage.enabled = true;
        }
    }
}
