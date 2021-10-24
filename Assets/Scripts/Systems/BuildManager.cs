using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Update()
    { 
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit = new RaycastHit();

            // Bit shift the index of the layer (8: Structures) to get a bit mask
            int layerMask = 1 << 8;
            // But instead we want to collide against everything except layer 8.The ~ operator does this, it inverts a bitmask.
            layerMask = ~layerMask;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.tag == "World")
                {
                    checkWorldCoordinates(hit);
                    
                }

            }
            else if (hit.collider == UIController.instance.upgradeMenu)//if raycast doesnt hit
            {

                Debug.Log("Menu");

            }
            else if (hit.collider == null)//if raycast doesnt hit
            {

                Debug.Log("DidntHit");
                //UIController.instance.DisableUpdateMenu();
            }
           
        }
    }

    internal void SetSelectedStructure(Structure structure)
    {
        selectedStructure = structure;
    }

    private void checkWorldCoordinates(RaycastHit hit)
    {
        Vector3 pos = hit.point;
        pos -= hit.normal / 2;

        Vector3Int intPos = new Vector3Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z));

        CellInfo cell = LevelManager.instance.world.GetCell(intPos);

        Vector3 rayNormal = hit.normal;

        int x = Mathf.RoundToInt(rayNormal.x);
        int y = Mathf.RoundToInt(rayNormal.y);
        int z = Mathf.RoundToInt(rayNormal.z);

        Vector3Int normalInt = new Vector3Int(x,y,z);


        switch (cell.blockType)
        {
            case BlockType.Air:
                break;
            case BlockType.Path:
                return;
            case BlockType.Grass:
                intPos += normalInt;
                break;
            case BlockType.Rock:
                intPos += normalInt;
                break;
            case BlockType.Swamp:
                return;
            default:
                break;
        }


        //SelectCell(cell);
        selectedCell = cell;
        Debug.Log("Hit World");

        if (!canBuild)
            return;

        BuildStructure(intPos, rayNormal);

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
            CreateTowerOnCell(position,normal);
            ResetCanBuild(); // after building an structure you have to select another one to be able to place it
        }
        else if (LevelStats.instance.CurrentMoney >= structureToBuild.creationCost )
        {

            CreateTowerOnCell(position,normal);
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
