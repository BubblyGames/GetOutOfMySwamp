using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    public DefenseBlueprint defenseToBuild = null; //Defense is going to be built

    public CellInfo selectedCell;
    
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

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                if (hit.collider.tag == "World")
                {
                    checkWorldCoordinates(hit);
                }
                else if(hit.collider.tag == "Structure")
                {
                    //Interact with existing defenses
                    selectedCell = hit.collider.gameObject.GetComponent<CellInfo>();
                }
            }
        }
    }

    //old SpawnWeapon
    private void checkWorldCoordinates(RaycastHit hit)
    {
        Vector3 pos = hit.point;
        pos -= hit.normal / 2;

        Vector3Int intPos = new Vector3Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), Mathf.RoundToInt(pos.z));

        CellInfo cell = LevelManager.instance.world.GetCell(intPos);

        Vector3 rayNormal = hit.normal;
        Vector3Int normal = new Vector3Int();

        float x = Mathf.Abs(rayNormal.x);
        float y = Mathf.Abs(rayNormal.y);
        float z = Mathf.Abs(rayNormal.z);

        if (x >= y && x >= z)
        {
            if (rayNormal.x > 0)
            {
                normal.x = 1;
            }
            else
            {
                normal.x = -1;
            }
        }
        else if (y >= x && y >= z)
        {
            if (rayNormal.y > 0)
            {
                normal.y = 1;
            }
            else
            {
                normal.y = -1;
            }
        }
        else
        {
            if (rayNormal.z > 0)
            {
                normal.z = 1;
            }
            else
            {
                normal.z = -1;
            }
        }

        switch (cell.blockType)
        {
            case BlockType.Air:
                break;
            case BlockType.Path:
                break;
            case BlockType.Grass:
                intPos += normal;
                break;
            case BlockType.Rock:
                intPos += LevelManager.instance.world.GetFaceNormal(cell);
                break;
            case BlockType.Swamp:
                return;
            default:
                break;
        }
        
        selectedCell = cell;
        if (!canBuild)
            return;

        BuildDefenseOn(intPos);

    }

    public void SelectDefenseToBuild(DefenseBlueprint defense)
    {
        defenseToBuild = defense;
    }

    public void ResetDefenseToBuild()
    {
        defenseToBuild = null;
    }


    public void BuildDefenseOn(Vector3 position)
    {
        if (LevelStats.instance.CurrentMoney >= defenseToBuild.cost )
        {
            Structure structure = Instantiate(defenseToBuild.defensePrefab, position, Quaternion.identity).GetComponent<Structure>();
            selectedCell.SetStructure(structure);
            ResetDefenseToBuild(); // after building an structure you hace to select another one to be able to place it
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
