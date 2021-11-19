using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Midpoint : IEquatable<Midpoint>
{
    public CellInfo cell;
    public bool important; //Can't be replaced

    public Midpoint(CellInfo cell, bool important)
    {
        this.cell = cell;
        this.important = important;
    }

    public bool Equals(Midpoint other)
    {
        return cell == other.cell;
    }
}
