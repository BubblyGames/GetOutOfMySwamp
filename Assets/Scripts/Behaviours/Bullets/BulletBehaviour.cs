using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    protected int damage;
    public float speed = 50f;
    protected Transform target;
    private int layerMaskId = 1 << 6;
    public LayerMask layerMask;

    [Header("Bullet Destruction")]
    [SerializeField] protected float maxDistance;
    [SerializeField] protected float distanceTravelled;
    [SerializeField] protected float areaEffect;

    [Header("Bullet Effects")]
    [SerializeField] protected int actualEffect;

    public GameObject particles;


    void FixedUpdate()
    {

        if (distanceTravelled >= maxDistance)
        {
            Destroy(gameObject);
        }
        else
        {

            if (target == null)
            {
                Destroy(gameObject);
            }
            else
            {
                if (target == null)
                {
                    transform.Translate(Time.deltaTime * speed * Vector3.forward);
                    distanceTravelled += Time.deltaTime * speed;
                    return;
                }
                Vector3 direction = target.position - transform.position;
                float distanceThisFrame = speed * Time.fixedDeltaTime;

                transform.Translate(direction.normalized * distanceThisFrame, Space.World);
            }

        }

    }
    virtual public void SetBulletBehaviour(Transform target, int damage, int effect, float range)
    {
        this.damage = damage;
        this.target = target;
        actualEffect = effect;
        maxDistance = range;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (particles != null)
                GameObject.Instantiate(particles, transform.position, Quaternion.identity);


            switch (actualEffect)
            {
                case 1:
                    other.gameObject.GetComponent<EnemyBehaviour>().slowAndDamage(damage);
                    Destroy(gameObject);
                    break;
                case 2:
                    //other.gameObject.GetComponent<EnemyBehaviour>().AreaDamage(damage, areaEffect, layerMask);
                    other.gameObject.GetComponent<EnemyBehaviour>().Hurt(damage);
                    Explode();
                    Destroy(gameObject);
                    break;
                default:
                    other.gameObject.GetComponent<EnemyBehaviour>().Hurt(damage);
                    Destroy(gameObject);
                    break;
            }
        }
        else if (other.gameObject.CompareTag("World"))
        {
            if (actualEffect == 2)
                Explode();
            Destroy(gameObject);
        }
    }

    void Explode()
    {

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, areaEffect, transform.forward, areaEffect, layerMask);
        for (int i = 0; i < hits.Length; i++)
        {
            EnemyBehaviour eb;
            if (hits[i].collider.TryGetComponent<EnemyBehaviour>(out eb))
            {
                eb.Hurt(damage / 4);
            }
        }

    }

}
