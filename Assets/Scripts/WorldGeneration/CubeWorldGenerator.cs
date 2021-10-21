using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

//https://www.youtube.com/watch?v=s5mAf-VMgCM&list=PLcRSafycjWFdYej0h_9sMD6rEUCpa7hDH&index=30

[RequireComponent(typeof(MeshCollider))]
public class CubeWorldGenerator : MonoBehaviour
{
    public int size = 20;
    internal CellInfo[,,] cells; //0 walkable //1 can build //2 can't build //3 target

    internal List<Path> paths;
    public int nPaths = 4;

    [Range(0.0f, 1.0f)]
    public float wallDensity = 0.3f;
    public float rockSize = 3f;
    public float seed = 0f;
    internal Vector3 end;

    VoxelRenderer voxelRenderer;
    MeshCollider meshCollider;

    public static CubeWorldGenerator worldGeneratorInstance;

    private void Awake()
    {
        worldGeneratorInstance = this;
        voxelRenderer = GetComponent<VoxelRenderer>();
        meshCollider = GetComponent<MeshCollider>();
    }

    void Start()
    {
        bool success = false;
        int count = 0;
        if (seed == 0f)
            seed = Mathf.RoundToInt(Random.value * 10000);

        while (!success && count < 100)
        {
            Debug.Log("Attempt: " + count + " Seed: " + seed.ToString());
            success = GenerateWorld();
            seed = Mathf.RoundToInt(Random.value * 10000);
            count++;
        }

        //Add geometry
        MeshData meshData = new MeshData(true);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                for (int k = 0; k < size; k++)
                {
                    if (cells[i, j, k].blockType != BlockType.Air)
                    {
                        cells[i, j, k].normalInt = Vector3Int.zero;

                        if (j + 1 >= size - 1 || (j + 1 < size - 1 && cells[i, j + 1, k].blockType == BlockType.Air))
                        {
                            meshData.AddFace(Direction.Up, i, j, k, cells[i, j, k].blockType);
                            cells[i, j, k].normalInt += Vector3Int.up;
                        }

                        if (j - 1 <= 0 || (j - 1 > 0 && cells[i, j - 1, k].blockType == BlockType.Air))
                        {
                            meshData.AddFace(Direction.Down, i, j, k, cells[i, j, k].blockType);
                            cells[i, j, k].normalInt += Vector3Int.down;
                        }

                        if (i + 1 >= size - 1 || (i + 1 < size - 1 && cells[i + 1, j, k].blockType == BlockType.Air))
                        {
                            meshData.AddFace(Direction.Right, i, j, k, cells[i, j, k].blockType);
                            cells[i, j, k].normalInt += Vector3Int.right;
                        }

                        if (i - 1 <= 0 || (i - 1 > 0 && cells[i - 1, j, k].blockType == BlockType.Air))
                        {
                            meshData.AddFace(Direction.Left, i, j, k, cells[i, j, k].blockType);
                            cells[i, j, k].normalInt += Vector3Int.left;
                        }

                        if (k + 1 >= size - 1 || (k + 1 < size - 1 && cells[i, j, k + 1].blockType == BlockType.Air))
                        {
                            meshData.AddFace(Direction.Front, i, j, k, cells[i, j, k].blockType);
                            cells[i, j, k].normalInt += Vector3Int.forward;
                        }

                        if (k - 1 <= 0 || (k - 1 > 0 && cells[i, j, k - 1].blockType == BlockType.Air))
                        {
                            meshData.AddFace(Direction.Back, i, j, k, cells[i, j, k].blockType);
                            cells[i, j, k].normalInt += Vector3Int.back;
                        }
                    }
                }
            }
        }
        voxelRenderer.RenderMesh(meshData);
    }

    private bool GenerateWorld()
    {
        int endX = size / 2;
        int endY = size - 1;
        int endZ = size / 2;
        end = new Vector3(endX, endY, endZ);

        cells = new CellInfo[size, size, size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                for (int k = 0; k < size; k++)
                {
                    CellInfo cell = new CellInfo(i, j, k);
                    cell.isSurface = CheckIfIsInSurface(cell);
                    if (cell.isSurface)
                        cell.normalInt = GetFaceNormal(cell);

                    //Rock generation
                    float alpha = 1;
                    //float dist = Mathf.Sqrt(2 * size * size) - Mathf.Sqrt(Mathf.Pow(endX - i, 2f) + Mathf.Pow(endY - j, 2f));

                    float horizontalNoise = Mathf.PerlinNoise(seed + (i / rockSize), seed + (j / rockSize));
                    float verticalNoise = Mathf.PerlinNoise(seed + (i / rockSize), seed + (k / rockSize));

                    float perlin = Perlin3D((seed + (i / rockSize)), (seed + (j / rockSize)), (seed + (k / rockSize)));

                    if (cell.isSurface && cell.normalInt.magnitude <= 2 && perlin > (1 - (wallDensity * alpha))) //i == 0 || j == 0 || i == size - 1 || j == size - 1 ||//|| (i == j && i < size - 1)
                    {
                        cell.blockType = BlockType.Rock;
                    }
                    else if (!cell.isSurface)
                    {
                        cell.blockType = BlockType.Grass;
                    }

                    cells[i, j, k] = cell;
                }
            }
        }

        GenerateSwamp(endX, endY, endZ);
        return GeneratePaths(endX, endY, endZ);
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

    public bool CheckIfIsInSurface(CellInfo cell)
    {
        return cell.x == 0 || cell.x == size - 1 ||
            cell.y == 0 || cell.y == size - 1 ||
            cell.z == 0 || cell.z == size - 1;
    }

    public CellInfo GetCell(int x, int y, int z) { return cells[x, y, z]; }

    public CellInfo GetCell(Vector3Int p) { return cells[p.x, p.y, p.z]; }

    private bool GeneratePaths(int endX, int endY, int endZ)
    {
        paths = new List<Path>();

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

            Node p = FindPathAstarWithMidpoints(cells[x, y, z], cells[endX, endY, endZ]);
            if (p != null)
            {
                List<CellInfo> pathCells = new List<CellInfo>();
                while (p != null)
                {
                    CellInfo cell = cells[p.x, p.y, p.z];
                    Vector3Int normal = cell.normalInt;
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
                            while (cellUnder.blockType == BlockType.Air && c < 100)
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

                for (int idx = 0; idx < pathCells.Count - 5; idx++)
                {
                    pathCells[idx].isPath = true;
                }

                /*for (int idx = 0; i < 2; idx++)
                {
                    if (pathCells.Count > 0)
                    {
                        pathCells.RemoveAt(pathCells.Count - 1);
                    }
                }*/

                paths.Add(new Path(pathCells.ToArray()));
            }
        }

        return paths.Count == nPaths;
    }

    public Vector3Int GetFaceNormal(CellInfo cellInfo)
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
            result = Vector3Int.zero;
        }

        return result;
    }

    public Vector3 GetFaceNormal(int x, int y, int z)
    {
        return GetFaceNormal(cells[x, y, z]);
    }

    public BlockType CheckBlockType(int x, int y, int z)
    {
        return cells[x, y, z].blockType;
    }

    Node FindPathRandom(CellInfo start, CellInfo end)
    {
        Node current;
        Node firstNode;

        List<Node> openList = new List<Node>();

        firstNode = new Node(start);
        firstNode.Parent = null;
        openList.Add(firstNode);

        current = firstNode;

        int count = 0;
        while (current.cell.blockType != BlockType.Swamp && count < 1000000)
        {
            //Expands neightbors, (compute cost of each one) and add them to the list
            CellInfo[] neighbours = GetWalkableNeighbours(current.cell);
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

                    if (!IsInOpen)
                    {
                        Node n = new Node(neighbour);
                        n.Parent = current;
                        n.cell = cells[n.x, n.y, n.z];

                        openList.Add(n);
                    }
                }
            }
            current = openList[Mathf.RoundToInt(Random.Range(0, openList.Count))];
            count++;
        }

        Debug.Log(count);

        return current;
    }

    Node FindPathAstar(CellInfo start, CellInfo end)
    {
        Node current;
        Node firstNode;

        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        firstNode = new Node(start);

        //First node, with starting position and null parent
        firstNode.Parent = null;
        firstNode.g = 0;
        firstNode.ComputeFScore(end.x, end.y, end.z);
        openList.Add(firstNode);

        int count = 0;
        while (openList.Count > 0 && count < 100000)
        {
            count++;
            //Sorting the list in "h" in increasing order
            openList = openList.OrderBy(o => o.f).ToList();

            //Check lists's first node
            current = openList[0];
            closedList.Add(current);
            openList.Remove(current);

            //If first node is goal,returns current Node3D
            if (current.cell.blockType == BlockType.Swamp)
            {
                //Debug.Log("Success: " + count.ToString());
                return current;
            }
            else
            {
                //Expands neightbors, (compute cost of each one) and add them to the list
                CellInfo[] neighbours = GetWalkableNeighbours(current.cell);
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
                            n.ComputeFScore(end.x, end.y, end.z);
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
        //Debug.Log("Fail: " + count.ToString());
        return null;
    }

    bool a = true;
    CellInfo GetRandomCell(Node current)
    {
        int x = 1;
        int y = 1;
        int z = 1;

        CellInfo cell = cells[x, y, z];

        //Big ñapa
        while (!CheckIfIsInSurface(cell) || cell.blockType != BlockType.Air)
        {
            if (a)
            {
                x = (size - 1) * Mathf.RoundToInt(Random.value);
                z = Random.Range(current.y, size - 1);
            }
            else
            {
                x = Random.Range(current.y, size - 1);
                z = (size - 1) * Mathf.RoundToInt(Random.value);
            }

            y = Random.Range(current.y, size - 1);

            cell = cells[x, y, z];
        }

        a = !a;
        return cell;
    }

    public int nMidpoints = 1;
    Node FindPathAstarWithMidpoints(CellInfo start, CellInfo end)
    {
        Node current = new Node(start);
        current.ComputeFScore(end.x, end.y, end.z);

        CellInfo midpoint;

        for (int i = 0; i < nMidpoints; i++)
        {
            midpoint = GetRandomCell(current);
            Node result = FindNextStep(current, midpoint);
            int count = 0;
            while (result != null && count < 100)
            {
                midpoint = GetRandomCell(current);
                result = FindNextStep(current, midpoint);
                count++;
            }

            GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = new Vector3(result.x, result.y, result.z);
            current = result;
        }

        //Final step is finding the actual end
        current = FindNextStep(current, end);

        return current;
    }


    Node FindNextStep(Node firstNode, CellInfo end)
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
        while (openList.Count > 0 && count < 100000)
        {
            count++;
            //Sorting the list in "h" in increasing order
            openList = openList.OrderBy(o => o.f).ToList();

            //Check lists's first node
            current = openList[0];
            closedList.Add(current);
            openList.Remove(current);

            if (current.cell == end)//If first node is goal,returns current Node3D
            {
                //Debug.Log("Success: " + count.ToString());
                return current;
            }
            else
            {
                //Expands neightbors, (compute cost of each one) and add them to the list
                CellInfo[] neighbours = GetWalkableNeighbours(current.cell);
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
                            n.ComputeFScore(end.x, end.y, end.z);
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
        //Debug.Log("Fail: " + count.ToString());

        return firstNode;
    }



    private CellInfo[] GetWalkableNeighbours(CellInfo current)
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

                    if (isPosInBounds(x, y, z))
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

    //https://answers.unity.com/questions/938178/3d-perlin-noise.html
    public static float Perlin3D(float x, float y, float z)
    {
        y += 1;
        z += 2;
        float xy = _perlin3DFixed(x, y);
        float xz = _perlin3DFixed(x, z);
        float yz = _perlin3DFixed(y, z);
        float yx = _perlin3DFixed(y, x);
        float zx = _perlin3DFixed(z, x);
        float zy = _perlin3DFixed(z, y);
        return xy * xz * yz * yx * zx * zy;
    }
    static float _perlin3DFixed(float a, float b)
    {
        return Mathf.Sin(Mathf.PI * Mathf.PerlinNoise(a, b));
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
            if (cell.explored)
                Handles.Label(new Vector3(cell.x, cell.y, cell.z), "a");
        }
    }
#endif
}
