using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShootingDefenseBehaviour : DefenseBehaviour
{
    public Transform firePoint;
    [SerializeField] private EnemyBehaviour enemyTarget;
    [SerializeField] public float turnSpeed = 5f;

    [Header("Bullet Atributes")]
    public GameObject bulletPrefab;
    [SerializeField] protected int actualEffect;

    private void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);

        //checks if is a mountainTower for only atacking flying enemies
        if (canHitSkyEnemies)
        {
            layerMask = 9;
        }

    }

    void UpdateTarget()
    {
         RaycastHit[] hits = Physics.SphereCastAll(transform.position, attackRange, transform.forward, attackRange, layerMask);
        if (hits.Length > 0)
        {
            float shortestDistance = Mathf.Infinity;
            EnemyBehaviour nearestEnemy = null;
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.tag =="Enemy")
                {
                    EnemyBehaviour enemy = hit.collider.gameObject.GetComponent<EnemyBehaviour>();
                    float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                    if (distanceToEnemy < shortestDistance)
                    {
                        shortestDistance = distanceToEnemy;
                        nearestEnemy = enemy;
                    }
                }
            }
                

            if (nearestEnemy !=null && shortestDistance <=attackRange)
            {
                enemyTarget = nearestEnemy;
            }
            else
            {
                enemyTarget = null;
            }
        }
    }

    protected void Update()
    {
        if (enemyTarget == null)
        {
            return;
        }

        transform.LookAt(enemyTarget.transform,normal);

        if (fireCountdown <= 0f)
        {
            Attack();
            fireCountdown = 1f / fireRate;
        }
        fireCountdown -= Time.deltaTime;
    }

    protected override void Attack()
    {
        
        BulletBehaviour bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<BulletBehaviour>();
        bullet.SetBulletBehaviour(enemyTarget.gameObject.transform, this.damage, this.actualEffect, this.attackRange);
    }

    public int GetEffect()
    {
        return actualEffect;
    }
}
