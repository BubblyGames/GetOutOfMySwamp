using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Basic Weapon Class, all kind of weapons from towers to land mines, etc etc, wille inherit from this*/
public abstract class WeaponBehaviour : MonoBehaviour
{
    //How much time does it take to reaload between shots
    public float attackWait = 1f;
    
    //Time when the next shot will be shot
    protected float nextAttackTime = 0;

    //Damage an attack will deal
    public int damage = 1;

    //Speed of the bullet
    public float bulletSpeed = 1f;

    //The radius of the sphere in which the weapon detects an enemy
    public float detectionRange = 5f;

    //This layer will be used to check for enemys
    public LayerMask enemyLayerMask;

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
