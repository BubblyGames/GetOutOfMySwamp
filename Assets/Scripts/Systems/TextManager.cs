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
        englishDictionary.Add("round", "ROUND");
        englishDictionary.Add("credits", "Credits");
        englishDictionary.Add("nextLevel", "Next Level");
        englishDictionary.Add("retry", "Retry");
        englishDictionary.Add("loose", "You loose!");
        englishDictionary.Add("win", "You win!");


        //upgrade texts in english
        englishDictionary.Add("target", "Target");
        englishDictionary.Add("range", "Range");
        englishDictionary.Add("damage", "Damage");
        englishDictionary.Add("fireRate", "Fire Rate");
        englishDictionary.Add("moneyGathered", "Money Given");

        //defense stats in english
        englishDictionary.Add("air", "Air");
        englishDictionary.Add("ground", "Ground");
        englishDictionary.Add("groundAir", "Ground & Air");
        englishDictionary.Add("large", "Large");
        englishDictionary.Add("medium", "Medium");
        englishDictionary.Add("low", "Low");
        englishDictionary.Add("slow", "Slow");
        englishDictionary.Add("fast", "Fast");
        englishDictionary.Add("high", "High");

        //structures names in english 
        englishDictionary.Add("towerName", "Tower Name");
        englishDictionary.Add("basicTower", "Basic Frog");
        englishDictionary.Add("psiquicTower", "Psiquic Frog");
        englishDictionary.Add("areaTower", "Area Frog");
        englishDictionary.Add("bomb", "mushBOOMB");
        englishDictionary.Add("moneyGatherer", "Money Frog");

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
        spanishDictionary.Add("round", "RONDA");
        spanishDictionary.Add("credits", "Créditos");
        spanishDictionary.Add("nextLevel", "Siguiente nivel");
        spanishDictionary.Add("retry", "Reintentar");
        spanishDictionary.Add("loose", "¡Has perdido!");
        spanishDictionary.Add("win", "¡Has ganado!");

        //upgrade texts in spanish
        spanishDictionary.Add("target", "Objetivo");
        spanishDictionary.Add("range", "Rango");
        spanishDictionary.Add("damage", "Daño");
        spanishDictionary.Add("fireRate", "Velocidad Disparo");
        spanishDictionary.Add("moneyGathered", "Dinero Dado");


        //defense stats in spanish
        spanishDictionary.Add("air", "Aire");
        spanishDictionary.Add("ground", "Tierra");
        spanishDictionary.Add("groundAir", "Tierra y Aire");
        spanishDictionary.Add("large", "Largo");
        spanishDictionary.Add("medium", "Medio");
        spanishDictionary.Add("low", "Bajo");
        spanishDictionary.Add("slow", "Lento");
        spanishDictionary.Add("fast", "Rápido");
        spanishDictionary.Add("high", "Alto");

        //structures names in spanish 
        spanishDictionary.Add("towerName", "Nombre Torre");
        spanishDictionary.Add("basicTower", "Rana básica");
        spanishDictionary.Add("psiquicTower", "Rana Psíquica");
        spanishDictionary.Add("areaTower", "Rana de Area");
        spanishDictionary.Add("bomb", "Seta Bomba");
        spanishDictionary.Add("moneyGatherer", "Rana banquera");

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
