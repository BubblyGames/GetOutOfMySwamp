using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : EnemyBehaviour
{
    public bool invencible;

    public override bool Hurt(int damage)
    {
        if (!invencible)
        {
            this.healthPoints -= damage;
            if (healthPoints < 0)
            {
                Destroy(gameObject);
                return true;
            }

            transform.localScale = Vector3.one * ((float)healthPoints / 10f);

            return false;
        }
        return false;
    }
}
