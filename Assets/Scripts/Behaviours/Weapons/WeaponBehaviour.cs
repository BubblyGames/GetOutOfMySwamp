using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Basic Weapon Class, all kind of weapons from towers to land mines, etc etc, wille inherit from this*/
public abstract class WeaponBehaviour : MonoBehaviour
{
    //How many attacks are shot each second
    public float attackSpeed = 1f;
    
    //Time when the next shot will be shot
    protected float nextAttackTime = 0;

    //Damage an attack will deal
    protected int damage = 1;

    //This layer will be used to check for enemys
    public LayerMask enemyLayerMask;

    protected virtual void Attack() { }

    protected void Update()
    {
        if (Time.time > nextAttackTime)
        {
            nextAttackTime = Time.time + attackSpeed;
            Attack();
        }
    }
}
