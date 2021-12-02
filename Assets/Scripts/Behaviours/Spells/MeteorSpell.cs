using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpell : SpellBehaviour
{
    // Start is called before the first frame update
    public GameObject meteorPrefab;
    GameObject meteor;
    public float time = 3f;
    float delta = 0;
    Vector3 startPos;
    public int damage = 30;
    void Start()
    {
        Vector3 dir = transform.position - LevelManager.instance.world.center.position;


        startPos = LevelManager.instance.world.center.position + (50 * dir.normalized);
        meteor = GameObject.Instantiate(meteorPrefab, startPos, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        delta += Time.deltaTime / time;

        meteor.transform.position = Vector3.Lerp(startPos, transform.position, delta);

        if (delta >= 1)
        {

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

            Destroy(meteor);
            Destroy(gameObject);

        }
    }
}
