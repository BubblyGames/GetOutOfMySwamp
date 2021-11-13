using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Structure : MonoBehaviour
{
    private StructureBlueprint _blueprint;

    public StructureBlueprint Blueprint { get => _blueprint; set => _blueprint = value; }

    [Header("General stats")]
    [SerializeField]
    protected int health;
    [SerializeField]
    protected bool canLoseHealth;

    [SerializeField]
    protected int level = 0;
    [SerializeField]
    protected int maxLevel = 3;
    [SerializeField]
    protected bool isMaxLevel = false;


    [SerializeField]
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
