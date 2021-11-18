using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//[System.Serializable]
public class Path
{
    internal CellInfo[] cells = new CellInfo[0];
    public List<Midpoint> midPoints = new List<Midpoint>();
    float spawnWait = 1f;
    float nextSpawnTime = 0;
    public int Length
    {
        get
        {
            if (cells != null)
                return cells.Length;
            else
                return 0;
        }
    }

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
        start = midPoints[0].cell.GetPosInt();
        end = midPoints[midPoints.Count() - 1].cell.GetPosInt();

        Node result = FindPathAstarWithMidpoints(world.GetCell(start), world.GetCell(end));

        //If result is null, a path couldn't be found and returns false so it tries another seed
        if (result == null)
            return false;

        result.normal = Vector3Int.up;

        /*CellInfo[] neighbours = world.GetNeighbours(world.cells[result.x, result.y, result.z]);
        lastCell = neighbours[0];*/

        lastCell = world.GetCellUnder(result.cell);

        //List of cells in the path
        List<CellInfo> pathCells = new List<CellInfo>();
        //midPoints.Clear();
        while (result != null)
        {
            CellInfo cell = world.cells[result.x, result.y, result.z];
            cell.normalInt = GetNormalOf(cell);

            CellInfo cellUnder = world.GetCellUnder(cell);
            if (cell.endZone && cellUnder.blockType == BlockType.Air)
            {
                cellUnder.normalInt = cell.normalInt;
                cell = cellUnder;
            }

            cell.isPath = true;
            pathCells.Add(cell);
            cell.paths.Add(this);

            result = result.Parent;
        }

        for (int j = 0; j < 7; j++)
        {
            pathCells[j].isPath = false;
        }

        //Cells are added from last to first, so we reverse the list
        pathCells.Reverse();

        //Send cells to path to store it
        cells = pathCells.ToArray();

        dirty = false;

        return true;
    }

    public static Node FindPathAstar(CubeWorldGenerator _world, Node firstNode, CellInfo end, bool lastStep = false, List<Node> excludedNodes = null)
    {
        Node current;

        NodeComparer nodeComparer = new NodeComparer();
        //SortedSet<Node> openList = new SortedSet<Node>(nodeComparer);
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        if (excludedNodes != null)
            closedList.AddRange(excludedNodes);

        //SortedSet<Node> sortedList = new SortedSet<Node>();

        //First node, with starting position and null parent
        firstNode.ComputeFScore(end.x, end.y, end.z);
        openList.Add(firstNode);

        int count = 0;
        while (openList.Count > 0 && count < 10000)
        {
            count++;
            //Sorting the list in "h" in increasing order
            openList.Sort(nodeComparer);

            //Check lists's first node
            //current = openList.Min;
            current = openList[0];
            closedList.Add(current);
            openList.Remove(current);

            if (current.cell == end)//If first node is goal,returns current Node3D
            {
                return current;
            }
            else
            {
                //Expands neightbors, (compute cost of each one) and add them to the list
                CellInfo[] neighbours = _world.GetNeighbours(current.cell);
                //CellInfo[] neighboursWitCorners = _world.GetNeighbours(current.cell, true);

                current.isFloating = true;
                for (int i = 0; i < neighbours.Length; i++)
                {
                    if (neighbours[i].blockType != BlockType.Air || current.cell.endZone)
                    {
                        current.isFloating = false;
                        break;
                    }
                }

                if (current.isFloating && current.Parent != null && current.Parent.isFloating)
                    continue;

                foreach (CellInfo neighbour in neighbours)
                {
                    if (neighbour == null ||
                        !neighbour.canWalk ||
                        (!lastStep && neighbour.endZone))//||(neighbour.isPath && !neighbour.endZone)
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

                        n.cell = _world.cells[n.x, n.y, n.z];
                        n.Parent = current;
                        n.ComputeFScore(end.x, end.y, end.z);

                        openList.Add(n);
                    }

                }
            }
        }

        return null;
    }

    Node FindPathAstarWithMidpoints(CellInfo start, CellInfo end)
    {

        List<Node> closedList = new List<Node>();

        Node current = new Node(start);
        current.midpoint = midPoints[0];
        current.ComputeFScore(end.x, end.y, end.z);

        Midpoint midpoint;
        bool lastSept = false;

        for (int i = 1; i < midPoints.Count; i++)
        {
            lastSept = i == midPoints.Count - 1; //Is this the segment bewteen the last midpoint and the end?

            Node result = null;
            midpoint = midPoints[i];

            int count = 0;
            //Tries 10 times to find a suitable midpoint
            while (result == null && count < 10)
            {
                int count2 = 0;
                CellInfo c;
                while ((world.CheckIfFloating(midpoint.cell) || !midpoint.cell.canWalk) && count2 < 10)
                {
                    c = world.GetCellUnder(midpoint.cell);
                    c.normalInt = midpoint.cell.normalInt;
                    midpoint.cell = c;
                    count2++;
                }

                result = Path.FindPathAstar(world, current, midpoint.cell, lastSept, closedList);//

                if (result == null)
                {
                    Debug.Log("This should never happen");

                    //If a midpoint is important but the path can't be made, the path fails
                    if (midpoint.important)
                    {
                        Debug.Log("Couldn't get to midpoint " + midpoint.cell.id + " (" + i + ")");
                        return null;
                    }

                    midpoint.cell = world.GetCompletelyRandomCell();
                }
                count++;
            }
            //Debug.Log(count + " attempts needed");

            if (result == null)
            {
                //Debug.Log("Failed to find a way");
                return null;
            }

            Node n = result;
            n.midpoint = midpoint;

            count = 0;
            int length = result.g - current.g;
            Debug.Log("Segment " + i + " has a length of " + length);

            while (n != current)
            {
                closedList.Add(n);
                n.cell.isPath = true;
                foreach (CellInfo c in world.GetNeighbours(n.cell, true))
                {
                    c.isCloseToPath = true;
                }
                n = n.Parent;
                count++;
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



    CellInfo lastCell;
    Vector3Int GetNormalOf(CellInfo c)
    {
        if (c.endZone)
        {
            lastCell = world.GetCell(c.GetPosInt() + Vector3Int.down);
            return Vector3Int.up;
        }

        Vector3Int result = Vector3Int.zero;
        CellInfo[] neighbours = world.GetNeighbours(c);
        CellInfo[] _neigbours;

        foreach (CellInfo n in neighbours)
        {
            if (n.blockType == BlockType.Air)
                continue;

            _neigbours = world.GetNeighbours(n);

            foreach (CellInfo _n in _neigbours)
            {
                if (_n.blockType != BlockType.Air && _n == lastCell && _n != c)
                {
                    result = c.GetPosInt() - n.GetPosInt();
                    lastCell = n;
                    //GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = lastCell.GetPos();
                    return result;
                }
            }
        }

        result = c.GetPosInt() - lastCell.GetPosInt();


        neighbours = world.GetNeighbours(c, true);

        int best = -1;
        float minDist = Mathf.Infinity;

        //Sometimes all neighbours are air and can't find the best option
        for (int i = 0; i < neighbours.Length; i++)
        {
            if (neighbours[i].blockType != BlockType.Air)
            {
                float dist = Vector3.Distance(c.GetPos(), neighbours[i].GetPos());
                if (dist < minDist)
                {
                    best = i;
                    minDist = dist;
                }
            }
        }

        if (best == -1)
        {
            best = UnityEngine.Random.Range(0, neighbours.Length - 1);
        }

        lastCell = neighbours[best];

        return result;
    }



}