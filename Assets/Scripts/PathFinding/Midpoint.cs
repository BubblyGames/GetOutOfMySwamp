using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Midpoint
{
    public CellInfo cell;
    public bool important;

    public Midpoint(CellInfo cell, bool important)
    {
        this.cell = cell;
        this.important = important;
    }
}
