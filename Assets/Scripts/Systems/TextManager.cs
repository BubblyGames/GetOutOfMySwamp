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
        /*if(subscribersGameScene==null && subscribersMainScene == null)
        {*/
            subscribersMainScene = new List<GameObject>();
            subscribersGameScene = new List<GameObject>();
        //}
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
            /*if (subscribersMainScene == null)
            {
                subscribersMainScene = new List<GameObject>();
            }*/
            subscribersMainScene.Add(reader);
        }
        else
        {
            /*if(subscribersGameScene == null)
            {
                subscribersGameScene = new List<GameObject>();
            }*/
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
        englishDictionary.Add("nextLevel", "Next Level");
        englishDictionary.Add("retry", "Retry");
        englishDictionary.Add("loose", "You lose!");
        englishDictionary.Add("win", "You win!");
        englishDictionary.Add("level", "Level");
        englishDictionary.Add("sound", "Sound");
        englishDictionary.Add("lessons", "Lessons");


        //upgrade texts in english
        englishDictionary.Add("target", "Target");
        englishDictionary.Add("range", "Range");
        englishDictionary.Add("damage", "Damage");
        englishDictionary.Add("fireRate", "Fire Rate");
        englishDictionary.Add("moneyGathered", "Money +");
        englishDictionary.Add("sell", "Sell");
        englishDictionary.Add("upgrade", "Upgrade");

        //credits text in english
        englishDictionary.Add("credits", "Credits");
        englishDictionary.Add("developed", "Developed by:");
        englishDictionary.Add("gonzalo", "Producer, Designer, Marketing");
        englishDictionary.Add("marta", "Artist");
        englishDictionary.Add("alberto", "Designer");
        englishDictionary.Add("daniel", "Programmer");
        englishDictionary.Add("adrian", "Programmer, Marketing");
        englishDictionary.Add("joy", "Programmer");
        englishDictionary.Add("thanks", "Thanks to");


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
        englishDictionary.Add("basicTower", "Gepe");
        englishDictionary.Add("psiquicTower", "Beerhouse");
        englishDictionary.Add("heavyTower", "Batracius");
        englishDictionary.Add("bomb", "mushBOOMB");
        englishDictionary.Add("moneyGatherer", "Broker");
        englishDictionary.Add("mountainTower", "Albatro");

        //descriptions of lessons in English
        englishDictionary.Add("lesson1", "Lesson 1");
        englishDictionary.Add("description1", "Ali and Oli are the cheapest defense of them all. However, its medium damage and range could save you when needed, so don't underestimate it!");
        englishDictionary.Add("lesson2", "Lesson 2");
        englishDictionary.Add("description2", "Did anyone say House? No, it's Beerhouse! Combine this defense with others for amazing results thanks to its delaying effect on enemies.");
        englishDictionary.Add("lesson3", "Lesson 3");
        englishDictionary.Add("description3", "Amazing damage makes up for the low range and slow fire rate Batracius does. Let's make the pond shake!");
        englishDictionary.Add("lesson4", "Lesson 4");
        englishDictionary.Add("description4", "Fighting? Nah. Broker sees a golden future for you. Just let it financiate it.");
        englishDictionary.Add("lesson5", "Lesson 5");
        englishDictionary.Add("description5", "Albatro is always looking to the moon, but no flying enemies will escape from it. Just place him on the rocks, please.");
        englishDictionary.Add("lesson6", "Lesson 6");
        englishDictionary.Add("description6", "Is that... a mushroom? No, it's a MushBOOMB! Be careful, it doesn't only destroys, but it also has a delaying effect on enemies.");
        englishDictionary.Add("lesson7", "Lesson 7");
        englishDictionary.Add("description7", "Gepe. Medium life and speed makes him an easy target. But be careful, don't forget about him!");
        englishDictionary.Add("lesson8", "Lesson 8");
        englishDictionary.Add("description8", "Gunther is used as shield by the other enemy units. Its huge amount of health won't make them easy to kill.");
        englishDictionary.Add("lesson9", "Lesson 9");
        englishDictionary.Add("description9", "The Wilsons are not brave enough to come alone. Besides, they are very fast so put an eye on them!");
        englishDictionary.Add("lesson10", "Lesson 10");
        englishDictionary.Add("description10", "From the sky another kind of dirty frogs are approaching, more difficult to reach, named Doolittle. Get an Albatro quickly, don't miss them!");

        //descriptions of tutorial in English
        englishDictionary.Add("tutorial1", "Build Defenses");
        englishDictionary.Add("Tdescription1", "Click and drag a card from the right side to the world to build a new defense. Before dropping it, the range of the tower is shown.");
        englishDictionary.Add("tutorial2", "Upgrade Defenses");
        englishDictionary.Add("Tdescription2", "For upgrading a single tower, click it and then press the upgrade button. When you upgrade a tower, its statistics will increase, making it more efficient.");
        englishDictionary.Add("tutorial3", "The cube");
        englishDictionary.Add("Tdescription3", "The Cube and the enemies routes are generated procedurally every time you play. The enemies will always appear in the lower face of the Cube.");
        englishDictionary.Add("tutorial4", "Move the cube I");
        englishDictionary.Add("Tdescription4", "To rotate the Cube, click and drag.");
        englishDictionary.Add("tutorial5", "Move the cube II");
        englishDictionary.Add("Tdescription5", "You can also zoom in and out, using the mouse wheel on PC or using two fingers on mobile.");
        englishDictionary.Add("tutorial6", "Reach the goal");
        englishDictionary.Add("Tdescription6", "The game�s objective is to defend your pond from all the approching enemies. If they get in the pond, you'll lose some health. If you lose all of your health, it's game over.");

        //Spanish 
        spanishDictionary.Add("play", "Jugar");
        spanishDictionary.Add("next", "Siguiente");
        spanishDictionary.Add("back", "Volver");
        spanishDictionary.Add("previous", "Anterior");
        spanishDictionary.Add("select", "Empezar");
        spanishDictionary.Add("pause", "Pausa");
        spanishDictionary.Add("health", "Vida");
        spanishDictionary.Add("score", "Puntos");
        spanishDictionary.Add("money", "Dinero");
        spanishDictionary.Add("settings", "Configuraci�n");
        spanishDictionary.Add("language", "Idioma");
        spanishDictionary.Add("exit", "Salir");
        spanishDictionary.Add("finish", "Finalizar");
        spanishDictionary.Add("restart", "Reiniciar");
        spanishDictionary.Add("pausedtext", "Pausa");
        spanishDictionary.Add("continue", "Continuar");
        spanishDictionary.Add("round", "RONDA");
        spanishDictionary.Add("nextLevel", "Siguiente nivel");
        spanishDictionary.Add("retry", "Reintentar");
        spanishDictionary.Add("loose", "�Has perdido!");
        spanishDictionary.Add("win", "�Has ganado!");
        spanishDictionary.Add("level", "Nivel");
        spanishDictionary.Add("sound", "Sonido");
        spanishDictionary.Add("lessons", "Lecciones");

        //upgrade texts in spanish
        spanishDictionary.Add("target", "Objetivo");
        spanishDictionary.Add("range", "Rango");
        spanishDictionary.Add("damage", "Da�o");
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
        spanishDictionary.Add("fast", "R�pido");
        spanishDictionary.Add("high", "Alto");

        //credits text in spanish
        spanishDictionary.Add("credits", "Cr�ditos");
        spanishDictionary.Add("developed", "Desarrollado por:");
        spanishDictionary.Add("gonzalo", "Productor, Dise�ador, Publicidad");
        spanishDictionary.Add("marta", "Artista");
        spanishDictionary.Add("alberto", "Dise�ador");
        spanishDictionary.Add("daniel", "Programador");
        spanishDictionary.Add("adrian", "Programador, Publicidad");
        spanishDictionary.Add("joy", "Programador");
        spanishDictionary.Add("thanks", "Agradecimientos");

        //structures names in spanish 
        spanishDictionary.Add("towerName", "Nombre Torre");
        spanishDictionary.Add("basicTower", "Flor y Fleta");
        spanishDictionary.Add("psiquicTower", "Beerhouse");
        spanishDictionary.Add("heavyTower", "Batracius");
        spanishDictionary.Add("bomb", "Petaseta");
        spanishDictionary.Add("moneyGatherer", "Especulio");
        spanishDictionary.Add("mountainTower", "Albatro");

        //descriptions of lessons in Spanish
        spanishDictionary.Add("lesson1", "Lecci�n 1");
        spanishDictionary.Add("description1", "Las Hermanas Flor y Fleta son la defensa m�s barata, pero no por ello la menos �til. Su nivel de ataque medio combinado con su velocidad media de ataque puede salvarte de muchos apuros.");
        spanishDictionary.Add("lesson2", "Lecci�n 2");
        spanishDictionary.Add("description2", "Beerhouse es la defensa perfecta contra enemigos r�pidos. Aunque tiene poco rango y da�o, su efecto ralentizador, combinado con otras defensas, puede darte la victoria.");
        spanishDictionary.Add("lesson3", "Lecci�n 3");
        spanishDictionary.Add("description3", "Batracius cuenta con una velocidad de disparo muy lenta, pero su alto alcance y su demoledor da�o directo pueden ser decisivos. Y aunque no de en el blanco, no pasa nada. El impacto tambien hace da�o en �rea.");
        spanishDictionary.Add("lesson4", "Lecci�n 4");
        spanishDictionary.Add("description4", "Especulio rehuye de combatir. Prefiere estar centrado en recaudar dinero para mantener en funcionamiento las defensas ya constru�das. �salo sabiamente.");
        spanishDictionary.Add("lesson5", "Lecci�n 5");
        spanishDictionary.Add("description5", "Albatro espera, colocado sobre las rocas y apuntando al cielo. Los enemigos deber�n tener siempre los pies en la tierra cerca de �l o sufrir�n las consecuencias.");
        spanishDictionary.Add("lesson6", "Lecci�n 6");
        spanishDictionary.Add("description6", "�BUM! ser� lo �ltimo que escuchar�n tus enemigos. Adem�s Petaseta trae un 2x1 porque dejar� a los enemigos aturdidos y se mover�n m�s lento (si sobreviven).");
        spanishDictionary.Add("lesson7", "Lecci�n 7");
        spanishDictionary.Add("description7", "Gepe tiene una esperanza de vida media y velocidad promedias, aunque no te conf�es. Veremos cuanto aguanta contra nuestras defensas.");
        spanishDictionary.Add("lesson8", "Lecci�n 8");
        spanishDictionary.Add("description8", "Gunther es lento, pesado, pero aguanta muy bien los golpes. Sirve de escudo para el resto de enemigos asi que cuidado.");
        spanishDictionary.Add("lesson9", "Lecci�n 9");
        spanishDictionary.Add("description9", "Dicen que los cobardes atacan en grupo. Los Hermanos Zuleta son esos cobardes. Tienen poca vida, pero son muy escurridizas. �Que no escape ni uno!");
        spanishDictionary.Add("lesson10", "Lecci�n 10");
        spanishDictionary.Add("description10", "Desde el cielo se acerca otro tipo de rana sucia, m�s dif�cil de alcanzar. Su nombre es Doolittle, que no Stuart. Pilla un Albatro r�pido, no las vayas a perder.");

        //tutorial spanish
        spanishDictionary.Add("tutorial1", "Construir defensas");
        spanishDictionary.Add("Tdescription1", "Presiona y arrastra una de las tarjetas del lateral derecho en la posici�n que quieras del cubo. Antes de hacerlo, podr�s observar su �rea de alcance.");
        spanishDictionary.Add("tutorial2", "Mejorar defensas");
        spanishDictionary.Add("Tdescription2", "Para mejorar una torre, t�cala y presiona el bot�n de mejorar. Al mejorar una torre aumentan las estad�sticas de esta, volvi�ndose m�s eficaz.");
        spanishDictionary.Add("tutorial3", "El cubo");
        spanishDictionary.Add("Tdescription3", "El Cubo y los caminos de los enemigos se generan procedimentalmente en cada partida. Los enemigos siempre aparecer�n en la cara inferior del cubo.");
        spanishDictionary.Add("tutorial4", "Mover el cubo I");
        spanishDictionary.Add("Tdescription4", "Para rotar el cubo, presiona y arrastra usando el rat�n en el PC o usando un dedo en el m�vil.");
        spanishDictionary.Add("tutorial5", "Mover el cubo II");
        spanishDictionary.Add("Tdescription5", "Adem�s, tambi�n puedes ampliar el cubo usando la rueda del rat�n en el PC o usando dos dedos en el m�vil.");
        spanishDictionary.Add("tutorial6", "El objetivo");
        spanishDictionary.Add("Tdescription6", "El objetivo de la partida consiste en evitar que tu vida llegue a 0 antes de acabar con todas las oleadas de enemigos que se dirigen al estanque.");
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
