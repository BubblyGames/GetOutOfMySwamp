using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CellInfo
{
    public int x, y, z;

    [HideInInspector]
    public int id { get { return x + (1000 * y) + (1000000 * z); } }
    public BlockType blockType = BlockType.Air;
    [HideInInspector]
    public bool explored = false;
    [HideInInspector]
    public bool isPath = false;
    [HideInInspector]
    public Vector3Int normalInt = Vector3Int.zero;
    internal bool isSurface = false;
    internal bool canWalk = false;
    internal bool endZone = false;

    public Structure structure;

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
}