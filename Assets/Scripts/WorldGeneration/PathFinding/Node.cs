using System;
using UnityEngine;

public class Node
{
    public int x, y, z;
    public int f, h, g;
    public Node Parent;
    public CellInfo cell;
    public Vector3 Position {
        get { return new Vector3(x,y,z); }
    }

    public Node(CellInfo cell)
    {
        this.cell = cell;
        x = cell.x;
        y = cell.y;
        z = cell.z;
    }

    public void ComputeFScore(int targetX, int targetY, int targetZ, int extra = 0)
    {
        h = Math.Abs(targetX - x) + Math.Abs(targetY - y) + Math.Abs(targetZ - z);
        if (Parent != null)
            g = Parent.g + 1;

        f = h - g;
        f += extra;
    }


}
