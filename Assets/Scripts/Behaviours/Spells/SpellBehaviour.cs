using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBehaviour : Structure
{
    // Start is called before the first frame update
    public GameObject particles;
    public int range = 5;

    void Start()
    {
        Destroy(gameObject);
    }

}
