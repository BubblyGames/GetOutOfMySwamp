using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSpell : SpellBehaviour
{
    // Start is called before the first frame update 
    //public int size = 5;

    void Start()
    {
        Vector3Int pos = new Vector3Int();

        pos.x = (int)transform.position.x;
        pos.y = (int)transform.position.y;
        pos.z = (int)transform.position.z;

        LevelManager.instance.world.Explode(pos, range);

        GameObject p = GameObject.Instantiate(particles);
        p.transform.position = transform.position;
        p.transform.rotation = transform.rotation;

        if (AudioManager.instance)
            AudioManager.instance.Play("bombSound");

        Destroy(gameObject);
    }
}
