using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    protected int damage;
    protected float speed = 30f;
    protected Transform target;

    [Header("Bullet Destruction")]
    [SerializeField] protected float maxDistance;
    [SerializeField] protected float distanceTravelled;

    [Header("Bullet Effects")]
    [SerializeField] protected int actualEffect;


    void Update()
    {
        if (distanceTravelled >= maxDistance)
        {
            Destroy(gameObject);
        }
        else
        {
            if (target != null)
            {
                rotateBullet();
            }
            transform.Translate(Time.deltaTime * speed * Vector3.forward);
            distanceTravelled += Time.deltaTime * speed;
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
                default:
                    other.gameObject.GetComponent<EnemyBehaviour>().Hurt(damage);
                    Destroy(gameObject);
                    break;
            }
        }
        else if (other.gameObject.CompareTag("World"))
        {
            Destroy(gameObject);
        }
    }
    void rotateBullet()
    {
        transform.LookAt(target.transform);
    }
}
