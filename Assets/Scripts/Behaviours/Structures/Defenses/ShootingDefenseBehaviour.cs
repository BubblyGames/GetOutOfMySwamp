using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShootingDefenseBehaviour : DefenseBehaviour
{
    public GameObject[] shotPositions; //0 front, 1 back, 2 right, 3 left
    public GameObject bulletType;

    GameObject enemyTarget;
    [Header("Bullet Effects")]
    [SerializeField] protected int actualEffect;

    new void Update()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, detectionRange, transform.forward, detectionRange, enemyLayerMask);
        if (hits.Length > 0)
        {
            if (Time.time > nextAttackTime)
            {
                nextAttackTime = Time.time + attackWait;
                {
                    enemyTarget = hits[0].collider.gameObject;
                    EnemyBehaviour eb;
                    if (enemyTarget != null && enemyTarget.TryGetComponent<EnemyBehaviour>(out eb))
                    {
                        Vector3 bulletPos = chooseBulletPos();
                        GameObject bullet = Instantiate(bulletType, bulletPos, Quaternion.identity);
                        BulletBehaviour b = bullet.GetComponent<BulletBehaviour>();
                        b.SetBulletBehaviour(enemyTarget.transform, damage, bulletSpeed, actualEffect, detectionRange);
                    }
                }
            }
        }
    }

    Vector3 chooseBulletPos()
    {
        Vector3 enemyVector = enemyTarget.transform.position - gameObject.transform.position;
        float angle = Mathf.Atan2(enemyVector.x, enemyVector.y) * Mathf.Rad2Deg;
        Transform finalShotPos;

        if (angle > 0)
        {
            if (angle <= 45)
            {
                finalShotPos = shotPositions[0].transform;
            }
            else if (angle >= 135)
            {
                finalShotPos = shotPositions[1].transform;
            }
            else
            {
                finalShotPos = shotPositions[2].transform;
            }
        }
        else
        {
            if (angle >= -45)
            {
                finalShotPos = shotPositions[0].transform;
            }
            else if (angle <= -135)
            {
                finalShotPos = shotPositions[1].transform;
            }
            else
            {
                finalShotPos = shotPositions[3].transform;
            }
        }
        return finalShotPos.position;
    }
}
