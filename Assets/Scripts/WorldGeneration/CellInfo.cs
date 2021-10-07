using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CellInfo
{
    public int x, y, z, face;
    public int id { get { return x + (1000 * y); } }
    public int state = 0;
    public CellInfo(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}