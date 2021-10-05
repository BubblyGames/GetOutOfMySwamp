using System;

public class Node
{
    public int x;
    public int y;
    public int h;
    public Node Parent;
    public CellInfo cell;

    public Node(CellInfo cell)
    {
        this.cell = cell;
        x = cell.x;
        y = cell.y;
    }

    public void ComputeHScore(int targetX, int targetY)
    {
        h = Math.Abs(targetX - x) + Math.Abs(targetY - y);
    }
}
