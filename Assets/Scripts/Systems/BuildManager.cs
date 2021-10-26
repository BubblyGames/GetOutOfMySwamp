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
            Destroy(this);
        }
    }

    public bool CheckIfCanBuild(RaycastHit hit, out Vector3Int intPos)
    {
        Vector3 pos = hit.point;
        pos -= hit.normal / 2;

        intPos = new Vector3Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z));

        CellInfo cell = LevelManager.instance.world.GetCell(intPos);

        Vector3 rayNormal = hit.normal;

        int x = Mathf.RoundToInt(rayNormal.x);
        int y = Mathf.RoundToInt(rayNormal.y);
        int z = Mathf.RoundToInt(rayNormal.z);

        Vector3Int normalInt = new Vector3Int(x, y, z);

        switch (cell.blockType)
        {
            case BlockType.Air:
                break;
            case BlockType.Path:
                return false;
            case BlockType.Grass:
                intPos += normalInt;
                break;
            case BlockType.Rock:
                intPos += normalInt;
                break;
            case BlockType.Swamp:
                return false;
            default:
                break;
        }

        //SelectCell(cell);
        selectedCell = cell;

        if (!canBuild)
            return false;

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

    public void BuildStructure(Vector3 position, Vector3 normal)
    {
        if (LevelStats.instance.infinteMoney)
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

    public void CreateTowerOnCell(Vector3 position, Vector3 normal)
    {
        Gatherer g;
        if (structureToBuild.structurePrefab.TryGetComponent<Gatherer>(out g))
        {
            bool canAdd = CubeWorldGenerator.worldGeneratorInstance.AddInterestPoint(new Vector3Int((int)position.x, (int)position.y, (int)position.z));
            if (!canAdd)
            {
                Debug.Log("Can't add");
                return;
            }
        }

        Structure structure = Instantiate(structureToBuild.structurePrefab, position, Quaternion.Euler(normal)).GetComponent<Structure>();
        structure.SetNormal(normal);

        //Not working
        if (selectedCell.GetStructure() != null)
        {
            Destroy(selectedCell.GetStructure());
        }
        selectedCell.SetStructure(structure);
    }

    public void UpgradeStructure()
    {
        if (LevelStats.instance.infinteMoney)
        {
            selectedCell.structure.UpgradeStrucrure();
        }
        if (LevelStats.instance.CurrentMoney >= structureToBuild.creationCost * Mathf.Pow(structureToBuild.upgradeMultiplicator, selectedCell.structure.GetLevel()))
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
        selectedStructure.Sell();
        selectedCell.structure = null;
        LevelStats.instance.EarnMoney(50);
    }
}
