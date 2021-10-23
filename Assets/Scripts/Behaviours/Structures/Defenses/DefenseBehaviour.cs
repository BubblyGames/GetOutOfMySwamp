using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Basic Defense Class, all kind of defenses from towers to land mines, etc etc, wille inherit from this*/
public abstract class DefenseBehaviour : Structure
{
    [Tooltip("This layer will be used to check for enemys")]
    protected LayerMask enemyLayerMask;

    [Header("Defense Stats")]

    [SerializeField][Tooltip("How much time does it take to reaload between shots")]
    protected float attackWait = 1f;
    
    [Tooltip("Time when the next shot will be shot")]
    protected float nextAttackTime = 0;

    [SerializeField][Tooltip("Damage an attack will deal")]
    protected int damage;

    [Tooltip("Speed of the bullet")]
    protected float bulletSpeed = 1f;

    [Tooltip("The radius of the sphere in which the defense detects an enemy")]
    protected float detectionRange = 5f;


    protected virtual void Attack() { }

    protected void Update()
    {
        if (Time.time > nextAttackTime)
        {
            nextAttackTime = Time.time + attackWait;
            Attack();
        }
    }

    public override void UpgradeStrucrure()
    {
        base.UpgradeStrucrure();

        if (level < maxLevel)
        {
            this.attackWait -= this.attackWait * 0.1f;
            this.damage += Mathf.RoundToInt(this.damage * 0.1f);
        }
    }
}
