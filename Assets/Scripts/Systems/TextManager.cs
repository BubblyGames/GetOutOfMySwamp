using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TextManager : MonoBehaviour
{
    private List<GameObject> subscribersMainScene;
    private List<GameObject> subscribersGameScene;
    private bool english = true;

    public Dictionary<string, string> englishDictionary = new Dictionary<string, string>();
    public Dictionary<string, string> spanishDictionary = new Dictionary<string, string>();
    public Dictionary<string, string> currentDictionary;

    /*Type of keys 
        Play,  
        next,
        back,
        Pause,
        health,
        Score,
        Money,
        Settings,
        Exit,
        Lesson,
        Description,
        restart,
        pausedtext,
        continue
    */

    private void Awake()
    {
        InitializeDictionaries();
        subscribersMainScene = new List<GameObject>();
        subscribersGameScene = new List<GameObject>();
        currentDictionary = spanishDictionary;
        english = false;
    }

    public void UpdateSubscribers()
    {
        if (SceneManager.GetActiveScene().name.Equals("MainMenu"))
        {
            foreach (GameObject t in subscribersMainScene)
            {
                t.GetComponent<TextReader>().Read();
            }
        }
        else
        {
            foreach (GameObject t in subscribersGameScene)
            {
                t.GetComponent<TextReader>().Read();
            }
        }
    }

    public void Subscribe(GameObject reader, string sceneName)
    {
        if (sceneName.Equals("MainMenu"))
        {
            subscribersMainScene.Add(reader);
        }
        else
        {
            subscribersGameScene.Add(reader);
        }
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
        englishDictionary.Add("next", "Next");
        englishDictionary.Add("back", "Back");
        englishDictionary.Add("previous", "Previous");
        englishDictionary.Add("select", "Select World");
        englishDictionary.Add("pause", "Pause");
        englishDictionary.Add("health", "Health");
        englishDictionary.Add("score", "Score");
        englishDictionary.Add("money", "Money");
        englishDictionary.Add("settings", "Settings");
        englishDictionary.Add("lenguage", "Lenguage");
        englishDictionary.Add("exit", "Exit");
        englishDictionary.Add("finish", "Finish");
        englishDictionary.Add("restart", "Restart");
        englishDictionary.Add("pausedtext", "Pause Menu");
        englishDictionary.Add("continue", "Continue");

        //descriptions of lessons in English
        englishDictionary.Add("lesson1", "Lesson 1");
        englishDictionary.Add("description1", "asadsafdsfdsfdsfsdfds");
        englishDictionary.Add("lesson2", "Lesson 2");
        englishDictionary.Add("description2", "bbbbb");

        //Spanish 
        spanishDictionary.Add("play", "Jugar");
        spanishDictionary.Add("next", "Siguiente");
        spanishDictionary.Add("back", "Atrás");
        spanishDictionary.Add("previous", "Anterior");
        spanishDictionary.Add("select", "Seleccionar Mundo");
        spanishDictionary.Add("pause", "Pausa");
        spanishDictionary.Add("health", "Vida");
        spanishDictionary.Add("score", "Puntuación");
        spanishDictionary.Add("money", "Dinero");
        spanishDictionary.Add("settings", "Ajustes");
        spanishDictionary.Add("lenguage", "Idioma");
        spanishDictionary.Add("exit", "Salir");
        spanishDictionary.Add("finish", "Finalizar");
        spanishDictionary.Add("restart", "Reiniciar");
        spanishDictionary.Add("pausedtext", "Menu de Pausa");
        spanishDictionary.Add("continue", "Continuar");

        //descriptions of lessons in Spanish
        spanishDictionary.Add("lesson1", "Lección 1");
        spanishDictionary.Add("description1", "asadsafdsfdsfdsfsdfds");
        spanishDictionary.Add("lesson2", "Lección 2");
        spanishDictionary.Add("description2", "aaaa");
    }

    public void emptyGameobjectsList(bool restart)
    {
     
        if (SceneManager.GetActiveScene().name.Equals("MainMenu")||restart)
        {
            subscribersGameScene.Clear();
        }
        else
        {
            subscribersMainScene.Clear();
        }
    }

}
