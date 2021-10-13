using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private int shopIndex = -1;

    public List<StructureBlueprint> structureBlueprints;
    public StructureBlueprint selectedStructureBlueprint;
    public void setShopIndex(int newIndex)
    {
        shopIndex = newIndex;
        selectedStructureBlueprint = structureBlueprints[shopIndex];
        purchaseStructure();
    }

    public void purchaseStructure()
    {
        if (shopIndex >= 0 && shopIndex <structureBlueprints.Count)
        {
            //BuildManager.buildManagerInstance.setStructureToBuild(BuildManager.buildManagerInstance.structures[shopIndex]);
            BuildManager.buildManagerInstance.SelectStructureToBuild(selectedStructureBlueprint);
        }
    }
}
