using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetFinalScore : MonoBehaviour
{
    public GameObject score;
    public GameObject container;
    void Start()
    {
        container.GetComponent<Text>().text = score.GetComponent<Text>().text;
    }
}
