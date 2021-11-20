using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetFinalScore : MonoBehaviour
{
    public GameObject score;
    public GameObject container;
    void Start()
    {
        container.GetComponent<TextMeshProUGUI>().text = score.GetComponent<TextMeshProUGUI>().text;
    }
}
