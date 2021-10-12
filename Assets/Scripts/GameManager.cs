using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CubeWorldGenerator))]
/*This manager inicialice the game */
//TODO: SPLIT MAP GENERATION AND PATH GENERATION INTO COMPONENTS 
public class GameManager : MonoBehaviour
{
    public GameObject weaponPrefab;
    public GameObject enemyPrefab;

    private CubeWorldGenerator world;

    public Text text;
    public LayerMask floorLayer;
    public Transform center;
    // Start is called before the first frame update

    private void Awake()
    {
        world = GetComponent<CubeWorldGenerator>();
        center.transform.position = Vector3.one * (world.size / 2);
    }

    private void Update()
    {
        //transform.Rotate(Vector3.forward,Time.deltaTime*10);
        text.text = (1 / Time.deltaTime).ToString();

        if (Input.GetMouseButtonDown(0))
        {
            SpawnWeapon();
        }

        for (int i = 0; i < world.nPaths; i++)
        {
            if (world.paths[i] != null && world.paths[i].CheckSpawn())
            {
                //GameObject.Instantiate(enemyPrefab, world.paths[i].GetStep(0), Quaternion.identity).GetComponent<EnemyBehaviour>().SetPath(world.paths[i]);
            }
        }
    }

    private void SpawnWeapon()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.point);
            Vector3Int pos = new Vector3Int(Mathf.RoundToInt(hit.point.x), Mathf.RoundToInt(hit.point.y), Mathf.RoundToInt(hit.point.z));

            CellInfo cell = world.GetCell(pos);

            Vector3 rayNormal = hit.normal;
            Vector3Int normal = new Vector3Int();

            float x = Mathf.Abs(rayNormal.x);
            float y = Mathf.Abs(rayNormal.y);
            float z = Mathf.Abs(rayNormal.z);


            if (x >= y && x >= z)
            {
                if(rayNormal.x > 0) {
                    normal.x = 1;
                } else
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

            

            if (cell.blockType == BlockType.Grass)
            {
                pos += normal;
                cell = world.GetCell(pos);
            }

            if (cell.blockType == BlockType.Rock)
            {
                pos += normal;
            }

            GameObject.Instantiate(weaponPrefab, pos, Quaternion.identity);
        }
    }
}








