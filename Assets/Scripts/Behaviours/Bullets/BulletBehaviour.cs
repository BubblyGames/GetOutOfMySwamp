using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    protected int damage;
    protected float speed;
    protected Transform target;
    [SerializeField] Vector3 velocity;


    [Header("Bullet Effects")]
    [SerializeField] protected int actualEffect;
    //enum effects {Recoil}; //special effects of bullets

    void FixedUpdate()
    {
        //gameObject.transform.Translate(Time.fixedDeltaTime * speed * velocity);
        //gameObject.transform.position += Time.fixedDeltaTime * velocity;
        transform.Translate(Time.fixedDeltaTime*speed*Vector3.forward);
    }
    virtual public void SetBulletBehaviour(Transform target, int damage, float speed, int effect)
    {
        this.damage = damage;
        this.speed = speed;
        this.target = target;
        transform.LookAt(target.position);
        velocity = speed * (target.position - transform.position).normalized;
        actualEffect = effect;
        //GetComponent<Rigidbody>().velocity = velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {

            switch (actualEffect)
            {
                case 0:
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
