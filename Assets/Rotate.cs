using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    float speed = 20;
    void Update()
    {
        //transform.Rotate(Vector3.up * Time.deltaTime * speed);
        transform.RotateAround(GetComponent<CubeWorldGenerator>().center.position,Vector3.up, Time.deltaTime * speed);
    }
}
