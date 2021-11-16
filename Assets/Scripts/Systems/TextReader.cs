using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextReader:MonoBehaviour
{
    GameObject textManager;
    public string key;
    string text;
    public GameObject textContainer;
    private void Start()
    {
        if (GameObject.Find("GameManager").GetComponent<TextManager>() != null)
        {
            textManager = GameObject.Find("GameManager");
            Subscribe();
            Read();
        }

        if (gameObject.name.Equals("LenguageButton"))
        {
            gameObject.GetComponent<Button>().onClick.AddListener(ChangeLenguage);
        }
    }
    public void Subscribe()
    {
        textManager.GetComponent<TextManager>().Subscribe(gameObject, SceneManager.GetActiveScene().name);
    }

    public void Read()
    {
        textContainer.GetComponent<Text>().text = textManager.GetComponent<TextManager>().currentDictionary[key];
    }

    public void ChangeLenguage()
    {
        textManager.GetComponent<TextManager>().ChangeLenguage();
    }
}
