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
    internal CellInfo[,,] cells;
    List<CellInfo> emptyCells;

    [Header("World generation")]
    public int size = 21;//Odd numbers look better
    [Range(0.0f, 1.0f)]
    public float wallDensity = 0.3f;
    [Range(0.0f, 1.0f)]
    public float rocksVisualReduction = 0.9f;
    public float rockSize = 3f;
    [Range(0.0f, 1.0f)]
    public float waterDensity = 0.2f;

    [Header("Path settings")]
    internal List<Path> paths;
    public int nPaths = 4;
    public bool canMergePaths = false;
    public int numberOfMidpoints = 1;
    public int seed = 0;
    internal Vector3Int end;
    internal Transform center;

    [Header("Extra")]
    public Material waterMaterial;
    public bool demo = false;
    public bool sphere = false;
    //Debug stuff
    public bool debugMidpoints = false;
    public GameObject lineRendererPrefab;
    List<GameObject> debugStuff = new List<GameObject>();

    MeshData meshData;
    VoxelRenderer voxelRenderer;//Transforms the cells array into a mesh

    int endzoneRadious;
    int swampRadious;

    #region SetUp
    private void Awake()
    {
        voxelRenderer = GetComponent<VoxelRenderer>();

        GameObject worldCenter = new GameObject("World center");
        worldCenter.transform.position = transform.position + (Vector3.one * ((size - 1) / 2f)); //set center too middle of the cube
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
            waterDensity = worldInfo.waterDensity;
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
        swampRadious = (int)(endzoneRadious * .75f);

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

        CreateWorld();
    }

    public void CreateWorld()
    {
        bool success = false;
        int count = 0;
        //While enough paths can't be created, it will try new seeds and restart the process
        while (!success && count < 5)
        {
            count++;
            Debug.Log("Attempt: " + count + " Seed: " + seed.ToString());
            GenerateWorld();//Choose the blocktype of each cell
            UpdateMesh();
            if (!demo)
            {
                foreach (Path p in paths)
                {
                    p.dirty = true;
                }

                success = GeneratePaths();//Tries to create paths
                if (!success)
                {
                    ClearDebugStuff();
                    seed = Mathf.RoundToInt(Random.value * 100000);//New seed
                    wallDensity -= 0.01f;
                }
            }
            else
            {
                success = true;
            }
            //yield return null;
        }

        if (LevelManager.instance)
        {
            CreateWater();
            LevelManager.instance.ready = true;
        }

        StartCoroutine(ShowPathsCoroutine());
        //yield return null;
    }

    public void CreateWater()
    {
        GameObject water;
        if (sphere)
            water = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        else
            water = GameObject.CreatePrimitive(PrimitiveType.Cube);
        water.transform.parent = transform;
        water.transform.position = center.transform.position;
        water.transform.localScale = (size - 4.5f) * Vector3.one;
        Destroy(water.GetComponent<Collider>());

        waterMaterial = new Material(waterMaterial);
        water.GetComponent<MeshRenderer>().material = waterMaterial;

        Color waterColor;
        if (LevelManager.instance)
        {
            waterColor = LevelManager.instance.colors[2];
        }
        else
        {
            Texture2D t = (Texture2D)GetComponent<MeshRenderer>().material.mainTexture;
            waterColor = t.GetPixel(21, 1);
        }

        waterMaterial.color = new Color(waterColor.r, waterColor.g, waterColor.b, .5f);
    }

    private void GenerateWorld()
    {
        end = new Vector3Int();
        end.x = size / 2;
        end.y = size - 3;
        end.z = size / 2;

        //cells = new CellInfo[size, size, size];
        FillWorld();

        //Debug.Log("World generated");
        GenerateSwamp();
    }

    void FillWorld()
    {
        emptyCells = new List<CellInfo>();
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
                    cell.blockType = BlockType.Air;

                    //Maybe some day
                    float distanceToCenter = Vector3.Distance(center.position, cell.GetPos());
                    if (sphere && distanceToCenter > size / 2f)
                        continue;

                    //Rock generation
                    float alpha = 1;
                    //float dist = Mathf.Sqrt(2 * size * size) - Mathf.Sqrt(Mathf.Pow(endX - i, 2f) + Mathf.Pow(endY - j, 2f));

                    //Distance to center
                    //Is it's inside of core equals 0
                    //Else equals float between 0 and 1 based on distance
                    //1 is far from center
                    //0 is close
                    alpha = Mathf.Clamp01((Vector2.Distance(new Vector2(end.x, end.z), new Vector2(i, k)) - endzoneRadious) / ((size / 2f) - endzoneRadious));

                    float perlin = Perlin3D((seed + (i / rockSize)), (seed + (j / rockSize)), (seed + (k / rockSize)));

                    bool isRock = perlin * alpha > 1 - (wallDensity * rocksVisualReduction);
                    bool isNotRockButActsLikeRock = perlin * alpha > 1 - wallDensity;
                    bool isWater = perlin < waterDensity * alpha;

                    if (isWater)
                    {
                        cell.canWalk = true;
                        cell.blockType = BlockType.Air;
                        emptyCells.Add(cell);
                    }
                    else if (isRock)
                    {
                        cell.canWalk = false;
                        cell.blockType = BlockType.Rock;
                    }
                    else if (cell.isSurface)
                    {
                        cell.blockType = BlockType.Air;
                        if (isNotRockButActsLikeRock)
                            cell.canWalk = false;
                        else
                        {
                            cell.canWalk = true;
                            emptyCells.Add(cell);
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

    private void GenerateSwamp()
    {
        for (int i = -endzoneRadious; i < endzoneRadious; i++)
        {
            for (int j = 0; j < size; j++)
            {
                for (int k = -endzoneRadious; k < endzoneRadious; k++)
                {
                    Vector2 pos = new Vector2(end.x + i, end.z + k);
                    if (Vector2.Distance(new Vector2(end.x, end.z), pos) > endzoneRadious)
                    {
                        continue;
                    }

                    cells[end.x + i, j, end.z + k].endZone = true;

                    if (Mathf.Abs(i) > swampRadious / 2 || Mathf.Abs(k) > swampRadious / 2)
                    {
                        continue;
                    }

                    if (j > size - 3 || j == 1)
                    {
                        cells[end.x + i, j, end.z + k].blockType = BlockType.Air;
                        cells[end.x + i, j, end.z + k].canWalk = true;
                    }
                    else if (j == size - 3)
                    {
                        cells[end.x + i, j, end.z + k].blockType = BlockType.Swamp;
                        cells[end.x + i, j, end.z + k].canWalk = true;
                    }
                    else if (j == 2)
                    {
                        cells[end.x + i, j, end.z + k].blockType = BlockType.Path;
                        cells[end.x + i, j, end.z + k].canWalk = true;
                    }
                    /*else
                    {
                        cells[endX + i, j, endZ + k].blockType = BlockType.Grass;
                        cells[endX + i, j, endZ + k].canWalk = false;
                    }*/
                }
            }
        }
    }

    private bool GeneratePaths()
    {
        float startTime = Time.realtimeSinceStartup;

        if (Path.FindPathAstar(this, new Node(cells[size / 2, 0, size / 2]), GetCell(end), true, canMergePaths) == null)
            return false;

        for (int i = 0; i < nPaths; i++)
        {
            if (!paths[i].dirty) //Should check if a path has been modified to update it, but not yet
                continue;

            if (!paths[i].initiated)//First time calculating a path
            {
                /*//Random start position
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
                    return false;*/

                paths[i].start = new Vector3Int(size / 2, 2, size / 2);
                paths[i].AddMidpoint(new Midpoint(GetCell(paths[i].start), true));
                for (int j = 0; j < numberOfMidpoints; j++)
                {
                    //while (!paths[i].AddMidpoint(new Midpoint(GetRandomCellOnSurface(paths[i].midPoints[j].cell), false))) ;
                    while (!paths[i].AddMidpoint(new Midpoint(GetRandomCellWithRay(), false))) ;
                    //paths[i].AddMidpoint(new Midpoint(GetCell(0,15,15), false)) ;
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
            UpdateMesh();
            //UpdateJob updateJob = new UpdateJob();
            //updateJob.Schedule();
            if (!demo && !GeneratePaths())
            {
                ShowDebugStuff();
            }

            ShowPaths();

        }
        else
        {
            hasNewChanges = true;
        }
        return true;
    }

    public bool UpdateWorld()
    {
        if (!GeneratePaths())
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
                //UpdateMesh();
                //yield return null;
            }
        }
        UpdateMesh();

        UpdateMesh();
        yield return null;
    }
    #endregion

    #region Gameplay
    public bool ReplaceInterestPoint(Vector3Int point)
    {
        Node start = new Node(GetCell(point));
        Node result = Path.FindPathAstar(this, start, GetCell(end), true, canMergePaths);
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
                    Node n = Path.FindPathAstar(this, new Node(paths[p].midPoints[m].cell), GetCell(point), true, canMergePaths);
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
            paths[bestP].dirty = true;
            CallUpdateWorld();
            return true;
        }

        //There are no midpoints close, so the point is added to a random path (shouldn't happen)
        Debug.Log("This should never happen");
        bestP = Mathf.RoundToInt(Random.Range(0, nPaths));
        if (paths[bestP].InsertMidpoint(1, new Midpoint(GetCell(point), true)))
        {
            paths[bestP].dirty = true;
            CallUpdateWorld();
            return true;
        }

        return false;
    }

    public bool AddInterestPoint(Vector3Int point)
    {
        Node start = new Node(GetCell(point));
        Node result = Path.FindPathAstar(this, start, GetCell(end), true, canMergePaths);
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
                    Node n = Path.FindPathAstar(this, new Node(paths[p].midPoints[m].cell), GetCell(point), true, canMergePaths);
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
            ///Debug.Log("Valid midpoint");
            paths[bestP].dirty = true;//This path need to be recalculated

            if (paths[bestP].InsertMidpoint(bestM + 1, new Midpoint(GetCell(point), true)))
            {
                CallUpdateWorld();
                return true;
            }
        }

        Debug.Log("This should never happen");

        //There are no midpoints close, so the point is added to a random path (shouldn't happen)
        bestP = Mathf.RoundToInt(Random.Range(0, nPaths));
        return paths[bestP].InsertMidpoint(1, new Midpoint(GetCell(point), true));
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
    public CellInfo GetRandomCellOnSurface(CellInfo lastCell = null)
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

            if (lastCell != null)
                y = Random.Range(lastCell.y, size - 3);
            else
                y = Random.Range(2, size - 3);

            currentCell = cells[x, y, z];

            if (currentCell.canWalk && !currentCell.endZone && Path.FindPathAstar(this, new Node(currentCell), GetCell(end.x, end.y, end.z), true, canMergePaths) != null)
                cell = currentCell;
        }
        //Debug.Log("Midpoint found after " + count.ToString() + " attempts");
        onXFace = !onXFace;

        return cell;
    }

    public CellInfo GetCompletelyRandomCell()
    {
        CellInfo cell = null;
        CellInfo currentCell;
        while (cell == null)
        {

            int randomIdx = Random.Range(0, emptyCells.Count);
            currentCell = emptyCells[randomIdx];

            if (!currentCell.endZone &&
                currentCell.y > 0 &&
                currentCell.y < size - 1 &&
                Path.FindPathAstar(this, new Node(currentCell), GetCell(end.x, end.y, end.z), true, canMergePaths) != null)
                cell = currentCell;
        }
        return cell;
    }

    public CellInfo GetRandomCellWithRay(CellInfo cell = null)
    {
        CellInfo selectedCell = null;
        int count = 0;
        while (selectedCell == null && count < 10)
        {
            CellInfo c;

            if (cell == null)
                c = GetRandomCellOnSurface();
            else
                c = cell;

            RaycastHit hit;
            Vector3 pos = c.GetPos();
            Physics.Raycast(pos, center.position - pos, out hit, Mathf.Infinity);

            Vector3Int intPos = new Vector3Int(
                Mathf.RoundToInt(hit.point.x + (hit.normal.x / 2)),
                Mathf.RoundToInt(hit.point.y + (hit.normal.y / 2)),
                Mathf.RoundToInt(hit.point.z + (hit.normal.z / 2)));

            if (!IsPosInBounds(intPos))
                continue;

            c = GetCell(intPos);

            if (c.canWalk)
                selectedCell = c;

            c = null;
            count++;
        }

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

    /// <summary>
    /// Checks if a cube's position is valid
    /// </summary>
    /// <param name="cellindex">Checked position</param>
    /// <returns>If position is inside cube's bounds</returns>
    public bool CheckCell(Vector3 cellindex, BlockType blocktype, Vector3 cellOnTop)
    {
        return (cellindex.x >= 1 && cellindex.x < cells.GetLength(0) - 1) &&
            (cellindex.y >= 1 && cellindex.y < cells.GetLength(0) - 1) &&
            (cellindex.z >= 1 && cellindex.z < cells.GetLength(0) - 1) &&
            GetCell(Vector3Int.FloorToInt(cellindex)).structure == null &&
            GetCell(Vector3Int.FloorToInt(cellindex)).blockType == blocktype &&
            GetCell(Vector3Int.FloorToInt(cellOnTop)).blockType == BlockType.Air;
    }

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

    /// <summary>
    /// Returns cell directly under based on its normal. If the normal isn't valid,
    /// generates a normal that sometimes is wrong.
    /// </summary>
    /// <param name="cell">Cell to pass.</param>
    /// <returns>Returns the cell.</returns>
    public CellInfo GetCellUnder(CellInfo cell)
    {

        if (cell.normalInt != Vector3Int.zero)
        {
            //FAILS SOMETIMES --> Normals are not always right
            Vector3Int newPos = cell.GetPosInt() - cell.normalInt;
            if (IsPosInBounds(newPos))//This shouldn't be necessary
                return GetCell(newPos);
        }

        return GetCellUnderWithGravity(cell);
    }

    public CellInfo GetCellUnderWithGravity(CellInfo cell)
    {
        return GetRandomCellWithRay(cell);

        CellInfo newCell;
        int count = 0;
        while (CheckIfFloating(cell) && count < size)
        {
            Vector3 dir = (cell.GetPos() - center.position);

            Vector3Int dirInt = Path.Vector3ToIntNormalized(dir);

            newCell = GetCell(cell.GetPosInt() - dirInt);

            if (!newCell.canWalk)
            {
                Vector3Int newDirInt = new Vector3Int(dirInt.x, 0, 0);
                newCell = GetCell(cell.GetPosInt() - newDirInt);
            }

            if (!newCell.canWalk)
            {
                Vector3Int newDirInt = new Vector3Int(0, dirInt.y, 0);
                newCell = GetCell(cell.GetPosInt() - newDirInt);
            }

            if (!newCell.canWalk)
            {
                Vector3Int newDirInt = new Vector3Int(0, 0, dirInt.z);
                newCell = GetCell(cell.GetPosInt() - newDirInt);
            }

            cell = newCell;
            //Debug.Log(cell.GetPos());
            count++;
        }

        if (count == size)
        {
            Debug.Log("Wtf fix this a$ap: " + cell.GetPos());
            return GetRandomCellWithRay(cell);
        }

        return cell;
    }

    internal CellInfo GetClosestWalkableCell(CellInfo cell)
    {
        Debug.Log("You shouldn't need me, but I just saved you");

        Node current = new Node(cell);

        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        openList.Add(current);

        int count = 0;
        while (openList.Count > 0 && count < 100)
        {
            current = openList[0];
            closedList.Add(current);
            openList.Remove(current);

            if (current.cell.canWalk && !CheckIfFloating(current.cell))//If first node is goal,returns current Node3D
            {
                return current.cell;
            }
            else
            {
                foreach (CellInfo neighbour in GetNeighbours(current.cell))
                {
                    if (neighbour == null)//||(neighbour.isPath && !neighbour.endZone)
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
                        openList.Add(n);
                    }
                }
            }
        }
        return null;
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

        return Mathf.Max(0, xy * xz * yz * yx * zx * zy);
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
                    midSphere.GetComponent<Collider>().enabled = false;
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
            AddInterestPoint(GetCompletelyRandomCell().GetPosInt());
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
    private void OnValidate()
    {
        if (!Application.isPlaying) return;

        if (demo && cells != null)
        {
            FillWorld();
            GenerateSwamp();
            UpdateMesh();
        }
    }
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

