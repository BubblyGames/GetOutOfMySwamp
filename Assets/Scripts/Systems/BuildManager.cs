using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    private StructureBlueprint structureToBuild = null; //Structure is going to be built
    [SerializeField]
    private Structure selectedStructure; //Already Built structure 
    [SerializeField]
    private CellInfo selectedCell;

    public bool canBuild;//Checks if a structure is selected to be built

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool CheckIfCanBuild(RaycastHit hit, out Vector3Int intPos)
    {
        //Position where structure will be build
        intPos = new Vector3Int(
            Mathf.RoundToInt(hit.point.x + (hit.normal.x / 2)),
            Mathf.RoundToInt(hit.point.y + (hit.normal.y / 2)),
            Mathf.RoundToInt(hit.point.z + (hit.normal.z / 2)));

        //If player has enough money
        if (LevelStats.instance.CurrentMoney < structureToBuild.creationCost || !canBuild)
            return false;

        //Spell will always be 
        SpellBehaviour sb;
        if (structureToBuild.structurePrefab.TryGetComponent<SpellBehaviour>(out sb))
            return true;

        //But for builidngs we need to check if the position is within the world bounds
        if (!LevelManager.instance.world.IsPosInBounds(intPos))
            return false;

        //Position of the block under the block where the structure will be built
        Vector3Int intPosUnder = new Vector3Int(
            Mathf.RoundToInt(hit.point.x - (hit.normal.x / 2)),
            Mathf.RoundToInt(hit.point.y - (hit.normal.y / 2)),
            Mathf.RoundToInt(hit.point.z - (hit.normal.z / 2)));

        selectedCell = LevelManager.instance.world.GetCell(intPosUnder);

        return CheckBuilding(intPos);
    }

    bool CheckBuilding(Vector3Int intPos)
    {
        if (selectedCell.blockType != structureToBuild.structurePrefab.GetComponent<Structure>().blockType)
            return false;

        Gatherer g = null;
        if (!LevelManager.instance.world.GetCell(intPos).isCloseToPath &&
            structureToBuild.structurePrefab.TryGetComponent<Gatherer>(out g))
        {
            //Debug.Log("Can't add");
            return false;
        }

        //if (g != null)
        //LevelManager.instance.world.AddInterestPoint(intPos);


        return true;
    }

    public void PlaceObject(RaycastHit hit)
    {
        Vector3Int pos;

        if (CheckIfCanBuild(hit, out pos))
        {
            BuildStructure(pos, hit.normal);
        }
    }

    internal void SetSelectedStructure(Structure structure)
    {
        selectedStructure = structure;
    }

    public void SelectCell(CellInfo cell)
    {
        selectedCell = cell;
        structureToBuild = null;
    }

    public void SelectStructureToBuild(StructureBlueprint defense)
    {
        structureToBuild = defense;
        selectedCell = null;
        canBuild = true;
    }

    public void ResetCanBuild()
    {
        canBuild = false;
    }

    public void BuildStructure(Vector3Int position, Vector3 normal)
    {
        if (CheatManager.instance != null && CheatManager.instance.infiniteMoney)
        {
            CreateTowerOnCell(position, normal);
            ResetCanBuild(); // after building an structure you have to select another one to be able to place it
        }
        else if (LevelStats.instance.CurrentMoney >= structureToBuild.creationCost)
        {
            CreateTowerOnCell(position, normal);
            LevelStats.instance.SpendMoney(structureToBuild.creationCost);
            ResetCanBuild(); // after building an structure you have to select another one to be able to place it
        }
        else
        {
            Debug.Log("Not enough Money");
            //TODO: Show on screen there is not enough money
        }
    }

    public void CreateTowerOnCell(Vector3Int position, Vector3 normal)
    {
        Structure structure = Instantiate(structureToBuild.structurePrefab, position, Quaternion.Euler(normal)).GetComponent<Structure>();
        structure.SetNormal(normal);
        structure.Blueprint = structureToBuild;

        //THIS SHOULDN'T BE NECESSARY
        if (!LevelManager.instance.world.IsPosInBounds(position))
            return;

        CellInfo cell = LevelManager.instance.world.GetCell(position);//Sometimes FAILS :(

        //If we are putting a bomb, apart from creating the model, we set the cell's structure associated in which we are creating it
        Bomb b;
        if (structure.TryGetComponent<Bomb>(out b))
        {
            if (cell.GetStructure() == null)
            {
                cell.SetStructure(b);
            }
        }

        cell.SetStructure(structure);
    }

    public void UpgradeStructure()
    {
        UIController.instance.ShowMenu(UIController.GameMenu.Game);

        if (LevelStats.instance.infinteMoney)
        {
            selectedCell.structure.UpgradeStrucrure();
        }

        //TODO: get values from Upgrade
        else if (LevelStats.instance.CurrentMoney >= structureToBuild.upgrades[selectedStructure.GetLevel()].cost)
        {
            selectedCell.structure.UpgradeStrucrure();
            LevelStats.instance.SpendMoney(structureToBuild.creationCost);

        }
        else
        {
            Debug.Log("Not enough Money");
            //TODO: Show on screen there is not enough money
        }
    }

    public void SellStructure()
    {
        Debug.Log("Selling: " + selectedStructure.name);
        UIController.instance.ShowMenu(UIController.GameMenu.Game);
        selectedStructure.Sell();
        selectedCell.structure = null;
        LevelStats.instance.EarnMoney(50);
    }
}
