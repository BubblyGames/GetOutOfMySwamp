using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Structure : MonoBehaviour
{
    [Header("General stats")]
    [SerializeField]
    protected int health;
    [SerializeField]
    protected bool canLoseHealth;

    [SerializeField]
    protected int level = 0;
    [SerializeField]
    protected int maxLevel = 3;

    public virtual void UpgradeStrucrure()
    {
        if (level < maxLevel)
        {
            level++;
        }
        
    }

    public int GetLevel()
    {
        return level;
    } 
}
