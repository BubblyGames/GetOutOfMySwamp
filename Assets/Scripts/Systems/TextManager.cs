using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager: MonoBehaviour
{
    private List<TextReader> subscribers;
    private bool english=true;

    public Dictionary<string, string> englishDictionary = new Dictionary<string, string>();
    public Dictionary<string, string> spanishDictionary = new Dictionary<string, string>();
    public Dictionary<string, string> currentDictionary;

    private void Start()
    {
        currentDictionary = englishDictionary;
    }

    public void UpdateSubscribers()
    {
        foreach(TextReader t in subscribers)
        {
            t.Read();
        }
    }

    public void Subscribe(TextReader reader)
    {
        subscribers.Add(reader);
    }

    public void ChangeLenguage()
    {
        if (english == false)
        {
            english = true;
            currentDictionary = englishDictionary;
        }
        else
        {
            english = false;
            currentDictionary = spanishDictionary;
        }
        UpdateSubscribers();
    }

}
