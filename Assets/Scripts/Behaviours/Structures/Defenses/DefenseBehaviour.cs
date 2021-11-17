using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Basic Defense Class, all kind of defenses from towers to land mines, etc etc, wille inherit from this*/
public abstract class DefenseBehaviour : Structure
{
    [Tooltip("This layer will be used to check for enemys")]
    protected int layerMask = 1 << 6;

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

    public string Target;
    public string Range;
    public string Damage;
    public string FireRate;

    public override void UpgradeStrucrure(UIController uIController)
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
        }
        base.UpgradeStrucrure(uIController);

    }
    protected virtual void Attack() { }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
