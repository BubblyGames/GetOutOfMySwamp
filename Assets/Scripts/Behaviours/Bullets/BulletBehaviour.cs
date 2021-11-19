using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    protected int damage;
    protected float speed = 50f;
    protected Transform target;
    private int layerMask = 1 << 6;

    [Header("Bullet Destruction")]
    [SerializeField] protected float maxDistance;
    [SerializeField] protected float distanceTravelled;


    [Header("Bullet Effects")]
    [SerializeField] protected int actualEffect;


    void FixedUpdate()
    {
        
        if (distanceTravelled >= maxDistance)
        {
            Destroy(gameObject);
        }
        else
        {

            if (target == null)
            {
                Destroy(gameObject);
            }
            else
            {
                if (target == null)
                {
                    transform.Translate(Time.deltaTime * speed * Vector3.forward);
                    distanceTravelled += Time.deltaTime * speed;
                    return;
                }
                Vector3 direction = target.position - transform.position;
                float distanceThisFrame = speed * Time.fixedDeltaTime;

                transform.Translate(direction.normalized * distanceThisFrame, Space.World);
            }

        }

    }
    virtual public void SetBulletBehaviour(Transform target, int damage, int effect, float range)
    {
        this.damage = damage;
        this.target = target;
        actualEffect = effect;
        maxDistance = range;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            switch (actualEffect)
            {
                case 1:
                    other.gameObject.GetComponent<EnemyBehaviour>().slowAndDamage(damage);
                    Destroy(gameObject);
                    break;
                case 2:
                    other.gameObject.GetComponent<EnemyBehaviour>().AreaDamage(damage, maxDistance, layerMask);
                    Destroy(gameObject);
                    break;
            }
        }
        else if (other.gameObject.CompareTag("World"))
        {
            Destroy(gameObject);
        }
    }

}
