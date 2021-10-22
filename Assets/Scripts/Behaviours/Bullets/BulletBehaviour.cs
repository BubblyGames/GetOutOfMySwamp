using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    protected int damage;
    protected float speed;
    protected Transform target;

    [Header("Bullet Destruction")]
    private float radius;
    private float distanceTravelled;


    [Header("Bullet Effects")]
    [SerializeField] protected int actualEffect;

    void FixedUpdate()
    {
        if (distanceTravelled >= radius)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.Translate(Time.deltaTime * speed * Vector3.forward);
            distanceTravelled += Time.deltaTime * speed;
        }
 
    }
    virtual public void SetBulletBehaviour(Transform target, int damage, float speed, int effect, float detectionRange)
    {
        this.damage = damage;
        this.speed = speed;
        this.target = target;
        transform.LookAt(target.position);
        actualEffect = effect;
        radius = detectionRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {

            switch (actualEffect)
            {
                case 1:
                    other.gameObject.GetComponent<EnemyBehaviour>().RecoilHurt(damage);
                    Destroy(gameObject);
                    break;
                default:
                    other.gameObject.GetComponent<EnemyBehaviour>().Hurt(damage);
                    Destroy(gameObject);
                    break;   
            }
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("World"))
        {
            Destroy(gameObject);
        }
    }
}
