using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    int damage = 1;

    Vector3 direction;
    Rigidbody rb;
    Vector3 moveQuantity;
    float speed=0.002f;
    bool shooted = false;

    //Radius of effect
    float radious = 5f;
    public LayerMask enemyLayerMask;
    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!shooted)
        {
            rotateBullet();
            shooted = true;
        }
        gameObject.transform.position = gameObject.transform.position+moveQuantity;
    }

    public void SetBulletBehaviour(Vector3 d)
    {
        direction = d;
        moveQuantity = d * speed;
        
    }
    void rotateBullet()
    {
        rb.MoveRotation(Quaternion.LookRotation(direction));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Enemy"))
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, radious, transform.forward, radious, enemyLayerMask);
            for (int i = 0; i < hits.Length; i++)
            {
                collision.collider.gameObject.GetComponent<EnemyBehaviour>().Hurt(damage);
            }
           
            Destroy(gameObject);
        }

        if (collision.collider.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }

    }

}
