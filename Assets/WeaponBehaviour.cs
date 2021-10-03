using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{
    public float waitTime = 1f;
    float nextTime = 0;
    float radious = 5f;
    int damage = 1;
    public LayerMask lm;

    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextTime)
        {
            nextTime = Time.time + waitTime;
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, radious, transform.forward, radious, lm);
            Debug.Log(hits.Length);

            for (int i = 0; i < hits.Length; i++)
            {
                EnemyBehaviour eb;
                if (hits[i].collider.TryGetComponent<EnemyBehaviour>(out eb))
                {
                    Debug.Log(hits[i].collider.name);
                    eb.Hurt(damage);
                }
            }
        }
    }
}
