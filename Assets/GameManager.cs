using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    int size = 20;
    CellInfo[,] cells; //0 walkable //1 can build //2 can't build //3 target
    GameObject[,] floor;
    Path[] paths;
    int nPaths = 4;

    public GameObject floorPrefab;
    public GameObject weaponPrefab;
    public GameObject enemyPrefab;
    public Material[] materials;

    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        cells = new CellInfo[size, size];
        floor = new GameObject[size, size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                GameObject cell = GameObject.Instantiate(floorPrefab, new Vector3(i, j, -Random.Range(0, .15f)), Quaternion.identity);
                cell.GetComponent<CellId>().x = i;
                cell.GetComponent<CellId>().y = j;

                floor[i, j] = cell;
                cell.transform.parent = transform;
                cells[i, j] = new CellInfo(i, j);
                floor[i, j].name = "Floor_" + cells[i, j].id.ToString();

                if (Random.value > .99f || (i == j && i < size - 1))//i == 0 || j == 0 || i == size - 1 || j == size - 1 ||
                {
                    cells[i, j].state = 2;
                    floor[i, j].transform.Translate(-Vector3.forward * 0.5f);
                    cell.GetComponent<MeshRenderer>().material = materials[2];
                }
                else
                {
                    cells[i, j].state = 1;//Should use perlin noise?
                    cell.GetComponent<MeshRenderer>().material = materials[1];

                }
            }
        }

        paths = new Path[nPaths];
        int endX = size - 1;
        int endY = size - 1;
        floor[endX, endY].transform.Translate(-Vector3.forward);

        for (int i = 0; i < nPaths; i++)
        {
            int x = 0;
            int y = 0;

            if (i < nPaths / 2)
            {
                x = Random.Range(1, size - 1);
            }
            else
            {
                y = Random.Range(1, size - 1);
            }

            cells[x, y].state = 0;
            floor[x, y].GetComponent<MeshRenderer>().material = materials[0];
            floor[x, y].transform.position = new Vector3(x, y, 0);

            Nodo p = GeneratePaths(nPaths, cells[x, y], cells[endX, endY]);
            if (p != null)
            {
                List<CellInfo> pathCells = new List<CellInfo>();
                while (p != null)
                {
                    cells[p.x, p.y].state = 0;
                    floor[p.x, p.y].GetComponent<MeshRenderer>().material = materials[0];
                    floor[p.x, p.y].transform.position = new Vector3(p.x, p.y, 0);
                    pathCells.Add(cells[p.x, p.y]);
                    //floor[p.x, p.y].transform.Translate(-Vector3.forward * 0.1f);
                    p = p.Parent;
                }
                pathCells.Reverse();
                paths[i] = new Path(pathCells.ToArray());
            }
        }
    }

    Nodo GeneratePaths(int nPaths, CellInfo start, CellInfo end)
    {
        Nodo current;
        Nodo firstNodo;

        List<Nodo> openList;

        firstNodo = new Nodo(start);
        openList = new List<Nodo>();

        //Primer nodo la posición incial con padre null
        firstNodo.ComputeHScore(end.x, end.y);
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


            openList.Remove(current);
            //Si el primer nodo es goal, returns current node
            if (current.x == end.x && current.y == end.y)
            {
                return current;
            }
            else
            {
                //Expande vecinos (calcula coste de cada uno, etc)y los añade en la lista
                CellInfo[] neighbours = WalkableNeighbours(current.cell);
                foreach (CellInfo neighbour in neighbours)
                {
                    if (neighbour != null)
                    {
                        //if neighbour no esta en open
                        bool IsInOpen = false;
                        foreach (Nodo nf in openList)
                        {
                            if (nf.cell.id == neighbour.id)
                            {
                                IsInOpen = true;
                                break;
                            }
                        }

                        if (!IsInOpen)
                        {
                            Nodo n = new Nodo(neighbour);
                            n.ComputeHScore(end.x, end.y);
                            n.Parent = current;
                            n.cell = cells[n.x, n.y];

                            if (n.h < current.h)
                            {
                                openList.Add(n);
                            }
                        }
                    }
                }
            }
        }
        Debug.Log("Fail");
        return null;
    }

    private CellInfo[] WalkableNeighbours(CellInfo current)
    {
        List<CellInfo> result = new List<CellInfo>();
        /*for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                int x = current.x + i;
                int y = current.y + j;
                if (x >= 0 && x < size && y >= 0 && y < size)
                {
                    result.Add(cells[x, y]);
                }
            }
        }*/

        int[,] neighbous = new int[,] { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 } };

        for (int i = 0; i < 4; i++)
        {
            int x = current.x + neighbous[i, 0];
            int y = current.y + neighbous[i, 1];
            if (x >= 0 && x < size && y >= 0 && y < size && (cells[x, y].state == 1 || cells[x, y].state == 0))
            {
                result.Add(cells[x, y]);
            }
        }

        return result.ToArray();
    }

    private void Update()
    {
        //transform.Rotate(Vector3.forward,Time.deltaTime*10);
        text.text = (1 / Time.deltaTime).ToString();

        if (Input.GetMouseButtonDown(0))
        {
            SpawnWeapon();
        }

        for (int i = 0; i < nPaths; i++)
        {
            if (paths[i].CheckSpawn())
            {
                GameObject.Instantiate(enemyPrefab, paths[i].GetStep(0), Quaternion.identity).GetComponent<EnemyBehaviour>().SetPath(paths[i]);
            }
        }
    }

    private void SpawnWeapon()
    {
        GameObject obj = CastRay();

        if (obj != null)
        {
            CellId cell;
            if (obj.TryGetComponent<CellId>(out cell))
            {
                CellInfo oppositeCell = cells[cell.y, cell.x];//First simmetry axis
                //CellInfo oppositeCell = cells[size - 1 - cell.y, size - 1 - cell.x];//Second simmetry axis

                if (cells[cell.x, cell.y].state == 1 && oppositeCell.state == 1)
                {
                    GameObject.Instantiate(weaponPrefab, obj.transform.position - Vector3.forward, Quaternion.identity);
                    GameObject.Instantiate(weaponPrefab, floor[oppositeCell.x, oppositeCell.y].transform.position - Vector3.forward, Quaternion.identity);
                    cells[cell.x, cell.y].state = 2;
                    oppositeCell.state = 2;
                }
            }
        }
    }

    GameObject CastRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = Physics.RaycastAll(ray.origin, ray.direction, Mathf.Infinity)[0];// .Raycast(ray.origin, ray.direction, Mathf.Infinity);
        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        return null;
    }
}

public class CellInfo
{
    public int x, y;
    public int id { get { return x + (1000 * y); } }
    public int state = 0;
    public CellInfo(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

public class Path
{
    CellInfo[] cells;
    float spawnWait = 1f;
    float nextSpawnTime = 0;
    public float Length { get { return cells.Length; } }

    public Path(CellInfo[] cellInfos)
    {
        cells = cellInfos;
    }
    public Vector3 GetStep(int idx) { return new Vector3(cells[idx].x, cells[idx].y, -1); }

    public bool CheckSpawn()
    {
        if (Time.time > nextSpawnTime)
        {
            nextSpawnTime = Time.time + spawnWait;
            return true;
        }
        return false;
    }
}





