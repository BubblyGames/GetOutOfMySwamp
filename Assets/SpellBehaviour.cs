using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBehaviour : Structure
{
    // Start is called before the first frame update
    public GameObject particles;
    //public int size = 5;

    private void Awake()
    {
        Size = 5;
    }
    void Start()
    {
        Vector3Int pos = new Vector3Int();

        pos.x = (int)transform.position.x;
        pos.y = (int)transform.position.y;
        pos.z = (int)transform.position.z;

        LevelManager.instance.world.Explode(pos, Size);

        GameObject p = GameObject.Instantiate(particles);
        p.transform.position = transform.position;
        p.transform.rotation = transform.rotation;

        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
