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

        if (LevelStats.instance.CurrentMoney < structureToBuild.creationCost)
            return false;

        Vector3Int intPosUnder = new Vector3Int(
            Mathf.RoundToInt(hit.point.x - (hit.normal.x / 2)),
            Mathf.RoundToInt(hit.point.y - (hit.normal.y / 2)),
            Mathf.RoundToInt(hit.point.z - (hit.normal.z / 2)));

        selectedCell = LevelManager.instance.world.GetCell(intPosUnder);

        if (!canBuild || selectedCell.blockType != structureToBuild.structurePrefab.GetComponent<Structure>().blockType)
            return false;

        Gatherer g;
        if (WorldManager.instance.IsPosInBounds(intPos.x, intPos.y, intPos.z) &&
            !LevelManager.instance.world.GetCell(intPos).isCloseToPath &&
            structureToBuild.structurePrefab.TryGetComponent<Gatherer>(out g))
        {
            //Debug.Log("Can't add");
            return false;
        }

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
        if (CheatManager.instance.infiniteMoney)
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

        //If we are putting a bomb, apart from creating the model, we set the cell's structure associated in which we are creating it
        Bomb b;
        CellInfo cell = LevelManager.instance.world.GetCell(position);
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
        if (CheatManager.instance.infiniteMoney)
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
