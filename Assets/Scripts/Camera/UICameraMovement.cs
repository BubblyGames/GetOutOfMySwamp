using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICameraMovement : MonoBehaviour
{
    Vector3 _position;
    Vector3 _actRot;
    int[] rotations;
    int side;// side in which the point is pointing to at the begining. 0 for top, 1 for bottom

    // Start is called before the first frame update
    void Start()
    {
        _position = gameObject.transform.position;
        _actRot = gameObject.transform.rotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void rotateToRight()
    {
        if (_actRot.z != 125)
        {
            gameObject.transform.position = gameObject.transform.position + new Vector3(-3, -2, 0);
            gameObject.transform.Rotate(0, 0, 125);
            _actRot = gameObject.transform.rotation.eulerAngles;
        }
        Debug.Log(_actRot);
    }

    public void rotateToLeft()
    {
        if (_actRot.z != 235)
        {
            gameObject.transform.position = gameObject.transform.position + new Vector3(3, 2, 0);
            gameObject.transform.Rotate(0, 0, -125);
            _actRot = gameObject.transform.rotation.eulerAngles;
        }
        Debug.Log(_actRot);
    }
}
