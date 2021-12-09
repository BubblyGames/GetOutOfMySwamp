using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingAnimation : MonoBehaviour
{
    // Start is called before t

    private void Update()
    {
        if (gameObject.activeSelf) {
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + Time.deltaTime * 50f);
        }
    }

}
