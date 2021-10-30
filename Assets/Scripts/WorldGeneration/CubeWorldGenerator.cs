using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

//https://www.youtube.com/watch?v=s5mAf-VMgCM&list=PLcRSafycjWFdYej0h_9sMD6rEUCpa7hDH&index=30

[RequireComponent(typeof(VoxelRenderer))]
public class CubeWorldGenerator : MonoBehaviour
{
    public bool demo = false;
    public int size = 21;//Odd numbers look better
    internal CellInfo[,,] cells;

    internal List<Path> paths;
    public int nPaths = 4;

    [Range(0.0f, 1.0f)]
    public float wallDensity = 0.3f;
    [Range(0.0f, 1.0f)]
    public float rocksVisualReduction = 0.9f;
    public float rockSize = 3f;
    public int seed = 0;
    public enum PathMethod
    {
        AStar,
        AStarWithMidpoints
    }
    public PathMethod pathMetod = PathMethod.AStarWithMidpoints;
    public int numberOfMidpoints = 1;

    internal Vector3Int end;
    internal Transform center;

    //Debug stuff
    public bool debugMidpoints = false;
    public GameObject lineRendererPrefab;
    List<GameObject> debugStuff = new List<GameObject>();

    VoxelRenderer voxelRenderer;//Transforms the cells array into a mesh

    private void Awake()
    {
        voxelRenderer = GetComponent<VoxelRenderer>();

        GameObject worldCenter = new GameObject("World center");
        worldCenter.transform.position = transform.position + (Vector3.one * ((size - 1) / 2f)); //set center tu middle of the cube
        worldCenter.transform.parent = transform;
        center = worldCenter.transform;

        if (!demo && GameManager.instance != null)
        {
            Debug.Log("Loading level: " + GameManager.instance.currentWorldId);
            WorldInfo worldInfo = GameManager.instance.GetCurrentWorld();
            nPaths = worldInfo.nPaths;
            wallDensity = worldInfo.wallDensity;
            rocksVisualReduction = worldInfo.rocksVisualReduction;
            rockSize = worldInfo.rockSize;
            numberOfMidpoints = worldInfo.numberOfMidpoints;
            GetComponent<MeshRenderer>().material = worldInfo.themeInfo.material;
            FindObjectOfType<Light>().color = worldInfo.themeInfo.lightColor;
            RenderSettings.skybox.SetColor("_Tint", worldInfo.themeInfo.backGroundColor);
        }
    }

    void Start()
    {
        bool success = false;
        int count = 1;
        if (seed == 0f)//If seed is set to 0 on inspector it chooses a random one
            seed = Mathf.RoundToInt(Random.value * 10000);

        cells = new CellInfo[size, size, size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                for (int k = 0; k < size; k++)
                {
                    //CreatinG cells JUST ONCE
                    cells[i, j, k] = new CellInfo(i, j, k);
                }
            }
        }

        //Paths store a list of cellinfos which the enemies use to navigate
        paths = new List<Path>();
        for (int i = 0; i < nPaths; i++)
        {
            Path p = new Path();
            p.id = i;
            paths.Add(p);
        }

        //While enough paths can't be created, it will try new seeds and restart the process
        while (!success && count < 100)
        {
            Debug.Log("Attempt: " + count + " Seed: " + seed.ToString());
            Random.InitState(seed);
            end = GenerateWorld();//Choose the blocktype of each cell
            if (!demo)
            {
                success = GeneratePaths(end.x, end.y, end.z);//Tries to create paths
                if (!success)
                {
                    ClearDebugStuff();
                    seed = Mathf.RoundToInt(Random.value * 100000);//New seed
                    count++;
                    wallDensity -= 0.1f;
                }
            }
            else
            {
                success = true;
            }
        }

        //Add geometry
        MeshData meshData = GenerateMesh(); //Converts the array into a mesh
        voxelRenderer.RenderMesh(meshData); //Renders mesh
    }

    private Vector3Int GenerateWorld()
    {
        end.x = size / 2;
        end.y = size - 1;
        end.z = size / 2;

        //cells = new CellInfo[size, size, size];
        FillWorld();

        //Debug.Log("World generated");
        GenerateSwamp(end.x, end.y, end.z);
        //Debug.Log("Swamp generated");

        return new Vector3Int(end.x, end.y, end.z);
    }

    void FillWorld()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                for (int k = 0; k < size; k++)
                {
                    CellInfo cell = cells[i, j, k];
                    cell.isSurface = CheckIfIsInSurface(cell);
                    if (cell.isSurface)
                        cell.normalInt = GetFaceNormal(cell);
                    cell.isPath = false;
                    cell.isCloseToPath = false;
                    cell.isInteresting = false;

                    //Rock generation
                    float alpha = 1;
                    //float dist = Mathf.Sqrt(2 * size * size) - Mathf.Sqrt(Mathf.Pow(endX - i, 2f) + Mathf.Pow(endY - j, 2f));

                    if (Vector3.Distance(end, new Vector3(i, j, k)) < size / 4)
                    {
                        alpha = 0;
                    }

                    float perlin = alpha * Perlin3D((seed + (i / rockSize)), (seed + (j / rockSize)), (seed + (k / rockSize)));

                    if (cell.isSurface)
                    {
                        if (perlin > (1 - ((wallDensity * rocksVisualReduction) * alpha)))
                        {
                            cell.canWalk = false;
                            cell.blockType = BlockType.Rock;
                        }
                        else if (perlin > (1 - (wallDensity * alpha)))
                        {
                            cell.canWalk = false;
                            cell.blockType = BlockType.Air;
                        }
                        else
                        {
                            cell.canWalk = true;
                            cell.blockType = BlockType.Air;
                        }
                    }
                    else
                    {
                        cell.blockType = BlockType.Grass;
                        cell.canWalk = false;
                    }
                }
            }
        }
    }

    MeshData GenerateMesh()
    {
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

        return meshData;
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

                cells[endX + i, size - 1, endZ + k].canWalk = true;
                cells[endX + i, size - 2, endZ + k].canWalk = true;
                cells[endX + i, size - 3, endZ + k].canWalk = true;

                cells[endX + i, size - 1, endZ + k].endZone = true;
                cells[endX + i, size - 2, endZ + k].endZone = true;
                cells[endX + i, size - 3, endZ + k].endZone = true;
            }
        }
    }

    private bool GeneratePaths(int endX, int endY, int endZ)
    {
        for (int i = 0; i < nPaths; i++)
        {
            /*if (!paths[i].dirty) //Should check if a path has been modified to update it, but not yet
                continue;*/
            paths[i].dirty = false;

            if (!paths[i].initiated)//First time calculating a path
            {
                //Random start position
                paths[i].start.x = Random.Range(2, size - 3);
                paths[i].start.y = 0;
                paths[i].start.z = Random.Range(2, size - 3);

                //If start position is not ok, find another one
                int count = 0;
                while ((GetCell(paths[i].start).isPath || //Not part of an existing path
                    !GetCell(paths[i].start).canWalk || //Must be normal floor
                    !GetCell(paths[i].start).isSurface || //Must be on surface
                    FindPathAstar(new Node(GetCell(paths[i].start)), cells[endX, endY, endZ], true) == null) //There must be a path to the end
                    && count < 10000)
                {
                    paths[i].start.x = Random.Range(2, size - 3);
                    paths[i].start.z = Random.Range(2, size - 3);
                    count++;
                }

                paths[i].AddMidpoint(new Midpoint(GetCell(paths[i].start), false));
                for (int j = 0; j < numberOfMidpoints; j++)
                {
                    paths[i].AddMidpoint(new Midpoint(GetRandomCell(paths[i].midPoints[j].cell.y), false));
                }
                paths[i].AddMidpoint(new Midpoint(GetCell(end), false));
            }
            //Debug.Log(count + " attempts needed to find starting point");

            //Node which will store the result of the path finding
            Node result;

            switch (pathMetod)
            {
                case PathMethod.AStar:
                    //Normal path to end
                    result = FindPathAstar(new Node(GetCell(paths[i].start)), cells[endX, endY, endZ], true);
                    break;
                case PathMethod.AStarWithMidpoints:
                    //Path to end with midpoints
                    result = FindPathAstarWithMidpoints(GetCell(paths[i].start), cells[endX, endY, endZ], paths[i]);
                    break;
                default:
                    result = null;
                    break;
            }

            //If result is null, a path couldn't be found and returns false so it tries another seed
            if (result == null)
                return false;

            //List of cells in the path
            List<CellInfo> pathCells = new List<CellInfo>();
            while (result != null)
            {
                CellInfo cell = cells[result.x, result.y, result.z];
                Vector3Int normal = cell.normalInt;
                CellInfo cellUnder = cells[result.x - normal.x, result.y - normal.y, result.z - normal.z];

                if (cellUnder.blockType != BlockType.Swamp && cellUnder.blockType != BlockType.Air)
                {
                    cellUnder.blockType = BlockType.Path;
                }
                else
                {
                    int c = 0;
                    while (cellUnder.blockType == BlockType.Air && c < 100)
                    {
                        cell = cellUnder;
                        cellUnder = cells[cell.x - normal.x, cell.y - normal.y, cell.z - normal.z];
                        c++;
                    }

                    if (cellUnder.blockType == BlockType.Swamp)
                    {
                        cell = cellUnder;
                    }
                }
                pathCells.Add(cell);

                cellUnder = cells[cell.x - normal.x, cell.y - normal.y, cell.z - normal.z];
                foreach (CellInfo c in GetNeighbours(cellUnder, true))
                {
                    c.isCloseToPath = true;
                }

                result = result.Parent;
            }

            //Cells are added from last to first, so we reverse the list
            pathCells.Reverse();

            //Send cells to path to store it
            paths[i].SetPath(pathCells.ToArray());

            //Debug paths and midpoints
            if (debugMidpoints)
            {
                GameObject line = GameObject.Instantiate(lineRendererPrefab);
                LineRenderer lr = line.GetComponent<LineRenderer>();
                debugStuff.Add(line);

                foreach (Midpoint m in paths[i].midPoints)
                {
                    GameObject midSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    midSphere.transform.position = new Vector3(m.cell.x, m.cell.y, m.cell.z);
                    debugStuff.Add(midSphere);
                }

                lr.positionCount = pathCells.Count;
                for (int idx = 0; idx < pathCells.Count; idx++)
                {
                    lr.SetPosition(idx, pathCells[idx].GetPos());
                }
            }
        }

        return true; //Success
    }

    Node FindPathAstarWithMidpoints(CellInfo start, CellInfo end, Path path)
    {

        Node current = new Node(start);
        current.ComputeFScore(end.x, end.y, end.z);
        Midpoint midpoint;
        bool lastSept = false;

        for (int i = 1; i < path.midPoints.Count; i++)
        {
            lastSept = i == path.midPoints.Count - 1; //Is this the segment bewteen the last midpoint and the end?

            midpoint = path.midPoints[i];

            //Debug.Log("Finding path to midpoint " + i);
            Node result = FindPathAstar(current, midpoint.cell, lastSept);

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
                midpoint.cell = GetRandomCell(path.midPoints[i - 1].cell.y);
                result = FindPathAstar(current, midpoint.cell, lastSept);
                count++;
            }
            //Debug.Log(count + " attempts needed");

            if (result == null)
            {
                //Debug.Log("Failed to find a way");
                return null;
            }

            //The end of this segment will become the start of the next one
            current = result;
        }

        //Final step is finding the actual end
        return current;
    }

    Node FindPathAstar(Node firstNode, CellInfo end, bool lastStep = false)
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
                CellInfo[] neighbours = GetNeighbours(current.cell);
                foreach (CellInfo neighbour in neighbours)
                {
                    if (!neighbour.canWalk ||
                        (!lastStep && neighbour.endZone) ||
                        neighbour.isInteresting)//||(neighbour.isPath && !neighbour.endZone)
                        continue;

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
                        if (IsInOpen)
                            continue;

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

                            openList.Add(n);
                        }
                    }
                }
            }
        }

        return null;
    }

    bool onXFace = true;
    CellInfo GetRandomCell(int minY = 1)
    {
        int x = 1;
        int y = 1;
        int z = 1;

        CellInfo cell = cells[x, y, z];

        //Debug.Log("Looking for a random midpoint...");
        //BIG ñapa
        int count = 0;
        while (!CheckIfIsInSurface(cell) || !cell.canWalk || cell.isPath)
        {
            if (!onXFace)
            {
                x = (size - 1) * Mathf.RoundToInt(Random.value);
                z = Random.Range(0, size - 1);
            }
            else
            {
                x = Random.Range(0, size - 1);
                z = (size - 1) * Mathf.RoundToInt(Random.value);
            }

            y = Random.Range(minY, size - 2);

            cell = cells[x, y, z];
            count++;
        }
        //Debug.Log("Midpoint found after " + count.ToString() + " attempts");
        onXFace = !onXFace;
        return cell;
    }

    private CellInfo[] GetNeighbours(CellInfo current, bool addCorners = false)
    {
        //Returns an array of cells around the provided cell
        //If it's not the path to the end (lastStep), cells in the end zone can't be returned
        List<CellInfo> result = new List<CellInfo>();
        CellInfo cell;

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                for (int k = -1; k <= 1; k++)
                {
                    int sum = Mathf.Abs(i) + Mathf.Abs(j) + Mathf.Abs(k);
                    if ((sum > 1 && !addCorners) || sum == 0)//Prevents movement in a diagonal and returning same cell
                        continue;

                    int x = current.x + i;
                    int y = current.y + j;
                    int z = current.z + k;

                    if (IsPosInBounds(x, y, z))
                    {
                        cell = cells[x, y, z];
                        result.Add(cell);
                    }
                }
            }
        }
        return result.ToArray();
    }

    public CellInfo GetCell(int x, int y, int z) { return cells[x, y, z]; }

    public bool CheckIfIsInSurface(CellInfo cell)
    {
        return cell.x == 0 || cell.x == size - 1 ||
            cell.y == 0 || cell.y == size - 1 ||
            cell.z == 0 || cell.z == size - 1;
    }

    public CellInfo GetCell(Vector3Int p) { return cells[p.x, p.y, p.z]; }

    public Vector3Int GetFaceNormal(CellInfo cellInfo)
    {
        //Returns the normal of the face in which a cell is placed
        //If a cell is in a corner, it gets the addition of all of the faces it's part of
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

    public bool IsPosInBounds(int coordX, int coordY, int coordZ)
    {
        return coordX >= 0 && coordX < size && coordY >= 0 && coordY < size && coordZ >= 0 && coordZ < size;
    }

    void ClearDebugStuff()
    {
        foreach (GameObject g in debugStuff)
        {
            Destroy(g);
        }
        debugStuff.Clear();
    }

    public bool AddInterestPoint(Vector3Int point)
    {
        int bestP = -1;
        int bestM = -1;
        float minDist = Mathf.Infinity;
        bool found = false;

        //Find the closest existing midpoint
        for (int p = 0; p < paths.Count; p++)
        {
            for (int m = 0; m < paths[p].midPoints.Count; m++)
            {
                float dist = Vector3.Distance(point, paths[p].midPoints[m].cell.GetPos());
                if (dist < minDist)
                {
                    Node n = FindPathAstar(new Node(paths[p].midPoints[m].cell), GetCell(point), true);
                    if (n != null)
                    {
                        //Found it!
                        minDist = dist;
                        bestP = p;
                        bestM = m;
                        found = true;
                    }
                }
            }
        }

        if (found)
        {
            //There's a midpoint close, so the point is added as a midpoint after it
            paths[bestP].midPoints.Insert(bestM + 1, new Midpoint(GetCell(point), true));
        }
        else
        {
            //There are no midpoints close, so the point is added to a random path (shouldn't happen)
            Debug.Log("This should never happen");
            bestP = Mathf.RoundToInt(Random.Range(0, nPaths));
            paths[bestP].midPoints.Add(new Midpoint(GetCell(point), true));
        }


        if (UpdateWorld())
        {
            paths[bestP].dirty = true;//This path need to be recalculated
            cells[(int)point.x, (int)point.y, (int)point.z].isInteresting = true;//This cell has an object on it
            return true;
        }
        return false;
    }

    public bool RemoveInterestPoint(Vector3Int point)
    {
        CellInfo cell = GetCell(point);
        if (cell.isInteresting)
        {
            cell.isInteresting = false;
            return true;
        }
        return false;
    }

    bool UpdateWorld()
    {
        if (cells != null)
        {
            ClearDebugStuff();
            FillWorld();
            GenerateSwamp(end.x, end.y, end.z);

            if (!demo)
            {
                if (!GeneratePaths(end.x, end.y, end.z))
                    return false;
            }

            MeshData meshData = GenerateMesh();
            voxelRenderer.RenderMesh(meshData);
        }
        else { return false; }

        return true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        foreach (CellInfo cell in cells)
        {
            if (cell.isCloseToPath)
                Handles.Label(new Vector3(cell.x, cell.y, cell.z), "1");
            //Gizmos.DrawSphere(new Vector3(cell.x, cell.y, cell.z), .5f);
        }
    }
    private void OnValidate()
    {
        if (!Application.isPlaying) return;
        /*foreach (Path p in paths)
        {
            if (p!= null && p.initiated)
                p.dirty = true;
        }
        UpdateWorld();*/
    }
#endif
}
