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
    public bool canMergePaths = false;

    [Range(0.0f, 1.0f)]
    public float wallDensity = 0.3f;
    [Range(0.0f, 1.0f)]
    public float rocksVisualReduction = 0.9f;
    public float rockSize = 3f;
    public int seed = 0;

    public int numberOfMidpoints = 1;

    internal Vector3Int end;
    internal Transform center;

    //Debug stuff
    public bool debugMidpoints = false;
    public GameObject lineRendererPrefab;
    List<GameObject> debugStuff = new List<GameObject>();

    VoxelRenderer voxelRenderer;//Transforms the cells array into a mesh

    int endzoneRadious;

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
            canMergePaths = worldInfo.canMergePaths;
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
        endzoneRadious = size / 4;

        bool success = false;
        int count = 0;
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
            Path p = new Path(this);
            p.id = i;
            paths.Add(p);
        }

        //While enough paths can't be created, it will try new seeds and restart the process
        while (!success && count < 100)
        {
            count++;
            Debug.Log("Attempt: " + count + " Seed: " + seed.ToString());
            end = GenerateWorld();//Choose the blocktype of each cell
            if (!demo)
            {
                foreach (Path p in paths)
                {
                    p.dirty = true;
                }

                success = GeneratePaths(end.x, end.y, end.z);//Tries to create paths
                if (!success)
                {
                    ClearDebugStuff();
                    seed = Mathf.RoundToInt(Random.value * 100000);//New seed
                    wallDensity -= 0.05f;
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
        end.y = size - 3;
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
                    /*if (cell.isSurface)
                        cell.normalInt = GetFaceNormal(cell);*/
                    cell.isPath = false;
                    cell.isCloseToPath = false;
                    cell.isInteresting = false;

                    //Rock generation
                    float alpha = 1;
                    //float dist = Mathf.Sqrt(2 * size * size) - Mathf.Sqrt(Mathf.Pow(endX - i, 2f) + Mathf.Pow(endY - j, 2f));

                    if (Vector3.Distance(end, new Vector3(i, j, k)) < endzoneRadious)
                    {
                        alpha = 0;
                    }

                    float perlin = alpha * Perlin3D((seed + (i / rockSize)), (seed + (j / rockSize)), (seed + (k / rockSize)));


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
                        if (cell.isSurface)
                        {
                            cell.canWalk = true;
                            cell.blockType = BlockType.Air;
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
    }

    bool IsCore(int i)
    {
        int radius = size / 4;
        int mid = 1 + (size / 2);

        return i < mid + radius && i > mid - radius;
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
                        if (j + 1 >= size - 1 || (j + 1 < size - 1 && cells[i, j + 1, k].blockType == BlockType.Air))
                        {
                            meshData.AddFace(Direction.Up, i, j, k, cells[i, j, k].blockType);
                        }

                        if (j - 1 <= 0 || (j - 1 > 0 && cells[i, j - 1, k].blockType == BlockType.Air))
                        {
                            meshData.AddFace(Direction.Down, i, j, k, cells[i, j, k].blockType);
                        }

                        if (i + 1 >= size - 1 || (i + 1 < size - 1 && cells[i + 1, j, k].blockType == BlockType.Air))
                        {
                            meshData.AddFace(Direction.Right, i, j, k, cells[i, j, k].blockType);
                        }

                        if (i - 1 <= 0 || (i - 1 > 0 && cells[i - 1, j, k].blockType == BlockType.Air))
                        {
                            meshData.AddFace(Direction.Left, i, j, k, cells[i, j, k].blockType);
                        }

                        if (k + 1 >= size - 1 || (k + 1 < size - 1 && cells[i, j, k + 1].blockType == BlockType.Air))
                        {
                            meshData.AddFace(Direction.Front, i, j, k, cells[i, j, k].blockType);
                        }

                        if (k - 1 <= 0 || (k - 1 > 0 && cells[i, j, k - 1].blockType == BlockType.Air))
                        {
                            meshData.AddFace(Direction.Back, i, j, k, cells[i, j, k].blockType);
                        }
                    }
                }
            }
        }

        return meshData;
    }

    private void GenerateSwamp(int endX, int endY, int endZ)
    {
        for (int i = -endzoneRadious; i < endzoneRadious; i++)
        {
            for (int j = size - 1; j > 0; j--)
            {
                for (int k = -endzoneRadious; k < endzoneRadious; k++)
                {
                    cells[endX + i, j, endZ + k].endZone = true;

                    if (Mathf.Abs(i) > endzoneRadious - 4 || Mathf.Abs(k) > endzoneRadious - 4)
                        continue;

                    if (j > size - 3)
                    {
                        cells[endX + i, j, endZ + k].blockType = BlockType.Air;
                        cells[endX + i, j, endZ + k].canWalk = true;
                    }
                    else if (j == size - 3)
                    {
                        cells[endX + i, j, endZ + k].blockType = BlockType.Swamp;
                        cells[endX + i, j, endZ + k].canWalk = true;
                    }
                    else
                    {
                        cells[endX + i, j, endZ + k].blockType = BlockType.Rock;
                    }
                }
            }
        }
    }

    private bool GeneratePaths(int endX, int endY, int endZ)
    {
        float startTime = Time.realtimeSinceStartup;

        for (int i = 0; i < nPaths; i++)
        {
            if (!paths[i].dirty) //Should check if a path has been modified to update it, but not yet
                continue;

            if (!paths[i].initiated)//First time calculating a path
            {
                //Random start position
                paths[i].start.x = Random.Range(endX - endzoneRadious, endX + endzoneRadious);
                paths[i].start.y = 0;
                paths[i].start.z = Random.Range(endX - endzoneRadious, endX + endzoneRadious);

                //If start position is not ok, find another one
                int count = 0;
                while ((GetCell(paths[i].start).isPath || //Not part of an existing path
                    !GetCell(paths[i].start).canWalk || //Must be normal floor
                    !GetCell(paths[i].start).isSurface || //Must be on surface
                    Path.FindPathAstar(this, new Node(GetCell(paths[i].start)), cells[endX, endY, endZ], true) == null) //There must be a path to the end
                    && count < 100)
                {
                    paths[i].start.x = Random.Range(endX - endzoneRadious, endX + endzoneRadious);
                    paths[i].start.z = Random.Range(endX - endzoneRadious, endX + endzoneRadious);
                    count++;
                }

                if (count >= 100)
                    return false;

                paths[i].AddMidpoint(new Midpoint(GetCell(paths[i].start), false));
                for (int j = 0; j < numberOfMidpoints; j++)
                {
                    paths[i].AddMidpoint(new Midpoint(GetRandomCell(paths[i].midPoints[j].cell.y), false));
                }
                paths[i].AddMidpoint(new Midpoint(GetCell(end), false));
                paths[i].initiated = true;
            }
            //Debug.Log(count + " attempts needed to find starting point");

            //Node which will store the result of the path finding

            paths[i].FindPath();
        }

        //Debug paths and midpoints
        if (debugMidpoints)
        {
            for (int i = 0; i < nPaths; i++)
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

                lr.positionCount = paths[i].Length;
                for (int idx = 0; idx < lr.positionCount; idx++)
                {
                    lr.SetPosition(idx, paths[i].GetCell(idx).GetPos());
                }
            }
        }

        Debug.Log("Generating paths took: " + (Time.realtimeSinceStartup - startTime) + "s");
        return true; //Success
    }





    bool onXFace = true;
    public CellInfo GetRandomCell(int minY = 1)
    {
        int x = 1;
        int y = 1;
        int z = 1;

        CellInfo cell = cells[x, y, z];

        int min = 10;
        int max = size - 11;
        //Debug.Log("Looking for a random midpoint...");
        //BIG ñapa
        int count = 0;
        while (!CheckIfIsInSurface(cell) || !cell.canWalk || cell.isPath)
        {
            if (!onXFace)
            {
                x = (size - 1) * Mathf.RoundToInt(Random.value);
                z = Random.Range(min, max);
            }
            else
            {
                x = Random.Range(min, max);
                z = (size - 1) * Mathf.RoundToInt(Random.value);
            }

            y = Random.Range(minY + 1, Mathf.Min(minY + min, size - 2));

            cell = cells[x, y, z];
            count++;
        }
        //Debug.Log("Midpoint found after " + count.ToString() + " attempts");
        onXFace = !onXFace;
        return cell;
    }

    public CellInfo[] GetNeighbours(CellInfo current, bool addCorners = false)
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

    float a = .5f;
    public CellInfo GetCellUnder(CellInfo cell)
    {
        /*Vector3 normal = new Vector3(
            cell.x - center.position.x,
            cell.y - center.position.y,
            cell.z - center.position.z);

        normal.Normalize();

        Vector3Int normalInt = Vector3Int.zero;

        if (normal.x >= a)
            normalInt.x = 1;

        if (normal.y >= a)
            normalInt.y = 1;

        if (normal.z >= a)
            normalInt.z = 1;

        if (normal.x <= -a)
            normalInt.x = -1;

        if (normal.y <= -a)
            normalInt.y = -1;

        if (normal.z <= -a)
            normalInt.z = -1;*/

        return GetCell(cell.GetPosInt() - cell.normalInt);
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
                    Node n = Path.FindPathAstar(this, new Node(paths[p].midPoints[m].cell), GetCell(point), true);
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

        Node start = new Node(GetCell(point));
        Node result = Path.FindPathAstar(this, start, GetCell(end), true);
        if (result != null)
        {
            paths[bestP].dirty = true;//This path need to be recalculated
            GetCell(point).isInteresting = true;//This cell has an object on it
            UpdateWorld();
            return true;
        }
        else
        {
            Debug.Log("Couldn't add interest point at: " + point.ToString());
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

    public void Explode(Vector3Int pos, int radius)
    {
        List<Path> affectedPaths = new List<Path>();
        List<CellInfo> affectedCells = new List<CellInfo>();

        for (int i = -radius; i <= radius; i++)
        {
            for (int j = -radius; j <= radius; j++)
            {
                for (int k = -radius; k <= radius; k++)
                {
                    int x = pos.x + i;
                    int y = pos.y + j;
                    int z = pos.z + k;

                    if (IsPosInBounds(x, y, z))
                    {
                        Vector3Int newPos = new Vector3Int(x, y, z);

                        if (cells[x, y, z].endZone || cells[x, y, z].isCore)
                            continue;

                        if (Vector3Int.Distance(pos, newPos) <= radius)
                        {
                            foreach (Path p in cells[x, y, z].paths)
                            {
                                if (!affectedPaths.Contains(p))
                                    affectedPaths.Add(p);
                            }

                            affectedCells.Add(cells[x, y, z]);
                        }
                    }
                }
            }
        }

        foreach (Path p in affectedPaths)
        {
            p.Reset();
        }

        foreach (CellInfo c in affectedCells)
        {
            c.blockType = BlockType.Air;
            c.canWalk = true;
            if (c.structure)
            {
                Destroy(c.structure.gameObject);
                c.structure = null;
                c.isInteresting = false;
            }
        }

        UpdateWorld();
    }

    bool UpdateWorld()
    {
        if (cells != null)
        {
            ClearDebugStuff();
            //FillWorld();

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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Adding random interest point");
            AddInterestPoint(GetRandomCell().GetPosInt());
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Random explosion");
            Explode(GetRandomCell().GetPosInt(), 5);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            CellInfo c = GetRandomCell();
            Debug.Log(c.GetPosInt());
            Debug.Log("---------->");
            Debug.Log(GetCellUnder(c).GetPosInt());
        }
    }


    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        /*foreach (CellInfo cell in cells)
        {
            if (cell.isPath)
            {
                Handles.Label(new Vector3(cell.x, cell.y, cell.z), "1");
            }
            else if (cell.isCloseToPath)
            {
                Handles.Label(new Vector3(cell.x, cell.y, cell.z), "2");
            }
            //Gizmos.DrawSphere(new Vector3(cell.x, cell.y, cell.z), .5f);
        }*/

        foreach (CellInfo c in paths[0].cells)
        {
            Gizmos.DrawLine(c.GetPos(), c.GetPos() + c.normalInt);
            Handles.Label(c.GetPos(), c.normalInt.magnitude.ToString());
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

        if (demo)
        {
            UpdateWorld();
        }
    }
#endif
}

