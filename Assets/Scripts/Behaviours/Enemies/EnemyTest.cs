using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : EnemyBehaviour
{

   
    public override bool Hurt(int damage)
    {
        healthPoints -= damage;
        if (healthPoints < 0)
        {
            return true;
        }

        healthBar.setHealth(healthPoints);
        transform.localScale = Vector3.one * ((float)healthPoints / 10f);

        return false;
    }

    public override void Die()
    {
        Destroy(gameObject);
    }
}
