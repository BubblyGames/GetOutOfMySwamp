using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBulletBehaviour : MonoBehaviour
{
    int damage = 3;
    GameObject target;
    Transform initialPos;
    Transform finalPos;
    Rigidbody rb;

    Vector3 movement;
    float speed = 50;
    float delta = 0;

    bool shooted = false;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);

        }
        else
        {
            if (!shooted)
            {
                rotateBullet();
                Vector3 forceD = finalPos.position - initialPos.position;
                rb.AddForce(forceD*speed);
                shooted = true;
            }
        }
    }

    public void SetBulletBehaviour(GameObject t, Transform pos)
    {
        if (t == null) return;
        target = t;
        initialPos = pos;
        finalPos = target.transform;
        rb.MovePosition(initialPos.position);

    }
    void rotateBullet()
    {
        Vector3 direction = finalPos.transform.position - gameObject.transform.position;
        rb.MoveRotation(Quaternion.LookRotation(direction));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject == target)
        {
            target.GetComponent<EnemyBehaviour>().Hurt(damage);
            Destroy(gameObject);
        }
    }

}
