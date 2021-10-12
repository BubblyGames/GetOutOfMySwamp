using System;

public class Node3D
{
    public int x, y, z;
    public int h;
    public Node3D Parent;
    public CellInfo3D cell;

    public Node3D(CellInfo3D cell)
    {
        this.cell = cell;
        x = cell.x;
        y = cell.y;
        z = cell.z;
    }

    public void ComputeHScore(int targetX, int targetY, int targetZ)
    {
        h = Math.Abs(targetX - x) + Math.Abs(targetY - y) + Math.Abs(targetX - z);
    }
}
