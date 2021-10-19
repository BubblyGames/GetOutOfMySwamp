using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private int shopIndex = -1;

    public List<DefenseBlueprint> defenseBlueprints;
    public DefenseBlueprint selectedDefenseBlueprint;
    public void setShopIndex(int newIndex)
    {
        shopIndex = newIndex;
        selectedDefenseBlueprint = defenseBlueprints[shopIndex];
        purchaseDefense();
    }

    public void purchaseDefense()
    {
        if (shopIndex >= 0 && shopIndex <defenseBlueprints.Count)
        {
           
            BuildManager.instance.SelectDefenseToBuild(selectedDefenseBlueprint);
        }
    }
}
