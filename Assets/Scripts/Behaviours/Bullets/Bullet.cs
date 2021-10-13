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
            collision.collider.gameObject.GetComponent<EnemyBehaviour>().Hurt(damage);
            Destroy(gameObject);
        }

        if (collision.collider.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }

    }

}
