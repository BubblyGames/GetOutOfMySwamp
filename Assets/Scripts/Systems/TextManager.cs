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

    /*Type of keys 
        Play,
        Pause,
        Score,
        Money,
        Settings,
        Exit,
        Lesson,
        Description
    */

    private void Awake()
    {
        InitializeDictionaries();
    }

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

    private void InitializeDictionaries()
    {
        //English 
        englishDictionary.Add("play", "Play");
        englishDictionary.Add("pause", "Pause");
        englishDictionary.Add("score", "Score");
        englishDictionary.Add("money", "Money");
        englishDictionary.Add("settings", "Settings");
        englishDictionary.Add("exit", "Exit");
        englishDictionary.Add("lesson", "Lesson");
        //descriptions of lessons in English
        englishDictionary.Add("description", "asadsafdsfdsfdsfsdfds");

        //Spanish 
        spanishDictionary.Add("play", "Jugar");
        spanishDictionary.Add("pause", "Pausa");
        spanishDictionary.Add("score", "Puntuación");
        spanishDictionary.Add("money", "Dinero");
        spanishDictionary.Add("settings", "Ajustes");
        spanishDictionary.Add("exit", "Sair");
        spanishDictionary.Add("lesson", "Lección");
        //descriptions of lessons in Spanish
        spanishDictionary.Add("description", "asadsafdsfdsfdsfsdfds");
    }

}
