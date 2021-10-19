using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    public DefenseBlueprint defenseToBuild = null; //Defense is going to be built
    
    public bool canBuild { get { return defenseToBuild.defensePrefab != null; } }//Checks if a defense is selected to be built

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

    public void SelectDefenseToBuild(DefenseBlueprint defense)
    {
        defenseToBuild = defense;
    }

    public void BuildDefenseOn(Vector3 position)
    {
        if (LevelStats.instance.currentMoney >= defenseToBuild.cost )
        {
            Instantiate(defenseToBuild.defensePrefab, position, Quaternion.identity);
            if (!LevelStats.instance.infinteMoney) {
                LevelStats.instance.SpendMoney(defenseToBuild.cost);            
            }
        }
        else
        {
            Debug.Log("Not enough Money");
            //TODO: Show on screen there is not enough money
        }
    }
}
