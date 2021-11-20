using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int index = 0;

    public void SetupCard(int i, Material m, float height, float space, float y)
    {
        transform.localPosition = new Vector3(0, -i * (height + space) + y, -0.01f);
        transform.localRotation = Quaternion.identity;
        GetComponent<MeshRenderer>().material = m;
        index = i;

        if (GameManager.instance != null)
        {
            GameObject back = transform.GetChild(0).gameObject;
            back.GetComponent<MeshRenderer>().material.color = GameManager.instance.GetCurrentWorld().themeInfo.backGroundColor;
        }
    }
}
