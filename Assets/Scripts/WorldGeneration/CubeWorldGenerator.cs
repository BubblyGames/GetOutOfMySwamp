using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CubeWorldGenerator : MonoBehaviour
{
    public int size = 20;
    internal CellInfo3D[,,] cells; //0 walkable //1 can build //2 can't build //3 target

    internal Path[] paths;
    public int nPaths = 4;

    [Range(0.0f, 1.0f)]
    public float wallDensity = 0.3f;
    public float rockSize = 3f;
    public float seed = 0f;

    public GameObject floorPrefab;
    public Material[] materials;

    VoxelRenderer voxelRenderer;

    private void Awake()
    {
        voxelRenderer = GetComponent<VoxelRenderer>();
    }

    void Start()
    {
        if (seed == 0f)
            seed = Random.value * 10;
        Debug.Log("Seed: " + seed.ToString());

        int endX = size - 1;
        int endY = size - 1;
        int endZ = size - 1;

        cells = new CellInfo3D[size, size, size];
        MeshData meshData = new MeshData(true);

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                for (int k = 0; k < size; k++)
                {
                    CellInfo3D cell = new CellInfo3D(i, j, k);

                    //Rock generation
                    float alpha = 1;
                    //float dist = Mathf.Sqrt(2 * size * size) - Mathf.Sqrt(Mathf.Pow(endX - i, 2f) + Mathf.Pow(endY - j, 2f));

                    float horizontalNoise = Mathf.PerlinNoise(seed + (i / rockSize), seed + (j / rockSize));
                    float verticalNoise = Mathf.PerlinNoise(seed + (i / rockSize), seed + (k / rockSize));

                    if ((horizontalNoise + verticalNoise) / 2 > (1 - (wallDensity * alpha)))//i == 0 || j == 0 || i == size - 1 || j == size - 1 ||//|| (i == j && i < size - 1)
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

        //GeneratePaths(endX, endY, endZ);
        cells[endX, endY, endZ].blockType = BlockType.Swamp;
        voxelRenderer.RenderMesh(meshData);
    }

    bool CheckIfSurface(CellInfo3D cell)
    {
        return cell.x == 0 || cell.x == size - 1 ||
            cell.y == 0 || cell.y == size - 1 ||
            cell.z == 0 || cell.z == size - 1;
    }

    private void GeneratePaths(int endX, int endY, int endZ)
    {
        paths = new Path[nPaths];
        for (int i = 0; i < nPaths; i++)
        {
            int x = 0;
            int y = 0;
            int z = 0;

            int count = 0;
            while ((cells[x, y, z].blockType == BlockType.Path || cells[x, y, z].blockType == BlockType.Rock) && count < 100)
            {
                if (i < nPaths / 2)
                {
                    x = Random.Range(1, size - 1);
                }
                else
                {
                    y = Random.Range(1, size - 1);
                }
                count++;
            }

            cells[x, y, z].blockType = BlockType.Path;

            Node3D p = FindPath(nPaths, cells[x, y, z], cells[endX, endY, endZ]);
            if (p != null)
            {
                List<CellInfo3D> pathCells = new List<CellInfo3D>();
                while (p != null)
                {
                    cells[p.x, p.y, p.z].blockType = BlockType.Path; ;
                    pathCells.Add(cells[p.x, p.y, p.z]);
                    //floor[p.x, p.y].transform.Translate(-Vector3.forward * 0.1f);
                    p = p.Parent;
                }
                pathCells.Reverse();
                paths[i] = new Path(pathCells.ToArray());
            }
        }
    }



    Node3D FindPath(int nPaths, CellInfo3D start, CellInfo3D end)
    {
        Node3D current;
        Node3D firstNodo;

        List<Node3D> openList = new List<Node3D>();
        List<Node3D> closedList = new List<Node3D>();

        firstNodo = new Node3D(start);

        //Primer nodo la posici�n incial con padre null
        firstNodo.ComputeHScore(end.x, end.y, end.z);
        firstNodo.Parent = null;
        openList.Add(firstNodo);



        int count = 0;
        while (openList.Count > 0 && count < 1000)
        {
            count++;
            //Ordenar la lista en orden ascendente de h
            openList = openList.OrderBy(o => o.h).ToList();

            //Mira el primer nodo de la lista
            current = openList[0];
            closedList.Add(current);
            openList.Remove(current);
            //Si el primer nodo es goal, returns current Node3D
            if (current.cell.blockType == BlockType.Swamp)
            {
                Debug.Log("Success: " + count.ToString());
                return current;
            }
            else
            {
                //Expande vecinos (calcula coste de cada uno, etc)y los a�ade en la lista
                CellInfo3D[] neighbours = WalkableNeighbours(current.cell);
                foreach (CellInfo3D neighbour in neighbours)
                {
                    if (neighbour != null)
                    {
                        //if neighbour no esta en open
                        bool IsInOpen = false;
                        foreach (Node3D nf in openList)
                        {
                            if (nf.cell.id == neighbour.id)
                            {
                                IsInOpen = true;
                                break;
                            }
                        }

                        bool IsInClosed = false;
                        foreach (Node3D nf in closedList)
                        {
                            if (nf.cell.id == neighbour.id)
                            {
                                IsInClosed = true;
                                break;
                            }
                        }

                        if (!IsInOpen && !IsInClosed)
                        {
                            Node3D n = new Node3D(neighbour);
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

    private CellInfo3D[] WalkableNeighbours(CellInfo3D current)
    {
        List<CellInfo3D> result = new List<CellInfo3D>();

        int[,] neighbours = new int[,] { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 }
                                        ,{ -1, -1 }, { -1, 1 }, { 1, 1 }, { 1, -1 }};

        for (int i = 0; i < 4; i++)
        {
            int x = current.x + neighbours[i, 0];
            int y = current.y + neighbours[i, 1];
            int z = current.z + neighbours[i, 2];

            if (x >= 0 && x < size && y >= 0 && y < size && (cells[x, y, z].blockType != BlockType.Rock))
            {
                result.Add(cells[x, y, z]);
            }
        }


        return result.ToArray();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        foreach (CellInfo3D cell in cells)
        {
            Handles.Label(new Vector3(cell.x, cell.y, cell.z), cell.blockType.ToString());
        }
    }
#endif
}
