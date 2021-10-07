using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBulletBehaviour : MonoBehaviour
{
    int damage = 1;

    Vector3 direction;
    Rigidbody rb;

    float speed = 50;
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
            rb.AddForce(direction * speed);   
            shooted = true;
        }
    }

    public void SetBulletBehaviour(Vector3 d)
    {
        direction = d;
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

    }

}
