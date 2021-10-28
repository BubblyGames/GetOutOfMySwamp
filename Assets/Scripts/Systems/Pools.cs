using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pools : MonoBehaviour
{
    public static Pools instance;
    public ObjectPool[] objectPools;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        for (int i = 0; i < objectPools.Length; i++)
        {
            objectPools[i].pooledObjects = new List<GameObject>();
            GameObject tmp;
            for (int j = 0; j < objectPools[i].amountToPool; j++)
            {
                tmp = Instantiate(objectPools[i].objectToPool);
                tmp.transform.parent = instance.transform;
                tmp.SetActive(false);
                objectPools[i].pooledObjects.Add(tmp);
            }
        }
    }

    public GameObject GetPooledObject(string id)
    {
        ObjectPool pool = Array.Find(objectPools, pool => pool.id == id);
        for(int i = 0; i < pool.amountToPool; i++)
        {
            if(!pool.pooledObjects[i].activeInHierarchy)
            {
                return pool.pooledObjects[i];
            }
        }
        return null;
    }
}
