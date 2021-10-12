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
    // Start is called before the first frame update

    private void Awake()
    {
        world = GetComponent<CubeWorldGenerator>();
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
                GameObject.Instantiate(enemyPrefab, world.paths[i].GetStep(0), Quaternion.identity).GetComponent<EnemyBehaviour>().SetPath(world.paths[i]);
            }
        }
    }

    private void SpawnWeapon()
    {
        GameObject obj = CastRay();

        if (obj != null)
        {
            int x = Mathf.RoundToInt(obj.transform.position.x);
            int y = Mathf.RoundToInt(obj.transform.position.y);
            Debug.Log(x);
            Debug.Log(y);

            /*if (world.cells[x, y].state == 1)
            {
                GameObject.Instantiate(weaponPrefab, obj.transform.position - Vector3.forward, Quaternion.identity);
                world.cells[x, y].state = 2;
            }*/

        }
    }

    GameObject CastRay()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray.origin, ray.direction, Mathf.Infinity, floorLayer);// .Raycast(ray.origin, ray.direction, Mathf.Infinity);
        if (hits.Length > 0 && hits[0].collider != null)
        {
            return hits[0].collider.gameObject;
        }
        return null;
    }
}








