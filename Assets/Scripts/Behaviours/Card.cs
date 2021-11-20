using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int index = 0;

    public void SetupCard(int i, Texture t, float height, float space, float y)
    {
        transform.localPosition = new Vector3(0, -i * (height + space) + y, -0.01f);
        transform.localRotation = Quaternion.identity;
        index = i;

        Material newMaterial = new Material(GetComponent<MeshRenderer>().material);

        newMaterial.mainTexture = t;
        GetComponent<MeshRenderer>().material = newMaterial;

        if (GameManager.instance != null)
        {
            GameObject back = transform.GetChild(0).gameObject;
            back.GetComponent<MeshRenderer>().material.color = GameManager.instance.GetCurrentWorld().themeInfo.backGroundColor;
        }
    }
}
