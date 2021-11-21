using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;
    [SerializeField]
    private StructureBlueprint structureBlueprint = null; //Structure is going to be built
    public StructureBlueprint StructureBlueprint { get => structureBlueprint; set => structureBlueprint = value; }

    [SerializeField]
    private Structure selectedStructure; //Already Built structure 
    public Structure SelectedStructure { get => selectedStructure; set => selectedStructure = value; }

    [SerializeField]
    private CellInfo selectedCell;

    public List<Vector3Int> structureFundation;


    public bool canBuild;//Checks if a structure is selected to be built
    public Vector3 currentConstructionPositionOffset;


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
        structureFundation = new List<Vector3Int>();
    }

    public bool CheckIfCanBuild(RaycastHit hit, out Vector3 outPos)
    {
        //Position where structure will be build
        Vector3Int intPos = new Vector3Int(
            Mathf.RoundToInt(hit.point.x + (hit.normal.x / 2)),
            Mathf.RoundToInt(hit.point.y + (hit.normal.y / 2)),
            Mathf.RoundToInt(hit.point.z + (hit.normal.z / 2)));

        outPos = intPos;

        //Check if player has enough money
        if (!canBuild || (!CheatManager.instance.infiniteMoney && LevelStats.instance.CurrentMoney < structureBlueprint.creationCost))
            return false;

        //Spell will always be created
        SpellBehaviour sb;
        if (StructureBlueprint.structurePrefab.TryGetComponent<SpellBehaviour>(out sb))
            return true;

        //Position of the block under the block where the structure will be built
        Vector3Int intPosUnder = new Vector3Int(
            Mathf.RoundToInt(hit.point.x - (hit.normal.x / 2)),
            Mathf.RoundToInt(hit.point.y - (hit.normal.y / 2)),
            Mathf.RoundToInt(hit.point.z - (hit.normal.z / 2)));

        //For builidngs we need to check if the position is within the world bounds
        if (!LevelManager.instance.world.IsPosInBounds(Vector3Int.FloorToInt(intPosUnder)))
            return false;

        //Gets cell under structure
        selectedCell = LevelManager.instance.world.GetCell(intPosUnder);

        //If structure is bigger than one block
        int structureSize = structureBlueprint.structurePrefab.GetComponent<Structure>().Size;
        if (structureSize > 1)
        {
            Vector3Int cubeForward = Vector3Int.zero;

            if (hit.normal == Vector3Int.forward)
            {
                cubeForward = Vector3Int.right;
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
                cubeForward = Vector3Int.right;
            }
            else if (hit.normal == Vector3Int.right)
            {
                cubeForward = Vector3Int.up;
            }
            else if (hit.normal == Vector3Int.left)
            {
                cubeForward = Vector3Int.forward;
            }

            //empties fundation list cause ws dont want all of the past position just the ones behind the structure
            structureFundation.Clear();

            Vector3Int cubeRight = Vector3Int.FloorToInt(Vector3.Cross(hit.normal, cubeForward));

            for (int i = 0; i < structureSize; i++)
            {
                for (int j = 0; j < structureSize; j++)
                {
                    Vector3Int sizeChecker = intPosUnder + (cubeForward * i) + (cubeRight * j);
                    Vector3Int OnTopofSizeChecker = intPos + (cubeForward * i) + (cubeRight * j);

                    if (LevelManager.instance.world.CheckIfCanBuildInCell(sizeChecker, structureBlueprint.structurePrefab.GetComponent<Structure>().blockType, OnTopofSizeChecker))
                    {
                        //if it can be placed we delete dteh fundation list
                        structureFundation.Clear();
                        return false;
                    }
                    else
                        structureFundation.Add(Vector3Int.FloorToInt(sizeChecker)); // and if it is a good place to build we save those positions
                }
            }
            currentConstructionPositionOffset = (hit.normal + cubeForward + cubeRight) / structureSize;
        }
        else if (selectedCell.blockType != structureBlueprint.structurePrefab.GetComponentInChildren<Structure>().blockType)
        {
            currentConstructionPositionOffset = Vector3.zero;
            return false;
        }

        Gatherer g;
        bool isCloseToPath = true;
        foreach (Vector3 cellposition in structureFundation)
        {
            if (!LevelManager.instance.world.GetCell(Vector3Int.FloorToInt(cellposition)).isCloseToPath &&
            StructureBlueprint.structurePrefab.TryGetComponent<Gatherer>(out g))
            {
                //set not close to path
                isCloseToPath = false;
            }
            else
            {
                isCloseToPath = true;
                break;
            }
        }

        //if (g != null)
        //LevelManager.instance.world.AddInterestPoint(intPos);


        return isCloseToPath;
    }

    public void PlaceObject(RaycastHit hit)
    {
        Vector3 pos;
        if (CheckIfCanBuild(hit, out pos))
        {
            pos += currentConstructionPositionOffset;
            BuildStructure(pos, hit.normal);
        }
    }

    public void SetSelectedStructure(Structure structure)
    {
        SelectedStructure = structure;
        StructureBlueprint = SelectedStructure.Blueprint;
        selectedCell = null;

    }

    public int GetStructureSize()
    {
        return StructureBlueprint.structurePrefab.GetComponent<Structure>().Size;
    }

    public void SelectCell(CellInfo cell)
    {
        selectedCell = cell;
        SelectedStructure = null;
        StructureBlueprint = null;
    }

    public void SelectStructureToBuild(StructureBlueprint defense)
    {
        StructureBlueprint = defense;
        selectedCell = null;
        canBuild = true;
    }

    public void ResetCanBuild()
    {
        canBuild = false;
    }

    public void BuildStructure(Vector3 position, Vector3 normal)
    {
        if (CheatManager.instance.infiniteMoney)
        {
            CreateTowerOnCell(position, normal);
            ResetCanBuild(); // after building an structure you have to select another one to be able to place it
        }
        else if (LevelStats.instance.CurrentMoney >= StructureBlueprint.creationCost)
        {
            CreateTowerOnCell(position, normal);
            LevelStats.instance.SpendMoney(StructureBlueprint.creationCost);
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
        GameObject structureGO = Instantiate(StructureBlueprint.structurePrefab, position, Quaternion.Euler(normal));
        Structure structure = structureGO.GetComponentInChildren<Structure>();
        structure.gameObject.transform.localScale *= structure.Size;
        structure.SetNormal(normal);
        structure.Blueprint = StructureBlueprint;

        //THIS SHOULDN'T BE NECESSARY
        //if (!LevelManager.instance.world.IsPosInBounds(position))
        //    return;

        //CellInfo cell = LevelManager.instance.world.GetCell(position);//Sometimes FAILS :(

        //If we are putting a bomb, apart from creating the model, we set the cell's structure associated in which we are creating it
        Bomb b;
        if (structure.TryGetComponent<Bomb>(out b))
        {
            CellInfo cell = LevelManager.instance.world.GetCell((int)position.x, (int)position.y, (int)position.z);
            if (cell.GetStructure() == null)
            {
                cell.SetStructure(b);
            }
        }

        int structureSize = StructureBlueprint.structurePrefab.GetComponent<Structure>().Size;

        for (int i = 0; i < structureFundation.Count; i++)
        {
            LevelManager.instance.world.GetCell(structureFundation[0]).SetStructure(structure);
            structureFundation.Remove(structureFundation[0]);
        }

        //cell.SetStructure(structure);
    }

    public void UpgradeStructure()
    {
        if (SelectedStructure.GetLevel() < 3)
        {
            if (CheatManager.instance.infiniteMoney)
            {
                SelectedStructure.UpgradeStrucrure();
            }
            else if (LevelStats.instance.CurrentMoney >= SelectedStructure.Blueprint.upgrades[SelectedStructure.GetLevel()].cost)
            {
                LevelStats.instance.SpendMoney(StructureBlueprint.upgrades[SelectedStructure.GetLevel()].cost);
                SelectedStructure.UpgradeStrucrure(uIController);
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
        Debug.Log("Selling: " + SelectedStructure.name);
        UIController.instance.ShowMenu(UIController.GameMenu.Game);
        SelectedStructure.Sell();
        //selectedCell.structure = null;
        LevelStats.instance.EarnMoney(50);
    }
}
