using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Structure : MonoBehaviour
{
    private StructureBlueprint _blueprint;

    public StructureBlueprint Blueprint { get => _blueprint; set => _blueprint = value; }

    public int Length { get => length; set => length = value; }

    public int Width { get => width; set => width = value; }

    [Header("General stats")]
    [SerializeField]
    protected int width =1;
    [SerializeField]
    protected int length =1;

    [SerializeField]
    protected int health;
    [SerializeField]
    protected bool canLoseHealth;

    [SerializeField]
    protected int level = 0;
    protected int maxLevel = 3;
    protected bool isMaxLevel = false;

    protected Vector3 normal;

    public BlockType blockType = BlockType.Grass;



    public void SetNormal(Vector3 normal)
    {
       this.normal = normal;
       transform.up = this.normal;
    }

    public virtual void UpgradeStrucrure()
    {
        if (!isMaxLevel)
        {
            if (level < maxLevel)
            {
                level++;
            }

            if (level == maxLevel)
            {
                isMaxLevel = true;
            }
        }
        
    }

    public int GetLevel()
    {
        return level;
    }

    public void Sell()
    {
        Destroy(gameObject);
    }
}
