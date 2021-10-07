using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    int damage=3;
    GameObject target;
    Transform initialPos;
    Rigidbody rb;

    Vector3 movement;
    float speed = 0.02f;
    float delta = 0;

    // Start is called before the first frame update
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
            transform.position = Vector3.Lerp(transform.position, target.transform.position, delta);
            if (delta < 1)
            {
                delta += Time.deltaTime * speed;
            }
            else
            {
                delta = 0;
            }
            rotateBullet();
        }
    }

    public void SetBulletBehaviour(GameObject t, Transform pos)
    {
        if (t == null) return;
        target = t;
        initialPos = pos;
        rb.MovePosition(initialPos.position);
    }
    void rotateBullet()
    {
        Vector3 direction = target.transform.position - gameObject.transform.position;
        rb.MoveRotation(Quaternion.LookRotation(direction));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.CompareTag("Enemy")){
            target.GetComponent<EnemyBehaviour>().Hurt(damage);
            Destroy(gameObject);
        }
       
    }
}