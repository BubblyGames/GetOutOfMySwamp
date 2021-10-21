using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPool 
{
    public string id;
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;
}
