using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartBulletBehaviour : BulletBehaviour
{
    Transform target;
    Vector3 initialPos;

    float delta = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target == null)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.position = Vector3.Lerp(initialPos, target.transform.position, delta);
            if (delta < 1)
            {
                delta += Time.fixedDeltaTime * speed;
            }
            else
            {
                delta = 0;
            }
            rotateBullet();
        }
    }

    new public void SetBulletBehaviour(Transform target, int damage, float speed)
    {
        this.target = target;
        this.damage = damage;
        this.speed = speed;
        initialPos = transform.position;
        rotateBullet();
    }

    void rotateBullet()
    {
        transform.LookAt(target.transform, Vector3.up);
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