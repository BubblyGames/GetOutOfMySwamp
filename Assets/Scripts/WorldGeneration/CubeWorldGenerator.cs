using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

//https://www.youtube.com/watch?v=s5mAf-VMgCM&list=PLcRSafycjWFdYej0h_9sMD6rEUCpa7hDH&index=30

[RequireComponent(typeof(BoxCollider))]
public class CubeWorldGenerator : MonoBehaviour
{
    public int size = 20;
    internal CellInfo[,,] cells; //0 walkable //1 can build //2 can't build //3 target

    internal Path[] paths;
    public int nPaths = 4;

    [Range(0.0f, 1.0f)]
    public float wallDensity = 0.3f;
    public float rockSize = 3f;
    public float seed = 0f;

    VoxelRenderer voxelRenderer;
    BoxCollider boxCollider;

    public static CubeWorldGenerator worldGeneratorInstance;

    private void Awake()
    {
        worldGeneratorInstance = this;
        voxelRenderer = GetComponent<VoxelRenderer>();
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.size = new Vector3(size - 2, size - 2, size - 2);
        boxCollider.center = new Vector3((size - 1) / 2f, (size - 1) / 2f, (size - 1) / 2f);
    }

    void Start()
    {
        if (seed == 0f)
            seed = Random.value * 10;
        Debug.Log("Seed: " + seed.ToString());

        int endX = size / 2;
        int endY = size - 1;
        int endZ = size / 2;

        cells = new CellInfo[size, size, size];
        MeshData meshData = new MeshData(true);

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                for (int k = 0; k < size; k++)
                {
                    CellInfo cell = new CellInfo(i, j, k);

                    //Rock generation
                    float alpha = 1;
                    //float dist = Mathf.Sqrt(2 * size * size) - Mathf.Sqrt(Mathf.Pow(endX - i, 2f) + Mathf.Pow(endY - j, 2f));

                    float horizontalNoise = Mathf.PerlinNoise(seed + (i / rockSize), seed + (j / rockSize));
                    float verticalNoise = Mathf.PerlinNoise(seed + (i / rockSize), seed + (k / rockSize));

                    float perlin = Perlin3D((seed + (i / rockSize)), (seed + (j / rockSize)), (seed + (k / rockSize)));

                    if (CheckIfSurface(cell) && perlin > (1 - (wallDensity * alpha))) //i == 0 || j == 0 || i == size - 1 || j == size - 1 ||//|| (i == j && i < size - 1)
                    {
                        cell.blockType = BlockType.Rock;
                    }
                    else if (!CheckIfSurface(cell))
                    {
                        cell.blockType = BlockType.Grass;
                    }

                    cells[i, j, k] = cell;
                }
            }
        }

        GenerateSwamp(endX, endY, endZ);
        GeneratePaths(endX, endY, endZ);

        //Add geometry
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                for (int k = 0; k < size; k++)
                {
                    if (cells[i, j, k].blockType != BlockType.Air)
                    {
                        if (j + 1 >= size - 1 || (j + 1 < size - 1 && cells[i, j + 1, k].blockType == BlockType.Air))
                            meshData.AddFace(Direction.Up, i, j, k, cells[i, j, k].blockType);

                        if (j - 1 <= 0 || (j - 1 > 0 && cells[i, j - 1, k].blockType == BlockType.Air))
                            meshData.AddFace(Direction.Down, i, j, k, cells[i, j, k].blockType);

                        if (i + 1 >= size - 1 || (i + 1 < size - 1 && cells[i + 1, j, k].blockType == BlockType.Air))
                            meshData.AddFace(Direction.Right, i, j, k, cells[i, j, k].blockType);

                        if (i - 1 <= 0 || (i - 1 > 0 && cells[i - 1, j, k].blockType == BlockType.Air))
                            meshData.AddFace(Direction.Left, i, j, k, cells[i, j, k].blockType);

                        if (k + 1 >= size - 1 || (k + 1 < size - 1 && cells[i, j, k + 1].blockType == BlockType.Air))
                            meshData.AddFace(Direction.Front, i, j, k, cells[i, j, k].blockType);

                        if (k - 1 <= 0 || (k - 1 > 0 && cells[i, j, k - 1].blockType == BlockType.Air))
                            meshData.AddFace(Direction.Back, i, j, k, cells[i, j, k].blockType);
                    }
                }
            }
        }
        voxelRenderer.RenderMesh(meshData);
    }

    private void GenerateSwamp(int endX, int endY, int endZ)
    {
        int radius = 3;

        for (int i = -radius; i < radius; i++)
        {
            for (int k = -radius; k < radius; k++)
            {
                cells[endX + i, size - 1, endZ + k].blockType = BlockType.Air;
                cells[endX + i, size - 2, endZ + k].blockType = BlockType.Air;
                cells[endX + i, size - 3, endZ + k].blockType = BlockType.Swamp;
            }
        }
    }

    public bool CheckIfSurface(CellInfo cell)
    {
        return cell.x == 0 || cell.x == size - 1 ||
            cell.y == 0 || cell.y == size - 1 ||
            cell.z == 0 || cell.z == size - 1;
    }

    public CellInfo GetCell(int x, int y, int z) { return cells[x, y, z]; }
    public CellInfo GetCell(Vector3Int p) { return cells[p.x, p.y, p.z]; }

    private void GeneratePaths(int endX, int endY, int endZ)
    {
        paths = new Path[nPaths];

        for (int i = 0; i < nPaths; i++)
        {
            int x = Random.Range(2, size - 3);
            int y = 0;
            int z = Random.Range(2, size - 3);

            int count = 0;
            while ((cells[x, y, z].blockType == BlockType.Path || cells[x, y, z].blockType == BlockType.Rock) && count < 100)
            {
                if (i < nPaths / 2)
                {
                    x = Random.Range(2, size - 3);
                }
                else
                {
                    z = Random.Range(2, size - 3);
                }
                count++;
            }

            cells[x, y + 1, z].blockType = BlockType.Path;

            Node p = FindPath(nPaths, cells[x, y, z], cells[endX, endY, endZ]);
            if (p != null)
            {
                List<CellInfo> pathCells = new List<CellInfo>();
                while (p != null)
                {
                    Vector3Int normal = GetNormal(cells[p.x, p.y, p.z]);
                    CellInfo cell = cells[p.x, p.y, p.z];
                    CellInfo cellUnder = cells[p.x - normal.x, p.y - normal.y, p.z - normal.z];

                    if (cellUnder.blockType != BlockType.Swamp && cellUnder.blockType != BlockType.Air)
                    {
                        cellUnder.blockType = BlockType.Path;
                        cell = cells[p.x, p.y, p.z];
                    }
                    else
                    {
                        if (cellUnder.blockType == BlockType.Air)
                        {
                            int c = 0;
                            while (cellUnder.blockType == BlockType.Air && c<100)
                            {
                                cell = cellUnder;
                                cellUnder = cells[cell.x - normal.x, cell.y - normal.y, cell.z - normal.z];
                                c++;
                            }
                        }

                        if (cellUnder.blockType == BlockType.Swamp)
                        {
                            cell = cellUnder;
                        }
                    }

                    //floor[p.x, p.y].transform.Translate(-Vector3.forward * 0.1f);
                    pathCells.Add(cell);
                    p = p.Parent;
                }

                pathCells.Reverse();

                for (int idx = 0; idx < pathCells.Count-5; idx++)
                {
                    pathCells[idx].isPath = true;
                }

                paths[i] = new Path(pathCells.ToArray());
            }
        }
    }

    public Vector3Int GetNormal(CellInfo cellInfo)
    {
        Vector3Int result = Vector3Int.zero;

        if (cellInfo.x == 0)
            result += Vector3Int.left;

        if (cellInfo.x == size - 1)
            result += Vector3Int.right;

        if (cellInfo.y == 0)
            result += Vector3Int.down;

        if (cellInfo.y == size - 1)
            result += Vector3Int.up;

        if (cellInfo.z == 0)
            result += Vector3Int.back;

        if (cellInfo.z == size - 1)
            result += Vector3Int.forward;

        if (result == Vector3Int.zero)
        {
            result = Vector3Int.up;
        }

        return result;
    }

    public Vector3 GetNormal(int x, int y, int z)
    {
        Vector3Int result = Vector3Int.zero;

        if ( x == 0)
            result += Vector3Int.left;

        if ( x == size - 1)
            result += Vector3Int.right;

        if ( y == 0)
            result += Vector3Int.down;

        if ( y == size - 1)
            result += Vector3Int.up;

        if ( z == 0)
            result += Vector3Int.back;

        if ( z == size - 1)
            result += Vector3Int.forward;

        if (result == Vector3Int.zero)
        {
            result = Vector3Int.up;
        }

        return result;
    }

    public BlockType CheckBlockType(int x, int y, int z)
    {
        return cells[x, y, z].blockType;
    }

    Node FindPath(int nPaths, CellInfo start, CellInfo end)
    {
        Node current;
        Node firstNode;

        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        firstNode = new Node(start);

        //First node, with starting position and null parent
        firstNode.ComputeHScore(end.x, end.y, end.z);
        firstNode.Parent = null;
        openList.Add(firstNode);



        int count = 0;
        while (openList.Count > 0 && count < 1000)
        {
            count++;
            //Sorting the list in "h" in increasing order
            openList = openList.OrderBy(o => o.h).ToList();

            //Check lists's first node
            current = openList[0];
            closedList.Add(current);
            openList.Remove(current);

            //If first node is goal,returns current Node3D
            if (current.cell.blockType == BlockType.Swamp)
            {
                Debug.Log("Success: " + count.ToString());
                return current;
            }
            else
            {
                //Expands neightbors, (compute cost of each one) and add them to the list
                CellInfo[] neighbours = WalkableNeighbours(current.cell);
                foreach (CellInfo neighbour in neighbours)
                {
                    if (neighbour != null)
                    {
                        //if neighbour no esta en open
                        bool IsInOpen = false;
                        foreach (Node nf in openList)
                        {
                            if (nf.cell.id == neighbour.id)
                            {
                                IsInOpen = true;
                                break;
                            }
                        }

                        bool IsInClosed = false;
                        foreach (Node nf in closedList)
                        {
                            if (nf.cell.id == neighbour.id)
                            {
                                IsInClosed = true;
                                break;
                            }
                        }

                        if (!IsInOpen && !IsInClosed)
                        {
                            Node n = new Node(neighbour);
                            n.ComputeHScore(end.x, end.y, end.z);
                            n.Parent = current;
                            n.cell = cells[n.x, n.y, n.z];

                            if (true)//n.h < current.h
                            {
                                openList.Add(n);
                                //floor[n.x, n.y].transform.position = new Vector3(n.x, n.y,-count/200f);
                                //floor[n.x, n.y].transform.Translate(-Vector3.forward);
                            }
                        }
                    }
                }
            }
        }
        Debug.Log("Fail: " + count.ToString());
        return null;
    }

    private CellInfo[] WalkableNeighbours(CellInfo current)
    {
        List<CellInfo> result = new List<CellInfo>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                for (int k = -1; k <= 1; k++)
                {
                    if (Mathf.Abs(i) + Mathf.Abs(j) + Mathf.Abs(k) > 1)
                        continue;

                    int x = current.x + i;
                    int y = current.y + j;
                    int z = current.z + k;

                    if (isPosInBounds(x,y,z))
                    {
                        if (cells[x, y, z].isPath || (cells[x, y, z].blockType != BlockType.Air && cells[x, y, z].blockType != BlockType.Swamp))
                            continue;

                        result.Add(cells[x, y, z]);
                        cells[x, y, z].explored = true;
                    }
                }
            }
        }

        return result.ToArray();
    }

    public float Perlin3D(float x, float y, float z)
    {
        //Get all three Permutations of noise for X, Y and Z
        float XY = Mathf.PerlinNoise(x, y);
        float YZ = Mathf.PerlinNoise(y, z);
        float XZ = Mathf.PerlinNoise(x, z);

        //And their reverses
        float YX = Mathf.PerlinNoise(y, x);
        float ZY = Mathf.PerlinNoise(z, y);
        float ZX = Mathf.PerlinNoise(z, x);

        float XYZ = XY + YZ + XZ + YX + ZY + ZX;
        return XYZ / 6;
    }

    public bool isPosInBounds(int coordX, int coordY, int coordZ)
    {
        return coordX >= 0 && coordX < size && coordY >= 0 && coordY < size && coordZ >= 0 && coordZ < size;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        foreach (CellInfo cell in cells)
        {
            if (cell.isPath)
                Handles.Label(new Vector3(cell.x, cell.y, cell.z), "a");
        }
    }
#endif
}
