using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    protected int damage;
    protected float speed;
    protected Transform target;
    [SerializeField] Vector3 velocity;

    void FixedUpdate()
    {
        //gameObject.transform.Translate(Time.fixedDeltaTime * speed * velocity);
        //gameObject.transform.position += Time.fixedDeltaTime * velocity;
        transform.Translate(Time.fixedDeltaTime*speed*Vector3.forward);
    }
    virtual public void SetBulletBehaviour(Transform target, int damage, float speed)
    {
        this.damage = damage;
        this.speed = speed;
        this.target = target;
        transform.LookAt(target.position);
        velocity = speed * (target.position - transform.position).normalized;
        //GetComponent<Rigidbody>().velocity = velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyBehaviour>().Hurt(damage);
            Destroy(gameObject);
        }
    }

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Enemy"))
        {
            collision.collider.gameObject.GetComponent<EnemyBehaviour>().Hurt(damage);
            Destroy(gameObject);
        }
        else if (collision.collider.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }*/
}
