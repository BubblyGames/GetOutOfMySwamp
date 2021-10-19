using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager buildManagerInstance;

    public StructureBlueprint structureToBuild = null; //Structure is going to be built
    
    public bool canBuild { get { return structureToBuild.structurePrefab != null; } }//Checks if a structure is selected to be built

    private void Awake()
    {
        if (buildManagerInstance != null)
        {
            return;
        }
        buildManagerInstance = this;
    }

    public void SelectStructureToBuild(StructureBlueprint structure)
    {
        structureToBuild = structure;
    }

    public void BuildStructureOn(Vector3 position)
    {
        if (LevelStats.levelStatsInstance.currentMoney >= structureToBuild.cost )
        {
            Instantiate(structureToBuild.structurePrefab, position, Quaternion.identity);
            if (!LevelStats.levelStatsInstance.infinteMoney) {
                LevelStats.levelStatsInstance.SpendMoney(structureToBuild.cost);            
            }
        }
        else
        {
            Debug.Log("Not enough Money");
            //TODO: Show on screen there is not enough money
        }
    }
}
