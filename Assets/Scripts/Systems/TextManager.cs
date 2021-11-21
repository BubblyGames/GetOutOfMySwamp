using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TextManager : MonoBehaviour
{
    public static TextManager instance;

    private List<GameObject> subscribersMainScene;
    private List<GameObject> subscribersGameScene;
    private bool english = true;

    public Dictionary<string, string> englishDictionary = new Dictionary<string, string>();
    public Dictionary<string, string> spanishDictionary = new Dictionary<string, string>();
    public Dictionary<string, string> currentDictionary;
    public int currentLanguage;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeDictionaries();
        subscribersMainScene = new List<GameObject>();
        subscribersGameScene = new List<GameObject>();
        currentDictionary = spanishDictionary;
        currentLanguage = 1;
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

    public void ChangeLanguage(int languageid)
    {
        switch (languageid)
        {
            case 0:
                currentDictionary = englishDictionary;
                break;
            case 1:
                currentDictionary = spanishDictionary;
                break;
            default:
                currentDictionary = englishDictionary;
                break;
        }
        currentLanguage = languageid;
        UpdateSubscribers();
    }

    private void InitializeDictionaries()
    {
        //English 
        englishDictionary.Add("play", "Play");
        englishDictionary.Add("next", "Next");
        englishDictionary.Add("back", "Back");
        englishDictionary.Add("previous", "Previous");
        englishDictionary.Add("select", "Start");
        englishDictionary.Add("pause", "Pause");
        englishDictionary.Add("health", "Health");
        englishDictionary.Add("score", "Score");
        englishDictionary.Add("money", "Money");
        englishDictionary.Add("settings", "Settings");
        englishDictionary.Add("language", "Language");
        englishDictionary.Add("exit", "Exit");
        englishDictionary.Add("finish", "Finish");
        englishDictionary.Add("restart", "Restart");
        englishDictionary.Add("pausedtext", "Pause");
        englishDictionary.Add("continue", "Continue");
        englishDictionary.Add("round", "ROUND");
        englishDictionary.Add("credits", "Credits");
        englishDictionary.Add("nextLevel", "Next Level");
        englishDictionary.Add("retry", "Retry");
        englishDictionary.Add("loose", "You lose!");
        englishDictionary.Add("win", "You win!");
        englishDictionary.Add("level", "Level");
        englishDictionary.Add("sound", "Sound");


        //upgrade texts in english
        englishDictionary.Add("target", "Target");
        englishDictionary.Add("range", "Range");
        englishDictionary.Add("damage", "Damage");
        englishDictionary.Add("fireRate", "Fire Rate");
        englishDictionary.Add("moneyGathered", "Money +");
        englishDictionary.Add("sell", "Sell");
        englishDictionary.Add("upgrade", "Upgrade");

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
        englishDictionary.Add("heavyTower", "Heavy Frog");
        englishDictionary.Add("bomb", "mushBOOMB");
        englishDictionary.Add("moneyGatherer", "Money Frog");
        englishDictionary.Add("mountainTower", "Mountain Frog");

        //descriptions of lessons in English
        englishDictionary.Add("lesson1", "Lesson 1");
        englishDictionary.Add("description1", "The Basic Frog is the cheapest defense from all. However it's medium damage and range could save you when needed, son don't underestimate it!");
        englishDictionary.Add("lesson2", "Lesson 2");
        englishDictionary.Add("description2", "Did anyone said Psiquic Frog? Combine this defense with others for amazing results thanks to it's delaying effect on enemies.");
        englishDictionary.Add("lesson3", "Lesson 3");
        englishDictionary.Add("description3", "Amazing damage makes up for the low range and slow fire rate. Let's make the pond shake!");
        englishDictionary.Add("lesson4", "Lesson 4");
        englishDictionary.Add("description4", "Fighting? Nah. The Money Frog sees a golden future for you. Just let it financiate it.");
        englishDictionary.Add("lesson5", "Lesson 5");
        englishDictionary.Add("description5", "The Aerial Frog is always on the moon, but it is settle for destroying flying enemies.");
        englishDictionary.Add("lesson6", "Lesson 6");
        englishDictionary.Add("description6", "Is that... a mushroom? No, it's a MushBOOMB! Be careful, it not only destroys, it also has a delay effect on enemies.");
        englishDictionary.Add("lesson7", "Lesson 7");
        englishDictionary.Add("description7", "Basic Enemy unit. Medium life and speed makes it an easy target but be careful, don't forget about them");
        englishDictionary.Add("lesson8", "Lesson 8");
        englishDictionary.Add("description8", "Tank Enemy is used as a shield for the other enemy units. Be careful, it's huge amount of life won't make them easy to kill.");
        englishDictionary.Add("lesson9", "Lesson 9");
        englishDictionary.Add("description9", "Horde Enemies are not brave enough to come alone. Besides, they are very fast so put an eye on them!");

        //Spanish 
        spanishDictionary.Add("play", "Jugar");
        spanishDictionary.Add("next", "Siguiente");
        spanishDictionary.Add("back", "Volver");
        spanishDictionary.Add("previous", "Anterior");
        spanishDictionary.Add("select", "Empezar");
        spanishDictionary.Add("pause", "Pausa");
        spanishDictionary.Add("health", "Vida");
        spanishDictionary.Add("score", "Puntuación");
        spanishDictionary.Add("money", "Dinero");
        spanishDictionary.Add("settings", "Configuración");
        spanishDictionary.Add("language", "Idioma");
        spanishDictionary.Add("exit", "Salir");
        spanishDictionary.Add("finish", "Finalizar");
        spanishDictionary.Add("restart", "Reiniciar");
        spanishDictionary.Add("pausedtext", "Pausa");
        spanishDictionary.Add("continue", "Continuar");
        spanishDictionary.Add("round", "RONDA");
        spanishDictionary.Add("credits", "Créditos");
        spanishDictionary.Add("nextLevel", "Siguiente nivel");
        spanishDictionary.Add("retry", "Reintentar");
        spanishDictionary.Add("loose", "¡Has perdido!");
        spanishDictionary.Add("win", "¡Has ganado!");
        spanishDictionary.Add("level", "Nivel");
        spanishDictionary.Add("sound", "Sonido");

        //upgrade texts in spanish
        spanishDictionary.Add("target", "Objetivo");
        spanishDictionary.Add("range", "Rango");
        spanishDictionary.Add("damage", "Daño");
        spanishDictionary.Add("fireRate", "Vel. Disparo");
        spanishDictionary.Add("moneyGathered", "Dinero +");
        spanishDictionary.Add("sell", "Vender");
        spanishDictionary.Add("upgrade", "Mejora");


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
        spanishDictionary.Add("heavyTower", "Rana pesada");
        spanishDictionary.Add("bomb", "Seta Bomba");
        spanishDictionary.Add("moneyGatherer", "Rana banquera");
        spanishDictionary.Add("mountainTower", "Rana montañera");

        //descriptions of lessons in Spanish
        spanishDictionary.Add("lesson1", "Lección 1");
        spanishDictionary.Add("description1", "La rana básica es la defensa más barata, pero no por ello menos útil. Su nivel de ataque medio combinado con su velocidad media de ataque puede salvarte de muchos apuros.");
        spanishDictionary.Add("lesson2", "Lección 2");
        spanishDictionary.Add("description2", "La rana psíquica es la defensa perfecta contra enemigos rápidos. Aunque tiene poco rango y daño, su efecto ralentizador combinado con otras defensas puede darte la victoria.");
        spanishDictionary.Add("lesson3", "Lección 3");
        spanishDictionary.Add("description3", "La rana pesada cuenta con una velocidad de disparo muy lenta, pero su alto alcance y su demoledor daño pueden ser decisivos.");
        spanishDictionary.Add("lesson4", "Lección 4");
        spanishDictionary.Add("description4", "La rana banquera rehuye de combatir. Prefiere estar centrada en recaudar dinero para crear más defensas. Úsala sabiamente.");
        spanishDictionary.Add("lesson5", "Lección 5");
        spanishDictionary.Add("description5", "La rana aérea tiene la mira puesta en el horizonte. Los enemigos deberán tener siempre los pies en la tierra cerca de ella o sufrirán las consecuencias");
        spanishDictionary.Add("lesson6", "Lección 6");
        spanishDictionary.Add("description6", "¡BUM! será lo último que escucharán tus enemigos. Además esta seta mina trae un 2x1 porque dejará a los enemigos aturdidos y se moverán más lento.");
        spanishDictionary.Add("lesson7", "Lección 7");
        spanishDictionary.Add("description7", "La rana enemiga común, tiene una esperanza de vida media y velocidad promedias. Veremos cuanto aguantan contra nuestras defensas.");
        spanishDictionary.Add("lesson8", "Lección 8");
        spanishDictionary.Add("description8", "Los enemigos tanques son lentos, pero aguantan muy bien los golpes. Sirven de escudo para el resto de enemigos asi que cuidado.");
        spanishDictionary.Add("lesson9", "Lección 9");
        spanishDictionary.Add("description9", "Dicen que los cobardes atacan en grupo. Las ranas que van en hordas tienen poca vida, pero son muy escurridizas.¡Que no escape ni una!");
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
