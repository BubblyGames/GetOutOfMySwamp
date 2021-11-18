using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CellInfo
{
    public int x, y, z;

    public int id { get { return x + (1000 * y) + (1000000 * z); } }
    public BlockType blockType = BlockType.Air;

    internal bool isSurface = false;
    internal bool isCore = false;

    public Structure structure;
    public bool hasStructure = false;

    //Path stuff
    public List<Path> paths = new List<Path>();
    public bool isPath = false;
    internal bool isCloseToPath = false;
    internal bool canWalk = false;
    internal bool endZone = false;

    public Vector3Int normalInt = Vector3Int.zero;

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
        isPath = paths.Count > 0;
    }
}