using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//[System.Serializable]
public class Path
{
    internal CellInfo[] cells;
    public List<Midpoint> midPoints = new List<Midpoint>();
    float spawnWait = 1f;
    float nextSpawnTime = 0;
    public int Length { get { return cells.Length; } }
    public bool dirty = true;
    public bool initiated = false;
    public Vector3Int start = new Vector3Int();
    public Vector3Int end = new Vector3Int();
    public int id = -1;

    CubeWorldGenerator world;

    List<EnemyBehaviour> enemies = new List<EnemyBehaviour>();

    public Path(CubeWorldGenerator world)
    {
        this.world = world;
    }

    public bool FindPath()
    {
        Debug.Log("Finding path");

        end = midPoints[midPoints.Count() - 1].cell.GetPosInt();

        Node result = result = FindPathAstarWithMidpoints(world.GetCell(start), world.GetCell(end));

        //If result is null, a path couldn't be found and returns false so it tries another seed
        if (result == null)
            return false;

        //List of cells in the path
        List<CellInfo> pathCells = new List<CellInfo>();
        while (result != null)
        {
            CellInfo cell = world.cells[result.x, result.y, result.z];

            cell.normalInt = GetNormalOfff(result);

            /*CellInfo cellUnder = world.cells[result.x - cell.normalInt.x, result.y - cell.normalInt.y, result.z - cell.normalInt.z];
            cellUnder.blockType = BlockType.Path;*/

            /*foreach (CellInfo c in world.GetNeighbours(cell))
            {
                c.isCloseToPath = true;

                if (c.blockType != BlockType.Swamp && c.blockType != BlockType.Air)
                    c.blockType = BlockType.Path;
            }*/

            //cellUnder = world.GetCellUnder(cell);
            /*foreach (CellInfo c in world.GetNeighbours(cellUnder, true))
            {
                c.isCloseToPath = true;
            }*/

            cell.isPath = true;
            pathCells.Add(cell);
            cell.paths.Add(this);

            result = result.Parent;
        }

        for (int j = 0; j < 7; j++)
        {
            pathCells[j].isPath = false;
            foreach (CellInfo c in world.GetNeighbours(pathCells[j], true))
            {
                c.isCloseToPath = false;
            }
        }

        //Cells are added from last to first, so we reverse the list
        pathCells.Reverse();

        //Send cells to path to store it
        cells = pathCells.ToArray();

        dirty = false;

        return true;
    }


    public static Node FindPathAstar(CubeWorldGenerator _world, Node firstNode, CellInfo end, bool lastStep = false)
    {
        Node current;

        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        openList = new List<Node>();
        closedList = new List<Node>();

        //First node, with starting position and null parent
        firstNode.ComputeFScore(end.x, end.y, end.z);
        openList.Add(firstNode);

        int count = 0;
        while (openList.Count > 0 && count < 10000)
        {
            count++;
            //Sorting the list in "h" in increasing order
            openList = openList.OrderBy(o => o.f).ToList();

            //Check lists's first node
            current = openList[0];
            closedList.Add(current);
            openList.Remove(current);

            if (current.cell == end || (lastStep && current.cell.blockType == BlockType.Swamp))//If first node is goal,returns current Node3D
            {
                return current;
            }
            else
            {
                //Expands neightbors, (compute cost of each one) and add them to the list
                CellInfo[] neighbours = _world.GetNeighbours(current.cell);

                //If there are no neighbours, try next node in open list
                while (neighbours.Length == 0)
                {
                    openList[0].Parent = current;
                    current = openList[0];
                    closedList.Add(current);
                    openList.Remove(current);
                    neighbours = _world.GetNeighbours(current.cell);
                }

                foreach (CellInfo neighbour in neighbours)
                {
                    if (neighbour == null ||
                        !neighbour.canWalk ||
                        (!lastStep && neighbour.endZone) ||
                        neighbour.isInteresting ||
                        ((neighbour.isPath) && !_world.canMergePaths && !lastStep))//||(neighbour.isPath && !neighbour.endZone)
                        continue;

                    //if neighbour no esta en open
                    bool IsInOpen = false;
                    foreach (Node nf in openList)
                    {
                        if (nf.cell == neighbour)
                        {
                            IsInOpen = true;
                            break;
                        }
                    }
                    if (IsInOpen)
                        continue;

                    bool IsInClosed = false;
                    foreach (Node nf in closedList)
                    {
                        if (nf.cell == neighbour)
                        {
                            IsInClosed = true;
                            break;
                        }
                    }

                    if (!IsInOpen && !IsInClosed)
                    {
                        Node n = new Node(neighbour);

                        n.ComputeFScore(end.x, end.y, end.z);
                        n.Parent = current;
                        n.cell = _world.cells[n.x, n.y, n.z];

                        openList.Add(n);
                    }

                }
            }
        }

        return null;
    }

    Node FindPathAstarWithMidpoints(CellInfo start, CellInfo end)
    {

        Node current = new Node(start);
        current.ComputeFScore(end.x, end.y, end.z);
        Midpoint midpoint;
        bool lastSept = false;

        List<Node> closedList = new List<Node>();
        List<Node> openList = new List<Node>();

        for (int i = 1; i < midPoints.Count; i++)
        {
            lastSept = i == midPoints.Count - 1; //Is this the segment bewteen the last midpoint and the end?

            midpoint = midPoints[i];

            //Debug.Log("Finding path to midpoint " + i);
            Node result = Path.FindPathAstar(world, current, midpoint.cell, lastSept);

            //If a midpoint is important but the path can't be made, the path fails
            if (result == null && midpoint.important)
            {
                Debug.Log("Couldn't get to midpoint " + midpoint.cell.id);
                return null;//Could try to change previous point instead? For beta maybe
            }

            //Tries 10 times to find a suitable midpoint
            int count = 0;
            while (result == null && count < 10)
            {
                midpoint.cell = world.GetRandomCell(midPoints[i - 1].cell.y);
                result = Path.FindPathAstar(world, current, midpoint.cell, lastSept);
                count++;
            }
            //Debug.Log(count + " attempts needed");

            if (result == null)
            {
                //Debug.Log("Failed to find a way");
                return null;
            }

            Node n = result;
            while (n != current)
            {
                n.cell.isPath = true;
                foreach (CellInfo c in world.GetNeighbours(n.cell, true))
                {
                    c.isCloseToPath = true;
                }
                n = n.Parent;
            }

            //The end of this segment will become the start of the next one
            current = result;
        }

        //Final step is finding the actual end
        return current;
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

    public void AddEnemy(EnemyBehaviour enemy)
    {
        enemies.Add(enemy);
    }

    public void Reset()
    {
        foreach (EnemyBehaviour e in enemies)
        {
            e.FindNewPath();
        }

        foreach (CellInfo c in cells)
        {
            c.paths.Remove(this);
            c.Reset();
        }

        dirty = true;

        enemies.Clear();
    }

    float value = .5f;
    Vector3Int GetNormalOf(Node n)
    {
        Vector3Int result = Vector3Int.zero;

        Vector3 dir = n.Position - world.center.position;

        return Vec3ToInt(dir.normalized);
    }

    Vector3Int GetNormalOff(Node n)
    {
        if (n.Parent == null)
            return Vector3Int.down;

        Vector3Int result = Vector3Int.zero;


        Vector3 down = n.Position - world.center.position;

        Vector3 dirToParent = n.Position - n.Parent.Position;

        Vector3 right = Vector3.Cross(down, dirToParent);

        Vector3 dir = Vector3.Cross(dirToParent, right);

        return Vec3ToInt(dir.normalized);
    }


    Vector3Int GetNormalOfff(Node n)
    {
        if (n.Parent == null)
            return Vector3Int.down;

        Vector3 result = Vector3.zero;

        CellInfo[] parentNeighbours = world.GetNeighbours(n.Parent.cell, false);

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                for (int k = -1; k <= 1; k++)
                {
                    int sum = Mathf.Abs(i) + Mathf.Abs(j) + Mathf.Abs(k);
                    if (sum > 1)//Prevents movement in a diagonal and returning same cell
                        continue;

                    int x = n.x + i;
                    int y = n.y + j;
                    int z = n.z + k;

                    if (!world.IsPosInBounds(x, y, z))
                        continue;

                    foreach (CellInfo cell in parentNeighbours)
                    {
                        if (world.cells[x, y, z] == cell)
                        {
                            result += new Vector3Int(-i, -j, -k);

                            if (world.cells[x, y, z].blockType != BlockType.Swamp && world.cells[x, y, z].blockType != BlockType.Air)
                                world.cells[x, y, z].blockType = BlockType.Path;
                        }
                    }
                }
            }
        }

        return Vec3ToInt(result.normalized);
    }


    Vector3Int Vec3ToInt(Vector3 dir)
    {
        Vector3Int result = Vector3Int.zero;

        if (dir.x > value)
            result.x = 1;

        if (dir.x < -value)
            result.x = -1;

        if (dir.y > value)
            result.y = 1;

        if (dir.y < -value)
            result.y = -1;

        if (dir.z > value)
            result.z = 1;

        if (dir.z < -value)
            result.z = -1;

        return result;
    }
}