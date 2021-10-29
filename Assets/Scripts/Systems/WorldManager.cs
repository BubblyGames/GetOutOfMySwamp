using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CubeWorldGenerator))]
public class WorldManager : MonoBehaviour
{
    public static WorldManager instance;
    CubeWorldGenerator world;
    public int nPaths { get { return world.nPaths; } }
    public List<Path> paths { get { return world.paths; } }
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
        world = GetComponent<CubeWorldGenerator>();
    }

    public Transform GetCenter()
    {
        return world.center;
    }

    public bool IsPosInBounds(int coordX, int coordY, int coordZ)
    {
        return world.IsPosInBounds(coordX, coordY, coordZ);
    }
}
