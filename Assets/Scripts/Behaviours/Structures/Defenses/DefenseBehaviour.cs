using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Basic Defense Class, all kind of defenses from towers to land mines, etc etc, wille inherit from this*/
public abstract class DefenseBehaviour : Structure
{
    [Tooltip("This layer will be used to check for enemys")]
    protected int layerMask= 1 << 6;

    [Header("Defense Stats")]

    [SerializeField][Tooltip("Number of shoots per second")]
    protected float fireRate = 1f;
    
    [Tooltip("Time when the next shot will be shot")]
    protected float fireCountdown = 0;

    [SerializeField][Tooltip("Damage an attack will deal")]
    protected int damage;

    [Tooltip("Speed of the bullet")]
    protected float bulletSpeed = 1f;

    [SerializeField][Tooltip("The radius of the sphere in which the defense detects an enemy")]
    protected float attackRange = 5f;

    public override void UpgradeStrucrure()
    {
        base.UpgradeStrucrure();

        if (level < maxLevel)
        {
            this.fireRate -= this.fireRate * 0.1f;
            this.damage += Mathf.RoundToInt(this.damage * 0.1f);
        }
    }
    protected virtual void Attack() { }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
