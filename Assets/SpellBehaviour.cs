using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBehaviour : Structure
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3Int pos = new Vector3Int();

        pos.x = (int)transform.position.x;
        pos.y = (int)transform.position.y;
        pos.z = (int)transform.position.z;

        LevelManager.instance.world.Explode(pos, 5);

        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
