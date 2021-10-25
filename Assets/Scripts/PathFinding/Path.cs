using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Path
{
    CellInfo[] cells;
    public List<Midpoint> midPoints = new List<Midpoint>();
    float spawnWait = 1f;
    float nextSpawnTime = 0;
    public int Length { get { return cells.Length; } }

    public Path()
    {
        
    }

    public void SetPath(CellInfo[] cellInfos)
    {
        cells = cellInfos;
    }

    public void AddMidpoint(Midpoint midpoint)
    {
        midPoints.Add(midpoint);
    }

    public Vector3 GetStep(int idx) { return new Vector3(cells[idx].x, cells[idx].y, cells[idx].z); }

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
        return cells[idx];
    }
}