using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaWeaponBehaviour : WeaponBehaviour
{
    float radious = 5f;

    protected override void Attack()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, radious, transform.forward, radious, lm);

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
