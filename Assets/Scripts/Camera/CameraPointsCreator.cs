using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPointsCreator : MonoBehaviour
{
    int size;
    int _x, _y, _z;
    public GameObject gameManager;
    Transform center;
    List<GameObject> positions;
    public int distanceFromCube;

    bool finished = false;

    private void Awake()
    {
        size = gameManager.GetComponent<CubeWorldGenerator>().size;
        _x = size / 2;
        _y = _x;
        _z = _y;
    }
    // Start is called before the first frame update
    void Start()
    {
        center = LevelManager.instance.center;
        positions = new List<GameObject>();
        createPositions();
    }

    private void createPositions()
    {

        for (int i = 0; i < 8; i++)
        {
            GameObject _nPos = new GameObject("cameraPosition" + i);
            positions.Add(_nPos);
        }

        //set global positions acording to the center and the size
        //TOP VERTEX
        positions[0].transform.position = new Vector3(GetCenterPos().x + _x, GetCenterPos().y + _y, GetCenterPos().z - _z);
        positions[1].transform.position = new Vector3(GetCenterPos().x + _x, GetCenterPos().y + _y, GetCenterPos().z + _z);
        positions[2].transform.position = new Vector3(GetCenterPos().x - _x, GetCenterPos().y + _y, GetCenterPos().z + _z);
        positions[3].transform.position = new Vector3(GetCenterPos().x - _x, GetCenterPos().y + _y, GetCenterPos().z - _z);

        //BOT VERTEX
        positions[4].transform.position = new Vector3(GetCenterPos().x + _x, GetCenterPos().y - _y, GetCenterPos().z - _z);
        positions[5].transform.position = new Vector3(GetCenterPos().x + _x, GetCenterPos().y - _y, GetCenterPos().z + _z);
        positions[6].transform.position = new Vector3(GetCenterPos().x - _x, GetCenterPos().y - _y, GetCenterPos().z + _z);
        positions[7].transform.position = new Vector3(GetCenterPos().x - _x, GetCenterPos().y - _y, GetCenterPos().z - _z);

        for (int i = 0; i < 8; i++)
        {
            positions[i].transform.LookAt(center);
            positions[i].transform.Translate(0, 0, -distanceFromCube);
            positions[i].transform.position = new Vector3((int)positions[i].transform.position.x, (int)positions[i].transform.position.y, (int)positions[i].transform.position.z);
        }

        Camera.main.GetComponent<UICameraMovement>().startCamera(positions);
    }

    Vector3 GetCenterPos()
    {
        return center.position;
    }

    public List<GameObject> getCameraPositions()
    {
        return positions;
    }

    public bool pointCreationFinished()
    {
        return finished;
    }

}
