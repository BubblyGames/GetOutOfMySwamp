using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartBulletBehaviour : BulletBehaviour
{
    Vector3 initialPos;
    float delta = 0;

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
                rotateBullet();
            }
            else
            {
                target.gameObject.GetComponent<EnemyBehaviour>().Hurt(damage);
                Destroy(gameObject);
            }
        }
    }

    public void SetBulletBehaviour(Transform target, int damage, float speed)
    {
        Debug.Log("aaa");
        this.target = target;
        this.damage = damage;
        this.speed = speed / Vector3.Distance(target.position, transform.position);
        Debug.Log(this.speed);
        initialPos = transform.position;
        rotateBullet();
    }

    void rotateBullet()
    {
        transform.LookAt(target.transform, Vector3.up);
    }
}