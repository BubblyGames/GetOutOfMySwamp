using System;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int x, y, z;
    public int f, h, g;
    public Node Parent;
    public CellInfo cell;
    public bool isFloating = false;
    public Vector3Int normal = Vector3Int.zero;
    public Vector3 dir = Vector3.zero;

    public Midpoint midpoint = null;

    public Vector3 Position
    {
        get { return new Vector3(x, y, z); }
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

        f = (h - g) + extra;
    }


}

public class NodeComparer : IComparer<Node>
{
    // Compares by Height, Length, and Width.
    public int Compare(Node a, Node b)
    {
        return a.f.CompareTo(b.f);
    }
}
