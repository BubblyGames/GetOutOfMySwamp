using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : DefenseBehaviour
{
    private RaycastHit[] hits;
    private RaycastHit[] enemyTouchBomb;

    // Update is called once per frame
    void Update()
    {
        hits = Physics.SphereCastAll(transform.position, attackRange, transform.forward, attackRange, layerMask);
        enemyTouchBomb= Physics.SphereCastAll(transform.position, 0.25f, transform.forward, 0.25f, layerMask);
        if (enemyTouchBomb.Length > 0)
        {
            Debug.Log("BOMB");
            Attack();
            Destroy(gameObject);
        }
 

    }

    protected override void Attack()
    {
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

