using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextReader:MonoBehaviour
{
    GameObject textManager;
    public string key;
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
            Button b;
            if (gameObject.TryGetComponent<Button>(out b)) {
                b.onClick.AddListener(ChangeLenguage);
            }
            else
            {
                Debug.Log("ERROR");
            }
            
        }
        if (gameObject.name.Equals("ExitButton")|| gameObject.name.Equals("RestartButton") || gameObject.name.Equals("Select"))
        {
            Button b;
            if (gameObject.TryGetComponent<Button>(out b))
            {
                b.onClick.AddListener(EmptyLists);
            }
            else
            {
                Debug.Log("ERROR");
            }

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

    public void EmptyLists()
    {
        if (gameObject.name.Equals("RestartButton"))
        {
            textManager.GetComponent<TextManager>().emptyGameobjectsList(true);
        }
        else
        {
            textManager.GetComponent<TextManager>().emptyGameobjectsList(false);
        }
    
    }
}
