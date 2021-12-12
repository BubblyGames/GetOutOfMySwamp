using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpell : SpellBehaviour
{
    // Start is called before the first frame update
    public GameObject meteorPrefab;
    GameObject meteor;

    public int damage = 30;

    Vector3 startPos;
    //Vector3 targetPos;
    Vector3[] targets;
    int currentTarget = 1;

    Vector3 pt0, pt1, pt2, pt3;

    public float time = 3f;
    float delta = 0;

    private Vector3 _smoothVelocity = Vector3.zero;

    void OldStart()
    {
        targets = new Vector3[2];

        Vector3 dir = transform.position - LevelManager.instance.world.center.position;

        startPos = LevelManager.instance.world.center.position + (100 * dir.normalized);
        targets[0] = startPos;
        targets[1] = transform.position;
        meteor = GameObject.Instantiate(meteorPrefab, startPos, Quaternion.identity);
    }

    public void Start()
    {
        targets = new Vector3[4];


        pt0 = new Vector3(15, 0, 15);//Start
        pt3 = transform.position;//End

        float dist = Vector3.Distance(pt0, pt3);

        Vector3 dir = pt3 - LevelManager.instance.world.center.position;
        pt2 = LevelManager.instance.world.center.position + (50 * dir.normalized);//Point above end



        pt1 = pt2; //Midpoint between start and point above end
        pt1.y = -30;




        //pt1 += dir.normalized * dist;
        //pt1.y = -10;//Start

        meteor = GameObject.Instantiate(meteorPrefab, pt0, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        delta += Time.deltaTime / time;

        //meteor.transform.position = Vector3.Lerp(targets[currentTarget - 1], targets[currentTarget], delta);

        meteor.transform.position = new Vector3(
            X(delta, pt0.x, pt1.x, pt2.x, pt3.x),
            Y(delta, pt0.y, pt1.y, pt2.y, pt3.y),
            Z(delta, pt0.z, pt1.z, pt2.z, pt3.z)
            );

        //meteor.transform.position = Vector3.SmoothDamp(transform.position, targets[currentTarget], ref _smoothVelocity, 10f);//Smooth


        if (delta >= 1)
        {
            /*if (currentTarget < targets.Length - 1)
            {
                delta = 0;
                currentTarget++;
                return;
            }*/

            Vector3Int pos = new Vector3Int();

            pos.x = (int)transform.position.x;
            pos.y = (int)transform.position.y;
            pos.z = (int)transform.position.z;

            LevelManager.instance.world.Explode(pos, range);

            RaycastHit[] hits = Physics.SphereCastAll(transform.position, range, transform.forward, range);
            ////Debug.Log("Boom: " + hits.Length);
            for (int i = 0; i < hits.Length; i++)
            {
                EnemyBehaviour eb;
                if (hits[i].collider.TryGetComponent<EnemyBehaviour>(out eb))
                {
                    eb.slowAndDamage(damage);
                }
            }

            GameObject p = GameObject.Instantiate(particles);
            p.transform.position = transform.position;
            p.transform.rotation = transform.rotation;

            ParticleSystem pa = meteor.transform.GetChild(0).GetComponent<ParticleSystem>();
            pa.loop = false;

            pa.transform.parent = null;

            if (AudioManager.instance)
                AudioManager.instance.Play("bombSound");

            Destroy(meteor);
            Destroy(gameObject);

        }
    }


    private static float X(float t,
    float x0, float x1, float x2, float x3)
    {
        return (float)(
            x0 * Mathf.Pow((1 - t), 3) +
            x1 * 3 * t * Mathf.Pow((1 - t), 2) +
            x2 * 3 * Mathf.Pow(t, 2) * (1 - t) +
            x3 * Mathf.Pow(t, 3)
        );
    }
    private static float Y(float t,
        float y0, float y1, float y2, float y3)
    {
        return (float)(
            y0 * Mathf.Pow((1 - t), 3) +
            y1 * 3 * t * Mathf.Pow((1 - t), 2) +
            y2 * 3 * Mathf.Pow(t, 2) * (1 - t) +
            y3 * Mathf.Pow(t, 3)
        );
    }
    private static float Z(float t,
        float z0, float z1, float z2, float z3)
    {
        return (float)(
            z0 * Mathf.Pow((1 - t), 3) +
            z1 * 3 * t * Mathf.Pow((1 - t), 2) +
            z2 * 3 * Mathf.Pow(t, 2) * (1 - t) +
            z3 * Mathf.Pow(t, 3)
        );
    }
}
