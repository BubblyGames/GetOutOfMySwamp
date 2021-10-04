using System;

public class Nodo
{
    public int x;
    public int y;
    public int h;
    public Nodo Parent;
    public CellInfo cell;

    public Nodo(CellInfo cell)
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
