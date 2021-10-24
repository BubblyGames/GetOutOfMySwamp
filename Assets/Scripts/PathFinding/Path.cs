using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Path
{
    CellInfo[] cells3D;
    float spawnWait = 1f;
    float nextSpawnTime = 0;
    public int Length { get { return cells3D.Length; } }


    public Path(CellInfo[] cellInfos)
    {
        cells3D = cellInfos;
    }

    public Vector3 GetStep(int idx) { return new Vector3(cells3D[idx].x, cells3D[idx].y, cells3D[idx].z); }

    public bool CheckSpawn()
    {
        if (Time.time > nextSpawnTime)
        {
            nextSpawnTime = Time.time + spawnWait;
            return true;
        }
        return false;
    }

    internal CellInfo GetCell(int idx)
    {
        return cells3D[idx];
    }
}