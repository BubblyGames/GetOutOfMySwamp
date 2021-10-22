using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Basic Defense Class, all kind of defenses from towers to land mines, etc etc, wille inherit from this*/
public abstract class DefenseBehaviour : Structure
{
    [Tooltip("This layer will be used to check for enemys")]
    public LayerMask enemyLayerMask;

    [Header("Defense Stats")]

    [Tooltip("How much time does it take to reaload between shots")]
    public float attackWait = 1f;
    
    [Tooltip("Time when the next shot will be shot")]
    protected float nextAttackTime = 0;

    [Tooltip("Damage an attack will deal")]
    public int damage = 1;

    [Tooltip("Speed of the bullet")]
    public float bulletSpeed = 1f;

    [Tooltip("The radius of the sphere in which the defense detects an enemy")]
    public float detectionRange = 5f;


    protected virtual void Attack() { }

    protected void Update()
    {
        if (Time.time > nextAttackTime)
        {
            nextAttackTime = Time.time + attackWait;
            Attack();
        }
    }
}
