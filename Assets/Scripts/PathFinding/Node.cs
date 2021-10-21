using System;

public class Node
{
    public int x, y, z;
    public int f, h, g;
    public Node Parent;
    public CellInfo cell;

    public Node(CellInfo cell)
    {
        this.cell = cell;
        x = cell.x;
        y = cell.y;
        z = cell.z;
    }

    public void ComputeFScore(int targetX, int targetY, int targetZ)
    {
        h = Math.Abs(targetX - x) + Math.Abs(targetY - y) + Math.Abs(targetX - z);
        if (Parent != null)
            g = Parent.g + 10;
        f = h;
    }


}
