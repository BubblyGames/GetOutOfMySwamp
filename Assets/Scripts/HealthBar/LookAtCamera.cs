using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(CameraBehaviour.instance.gameObject.transform.position - transform.position);
    }
}
