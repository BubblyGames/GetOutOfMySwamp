using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This tower deals damage in all enemys inside its radius of effect */
public class AreaWeaponBehaviour : WeaponBehaviour
{
    protected override void Attack()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, detectionRange, transform.forward, detectionRange, enemyLayerMask);

        for (int i = 0; i < hits.Length; i++)
        {
            EnemyBehaviour eb;
            if (hits[i].collider.TryGetComponent<EnemyBehaviour>(out eb))
            {
                eb.Hurt(damage);
            }
        }
    }
}
