using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*This tower deals damage in all enemys inside its radius of effect */
public class AOEDefenseBehaviour : ShootingDefenseBehaviour
{

    private void Update()
    {
        if (fireCountdown <= 0f)
        {
            Attack();
            fireCountdown = 1f / fireRate;
        }
        fireCountdown -= Time.deltaTime;
    }

    protected override void Attack()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, attackRange, transform.forward, attackRange, layerMask);

        for (int i = 0; i < hits.Length; i++)
        {
            EnemyBehaviour eb;
            if (hits[i].collider.TryGetComponent<EnemyBehaviour>(out eb))
            {
                eb.slowAndDamage(damage);
            }
        }
    }
}
