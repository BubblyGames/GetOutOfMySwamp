using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Jobs;
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

    MeshData meshData;
    VoxelRenderer voxelRenderer;//Transforms the cells array into a mesh

    int endzoneRadious;

    #region SetUp
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

            debugMidpoints = false;
        }
    }
    #endregion

    #region  WorldGeneration
    void Start()
    {
        endzoneRadious = (int)(size / 3.5f);

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

        meshData = new MeshData(true);

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

        StartCoroutine(ShowPathsCoroutine());

        //Add geometry
        UpdateMesh();
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
                    cell.hasStructure = false;

                    //Rock generation
                    float alpha = 1;
                    //float dist = Mathf.Sqrt(2 * size * size) - Mathf.Sqrt(Mathf.Pow(endX - i, 2f) + Mathf.Pow(endY - j, 2f));

                    if (Vector3.Distance(end, new Vector3(i, j, k)) < endzoneRadious)
                    {
                        alpha = 0;
                    }

                    float perlin = alpha * Perlin3D((seed + (i / rockSize)), (seed + (j / rockSize)), (seed + (k / rockSize)));
                    bool isRock = perlin > (1 - ((wallDensity * rocksVisualReduction) * alpha));
                    bool isNotRockButActsLikeRock = perlin > (1 - (wallDensity * alpha));

                    if (isRock)
                    {
                        cell.canWalk = false;
                        cell.blockType = BlockType.Rock;
                    }
                    else
                    {
                        if (cell.isSurface)
                        {
                            cell.canWalk = !isNotRockButActsLikeRock;
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

    void GenerateMesh()
    {
        meshData.Clear();
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
                    else if (j == 1)
                    {
                        cells[endX + i, j, endZ + k].blockType = BlockType.Path;
                        cells[endX + i, j, endZ + k].canWalk = false;
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

                paths[i].AddMidpoint(new Midpoint(GetCell(paths[i].start), true));
                for (int j = 0; j < numberOfMidpoints; j++)
                {
                    paths[i].AddMidpoint(new Midpoint(GetRandomCellOnSurface(paths[i].midPoints[j].cell), false));
                }
                paths[i].AddMidpoint(new Midpoint(GetCell(end), true));
                paths[i].initiated = true;
            }
            else
            {
                paths[i].Reset();
            }
            //Debug.Log(count + " attempts needed to find starting point");

            //Node which will store the result of the path finding

            if (!paths[i].FindPath())
                return false;
            //else
            //StartCoroutine(ShowPath(paths[i]));
        }

        //Debug paths and midpoints
        ShowDebugStuff();

        //Debug.Log("Generating paths took: " + (Time.realtimeSinceStartup - startTime) + "s");
        return true; //Success
    }

    #endregion

    #region UpdateWorld
    bool updating = false;
    bool hasNewChanges = false;

    public bool CallUpdateWorld()
    {
        if (!updating)
        {
            ClearDebugStuff();
            StopAllCoroutines();
            //UpdateJob updateJob = new UpdateJob();
            //updateJob.Schedule();
            if (!GeneratePaths(end.x, end.y, end.z))
            {
                ShowDebugStuff();
            }

            ShowPaths();

            UpdateMesh();
        }
        else
        {
            hasNewChanges = true;
        }
        return true;
    }

    public bool UpdateWorld()
    {
        if (!GeneratePaths(end.x, end.y, end.z))
        {

            return false;
        }

        return true;
    }

    
    public void UpdateMesh()
    {
        GenerateMesh();
        voxelRenderer.RenderMesh(meshData);
    }

    public void UpdateMeshSingleBlock()
    {
        GenerateMesh();
        voxelRenderer.RenderMesh(meshData);
    }

    public void ShowPaths()
    {
        StartCoroutine(ShowPathsCoroutine());
    }

    IEnumerator ShowPathsCoroutine()
    {
        int max = 0;
        for (int i = 0; i < paths.Count; i++)
        {
            max = Mathf.Max(max, paths[i].Length);
        }

        for (int i = 0; i < max; i++)
        {
            bool dirty = false;
            for (int j = 0; j < paths.Count; j++)
            {
                if (i < paths[j].Length - 1)
                {
                    CellInfo c = GetCellUnder(paths[j].GetCell(i));
                    if (c.blockType != BlockType.Path && c.blockType != BlockType.Swamp)
                    {
                        dirty = true;
                        c.canWalk = false;
                        c.blockType = BlockType.Path;
                    }
                }
            }
            if (dirty)
            {
                UpdateMesh();
                yield return null;
            }
        }

        yield return null;
    }
    #endregion

    #region Gameplay
    public bool ReplaceInterestPoint(Vector3Int point)
    {
        Node start = new Node(GetCell(point));
        Node result = Path.FindPathAstar(this, start, GetCell(end), true);
        if (result == null)
        {
            Debug.Log("Couldn't add interest point at: " + point.ToString());
            return false;
        }

        int bestP = -1;
        int bestM = -1;
        float minDist = Mathf.Infinity;
        bool found = false;

        //Find the closest existing midpoint
        for (int p = 0; p < paths.Count; p++)
        {
            for (int m = 1; m < paths[p].midPoints.Count - 1; m++)
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

            paths[bestP].midPoints[bestM].cell = GetCell(point);
        }
        else
        {
            //There are no midpoints close, so the point is added to a random path (shouldn't happen)
            Debug.Log("This should never happen");
            paths[bestP].midPoints.Insert(1, new Midpoint(GetCell(point), true));
            return false;
        }


        paths[bestP].dirty = true;//This path need to be recalculated
        //GetCell(point).hasStructure = true;//This cell has an object on it
        CallUpdateWorld();
        return true;
    }

    public bool AddInterestPoint(Vector3Int point)
    {
        Node start = new Node(GetCell(point));
        Node result = Path.FindPathAstar(this, start, GetCell(end), true);
        if (result == null)
        {
            Debug.Log("Couldn't add interest point at: " + point.ToString());
            return false;
        }

        int bestP = -1;
        int bestM = -1;
        float minDist = Mathf.Infinity;
        bool found = false;

        //Find the closest existing midpoint
        for (int p = 0; p < paths.Count; p++)
        {
            for (int m = 0; m < paths[p].midPoints.Count - 1; m++)
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
            Debug.Log("Valid midpoint");
            paths[bestP].midPoints.Insert(bestM + 1, new Midpoint(GetCell(point), true));
        }
        else
        {
            //There are no midpoints close, so the point is added to a random path (shouldn't happen)
            Debug.Log("This should never happen");
            return false;
            bestP = Mathf.RoundToInt(Random.Range(0, nPaths));
            paths[bestP].midPoints.Insert(1, new Midpoint(GetCell(point), true));
        }


        paths[bestP].dirty = true;//This path need to be recalculated
        GetCell(point).hasStructure = true;//This cell has an object on it
        CallUpdateWorld();
        return true;
    }

    public bool RemoveInterestPoint(Vector3Int point)
    {
        CellInfo cell = GetCell(point);
        if (cell.hasStructure)
        {
            cell.hasStructure = false;
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
                        CellInfo c = cells[x, y, z];

                        if (c.endZone || c.isCore)// || cells[x, y, z].blockType == BlockType.Rock)
                            continue;

                        if (Vector3Int.Distance(pos, newPos) <= radius)
                        {
                            foreach (Path p in c.paths)
                            {
                                p.dirty = true;
                            }

                            c.blockType = BlockType.Air;
                            c.canWalk = true;
                            if (c.structure)
                            {
                                Destroy(c.structure.gameObject);
                                c.structure = null;
                                c.hasStructure = false;
                            }
                        }
                    }
                }
            }
        }

        CallUpdateWorld();
    }
    #endregion

    #region Getters
    bool onXFace = true;
    public CellInfo GetRandomCellOnSurface(CellInfo lastCell)
    {
        int x = 1;
        int y = 1;
        int z = 1;


        int min = 10;
        int max = size - 11;
        //Debug.Log("Looking for a random midpoint...");

        CellInfo cell = null;
        CellInfo currentCell;
        int count = 0;
        while (cell == null)
        {
            count++;
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

            y = Random.Range(lastCell.y, size - 3);

            currentCell = cells[x, y, z];

            if (currentCell.canWalk && Path.FindPathAstar(this, new Node(currentCell), GetCell(end.x, end.y, end.z), true) != null)
                cell = currentCell;
        }
        //Debug.Log("Midpoint found after " + count.ToString() + " attempts");
        onXFace = !onXFace;
        return cell;
    }

    public CellInfo GetCompletelyRandomCell()
    {
        int x, y, z;
        CellInfo cell = null;

        while (cell == null)
        {
            x = Random.Range(0, size);
            y = Random.Range(1, size - 2);
            z = Random.Range(0, size);

            if (cells[x, y, z].canWalk && !cells[x, y, z].endZone && !CheckIfFloating(cells[x, y, z]) &&
                Path.FindPathAstar(this, new Node(cells[x, y, z]), GetCell(end.x, end.y, end.z), true) != null)
                cell = cells[x, y, z];
        }
        return cell;
    }

    public CellInfo GetRandomCellWithRay()
    {
        int x, y, z;
        int min = 10;
        int max = size - 11;
        CellInfo selectedCell = null;

        while (selectedCell == null)
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

            y = Random.Range(1, size - 2);

            Vector3 pos = new Vector3(x, y, z);

            RaycastHit hit;
            Physics.Raycast(pos, center.position - pos, out hit, Mathf.Infinity);

            Vector3Int intPos = new Vector3Int(
                Mathf.RoundToInt(hit.point.x + (hit.normal.x / 2)),
                Mathf.RoundToInt(hit.point.y + (hit.normal.y / 2)),
                Mathf.RoundToInt(hit.point.z + (hit.normal.z / 2)));

            if (!IsPosInBounds(intPos))
                continue;

            CellInfo c = GetCell(intPos);

            if (c.canWalk)
                selectedCell = c;
        }
        onXFace = !onXFace;

        return selectedCell;
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


    public CellInfo GetCellUnder(CellInfo cell)
    {

        if (cell.normalInt != Vector3Int.zero)
        {
            //FAILS SOMETIMES --> Normals are not always right
            Vector3Int newPos = cell.GetPosInt() - cell.normalInt;
            if (IsPosInBounds(newPos))//This shouldn't be necessary
                return GetCell(newPos);
        }

        Vector3 dir = 1.5f * (cell.GetPos() - center.position).normalized;

        Vector3Int dirInt = new Vector3Int(Mathf.RoundToInt(dir.x),
            Mathf.RoundToInt(dir.y),
            Mathf.RoundToInt(dir.z));
        //Debug.Log(dirInt);

        return GetCell(cell.GetPosInt() - dirInt);


    }
    #endregion

    #region Utils
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

    void ShowDebugStuff()
    {
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
    }

    void ClearDebugStuff()
    {
        foreach (GameObject g in debugStuff)
        {
            Destroy(g);
        }
        debugStuff.Clear();
    }

    #endregion

    #region Checks
    bool IsCore(int i)
    {
        int radius = size / 4;
        int mid = 1 + (size / 2);

        return i < mid + radius && i > mid - radius;
    }
    public bool CheckIfFloating(CellInfo cell)
    {
        foreach (CellInfo c in GetNeighbours(cell))
        {
            if (c.blockType != BlockType.Air)
                return false;
        }
        return true;
    }
    public bool CheckIfIsInSurface(CellInfo cell)
    {
        return cell.x == 0 || cell.x == size - 1 ||
            cell.y == 0 || cell.y == size - 1 ||
            cell.z == 0 || cell.z == size - 1;
    }
    public bool IsPosInBounds(int coordX, int coordY, int coordZ)
    {
        return coordX >= 0 && coordX < size && coordY >= 0 && coordY < size && coordZ >= 0 && coordZ < size;
    }

    public bool IsPosInBounds(Vector3Int pos)
    {
        return pos.x >= 0 && pos.x < size && pos.y >= 0 && pos.y < size && pos.z >= 0 && pos.z < size;
    }
    #endregion

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Adding random interest point");
            ReplaceInterestPoint(GetCompletelyRandomCell().GetPosInt());
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("Adding random interest point with ray");
            AddInterestPoint(GetRandomCellWithRay().GetPosInt());
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Random explosion");
            Explode(GetCompletelyRandomCell().GetPosInt(), 15);
        }
    }


    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        foreach (CellInfo cell in cells)
        {
            if (cell.normalInt != Vector3Int.zero)
            {
                Handles.Label(new Vector3(cell.x, cell.y, cell.z), "1");
            }
            //Gizmos.DrawSphere(new Vector3(cell.x, cell.y, cell.z), .5f);
        }

        /*foreach (CellInfo c in paths[0].cells)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(c.GetPos(), c.GetPos() + c.normalInt);
            //Gizmos.color = Color.white;
            //Gizmos.DrawLine(c.GetPos(), c.GetPos() + c.dir);
            //Handles.Label(c.GetPos(), c.normalInt.magnitude.ToString());
        }*/
    }
    /*private void OnValidate()
    {
        if (!Application.isPlaying) return;

        if (demo)
        {
            FillWorld();
            CallUpdateWorld();
        }
    }*/
#endif
}


// Job adding two floating point values together
public struct UpdateJob : IJob
{
    public void Execute()
    {
        //LevelManager.instance.world.UpdateWorld();
        Debug.Log("Job running");
    }
}

