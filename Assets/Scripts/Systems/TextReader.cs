using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextReader:MonoBehaviour
{
    TextManager textManager;
    public string key;
    string text;
    public Text textContainer;
    private void Start()
    {

        if (GameObject.Find("TextManager") != null)
        {
            textManager = GameObject.Find("TextManager").GetComponent<TextManager>();
            Subscribe();
            Read();
        }
    }
    public void Subscribe()
    {
        textManager.Subscribe(this);
    }

    public void Read()
    {
        text = textManager.currentDictionary[key];
        textContainer.text = text;
    }
}
