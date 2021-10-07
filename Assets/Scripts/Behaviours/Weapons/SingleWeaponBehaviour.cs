using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleWeaponBehaviour : WeaponBehaviour
{
    public float waitTime = 0.5f;
    float radious = 4f;
    public LayerMask lm;

    public GameObject bulletType;
    float targetDistance;
    GameObject enermyTargeted;
   
    void Update()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, radious, transform.forward, radious, lm);
        if (hits.Length > 0)
        {
            enermyTargeted = hits[0].collider.gameObject;
            targetDistance = (enermyTargeted.transform.position - gameObject.transform.position).magnitude;
            if (Time.time > nextAttackTime)
            {
                nextAttackTime = Time.time + waitTime;

                {
                    GameObject enemy = hits[0].collider.gameObject;
                    if (enemy != null)
                    {
                        GameObject bullet = Instantiate(bulletType, gameObject.transform.position, Quaternion.identity);
                        bullet.GetComponent<BulletBehaviour>().SetBulletBehaviour(enemy, gameObject.transform);

                    }
                }

            }
        }
    }
}
