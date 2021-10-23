using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public static Shop instance;

    [SerializeField] private int shopIndex = -1;

    public List<StructureBlueprint> defenseBlueprints;
    public StructureBlueprint selectedDefenseBlueprint;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    public void setShopIndex(int newIndex)
    {
        shopIndex = newIndex;
        selectedDefenseBlueprint = defenseBlueprints[shopIndex];
        purchaseDefense();
    }

    public void purchaseDefense()
    {
        if (shopIndex >= 0 && shopIndex < defenseBlueprints.Count)
        {
            BuildManager.instance.SelectStructureToBuild(selectedDefenseBlueprint);
        }
    }
}
