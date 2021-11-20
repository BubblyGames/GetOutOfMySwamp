using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : DefenseBehaviour
{
    public bool canBreakWorld = false;
    public GameObject explosionParticles;
    public void Explode()
    {

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, attackRange, transform.forward, attackRange, layerMask);
        ////Debug.Log("Boom: " + hits.Length);
        for (int i = 0; i < hits.Length; i++)
        {
            EnemyBehaviour eb;
            if (hits[i].collider.TryGetComponent<EnemyBehaviour>(out eb))
            {
                eb.slowAndDamage(damage);
            }
        }

        GameObject.Instantiate(explosionParticles,transform.position,Quaternion.identity);

        if (canBreakWorld)
            LevelManager.instance.world.Explode(Vector3Int.RoundToInt(transform.position), 3);

        Destroy(gameObject);
    }
}

