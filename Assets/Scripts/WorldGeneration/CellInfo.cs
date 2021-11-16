using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CellInfo
{
    public int x, y, z;

    [HideInInspector]
    public int id { get { return x + (1000 * y) + (1000000 * z); } }
    public BlockType blockType = BlockType.Air;

    [HideInInspector]
    public bool isPath = false;
    [HideInInspector]
    public Vector3Int normalInt = Vector3Int.zero;
    internal bool isSurface = false;
    internal bool canWalk = false;
    internal bool endZone = false;

    public List<Path> paths = new List<Path>();

    public Structure structure;
    public bool isInteresting = false;
    internal bool isCloseToPath = false;
    internal bool isCore = false;

    //DELETE LATER
    //internal Vector3 dir = Vector3.zero;
    internal bool isCorner;

    public CellInfo(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public void SetStructure(Structure structure)
    {
        this.structure = structure;
    }
    public Structure GetStructure()
    {
        return this.structure;
    }

    public Vector3 GetPos()
    {
        return new Vector3(x, y, z);
    }

    public Vector3Int GetPosInt()
    {
        return new Vector3Int(x, y, z);
    }

    public void Reset()
    {
        if (paths.Count == 0)
        {
            isPath = false;
            foreach (CellInfo cell in LevelManager.instance.world.GetNeighbours(this))
            {
                if (cell.blockType == BlockType.Path)
                    cell.blockType = BlockType.Grass;
            }
        }
    }
}