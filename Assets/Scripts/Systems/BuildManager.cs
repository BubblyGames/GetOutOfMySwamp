using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    private StructureBlueprint structureBlueprint = null; //Structure is going to be built
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

        if (!CheatManager.instance.infiniteMoney)
        {
            if (LevelStats.instance.CurrentMoney < structureBlueprint.creationCost)
                return false;
        }

        //Unnecessary ??
        Vector3Int intPosUnder = new Vector3Int(
            Mathf.RoundToInt(hit.point.x - (hit.normal.x / 2)),
            Mathf.RoundToInt(hit.point.y - (hit.normal.y / 2)),
            Mathf.RoundToInt(hit.point.z - (hit.normal.z / 2)));



        selectedCell = LevelManager.instance.world.GetCell(intPosUnder);
        Vector3Int cubeForward = Vector3Int.zero;
        
        if (hit.normal == Vector3Int.forward)
        {
            cubeForward = Vector3Int.up;
        }
        else if (hit.normal == Vector3Int.up)
        {
            cubeForward = Vector3Int.forward;
        }
        else if (hit.normal == Vector3Int.back)
        {
            cubeForward = Vector3Int.up;
        }
        else if (hit.normal == Vector3Int.down)
        {
            cubeForward = Vector3Int.forward;
        }
        else if(hit.normal == Vector3Int.right)
        {
            cubeForward = Vector3Int.up;
        }
        else if (hit.normal == Vector3Int.left)
        {
            cubeForward = Vector3Int.up;
        }


        Vector3Int cubeDotProduct = Vector3Int.FloorToInt(Vector3.Cross(hit.normal, cubeForward));

        for (int i = 0; i < structureBlueprint.structurePrefab.GetComponentInChildren<Structure>().Width; i++)
        {
            for (int j = 0; j < structureBlueprint.structurePrefab.GetComponentInChildren<Structure>().Width; j++)
            {
 
                Vector3Int sizeChecker = intPosUnder + (cubeForward * i) + (cubeDotProduct * j) ;
                Vector3Int OnTopofSizeChecker = intPos + (cubeForward * i) + (cubeDotProduct * j) ;
                if (!LevelManager.instance.world.CheckCell(sizeChecker, structureBlueprint.structurePrefab.GetComponentInChildren<Structure>().blockType, OnTopofSizeChecker))
                    return false;
            }
        }

        if (!canBuild || selectedCell.blockType != structureBlueprint.structurePrefab.GetComponentInChildren<Structure>().blockType)
            return false;

        Gatherer g;
        if (WorldManager.instance.IsPosInBounds(intPos.x, intPos.y, intPos.z) &&
            !LevelManager.instance.world.GetCell(intPos).isCloseToPath &&
            structureBlueprint.structurePrefab.TryGetComponent<Gatherer>(out g))
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
        structureBlueprint = selectedStructure.Blueprint;
        selectedCell = null;

    }

    public void SelectCell(CellInfo cell)
    {
        selectedCell = cell;
        selectedStructure = null;
        structureBlueprint = null;
    }

    public void SelectStructureToBuild(StructureBlueprint defense)
    {
        structureBlueprint = defense;
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
        else if (LevelStats.instance.CurrentMoney >= structureBlueprint.creationCost)
        {
            CreateTowerOnCell(position, normal);
            LevelStats.instance.SpendMoney(structureBlueprint.creationCost);
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
        GameObject structureGO = Instantiate(structureBlueprint.structurePrefab, position, Quaternion.Euler(normal));
        Debug.Log(position);
        Structure structure = structureGO.GetComponentInChildren<Structure>();
        structure.gameObject.transform.localScale *= structure.Width;
        structure.SetNormal(normal);
        structure.Blueprint = structureBlueprint;

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

        if (selectedStructure.GetLevel() < 3)
        {
            if (CheatManager.instance.infiniteMoney)
            {
                selectedCell.structure.UpgradeStrucrure();
            }
            else if ( LevelStats.instance.CurrentMoney >= structureBlueprint.upgrades[selectedStructure.GetLevel()].cost)
            {
                selectedStructure.UpgradeStrucrure();
                LevelStats.instance.SpendMoney(structureBlueprint.creationCost);

            }
            else
            {
                Debug.Log("Not enough Money");
                //TODO: Show on screen there is not enough money
            }
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
