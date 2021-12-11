using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldInfo
{
    public int nPaths = 4;
    public float wallDensity = 0.3f;
    public float rocksVisualReduction = 0.9f;
    public float rockSize = 3f;
    public int numberOfMidpoints = 1;
    public ThemeInfo themeInfo;
    public Wave[] waves;
    public bool canMergePaths;
    internal float waterDensity;

    public bool basicDef;
    public bool HeavyDef;
    public bool MoneyDef;
    public bool PoisonDef;
    public bool AerealDef;
    public bool Bomb;

    public WorldInfo() { }
}
