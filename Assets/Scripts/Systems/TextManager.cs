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
        englishDictionary.Add("level1", "Pond");
        englishDictionary.Add("level2", "Tundra");
        englishDictionary.Add("level3", "Autumn");
        englishDictionary.Add("level4", "Cherry");
        englishDictionary.Add("level5", "Snow");
        englishDictionary.Add("sound", "Sound");
        englishDictionary.Add("lessons", "Lessons");
        englishDictionary.Add("levelInfoText", "Natural habitat of:");
        


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
        englishDictionary.Add("music", "Music by:");
        englishDictionary.Add("resources", "Resources");
        englishDictionary.Add("sounds", "SFX:");
        englishDictionary.Add("VFX", "Base VFX by:");


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
        englishDictionary.Add("short", "Short");

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
        englishDictionary.Add("description1", "<b><color=#C27203>Ali and Oli</color></b> are the <b><color=#C27203>cheapest</color></b> defense of them all. However, its <b><color=#C27203>medium</color></b> damage and range could save you when needed, so don't underestimate it!");
        englishDictionary.Add("lesson2", "Lesson 2");
        englishDictionary.Add("description2", "Did anyone say House? No, it's <b><color=#C27203>Beerhouse</color></b>! Combine this defense with others for amazing results thanks to its <b><color=#C27203>delaying effect</color></b> on enemies.");
        englishDictionary.Add("lesson3", "Lesson 3");
        englishDictionary.Add("description3", "<b><color=#C27203>Amazing damage</color></b> makes up for the low range and slow fire rate <b><color=#C27203>Batracius</color></b> does. Let's make the pond shake!");
        englishDictionary.Add("lesson4", "Lesson 4");
        englishDictionary.Add("description4", "Fighting? Nah. <b><color=#C27203>Broker</color></b> sees a <b><color=#C27203>golden future</color></b> for you. Just let it financiate it.");
        englishDictionary.Add("lesson5", "Lesson 5");
        englishDictionary.Add("description5", "<b><color=#C27203>Albatro</color></b> is always looking to the moon, but no <b><color=#C27203>flying enemies</color></b> will escape from it. Just place him <b><color=#C27203>on the rocks</color></b>, please.");
        englishDictionary.Add("lesson6", "Lesson 6");
        englishDictionary.Add("description6", "Is that... a mushroom? No, it's a <b><color=#C27203>MushBOOMB</color></b>! Be careful, it doesn't only <b><color=#C27203>destroys</color></b>, but it also has a <b><color=#C27203>delaying effect</color></b> on enemies.");
        englishDictionary.Add("lesson7", "Lesson 7");
        englishDictionary.Add("description7", "<b><color=#6100b4>Gepe</color></b>. <b><color=#6100b4>Medium</color></b> life and speed makes him an easy target. But be careful, don't forget about him!");
        englishDictionary.Add("lesson8", "Lesson 8");
        englishDictionary.Add("description8", "<b><color=#6100b4>Gunther</color></b> is used as shield by the other enemy units. Its <b><color=#6100b4>huge amount of health</color></b> won't make them easy to kill.");
        englishDictionary.Add("lesson9", "Lesson 9");
        englishDictionary.Add("description9", "<b><color=#6100b4>The Wilsons</color></b> are not brave enough to come alone. Besides, they are <b><color=#6100b4>very fast</color></b> so put an eye on them!");
        englishDictionary.Add("lesson10", "Lesson 10");
        englishDictionary.Add("description10", "From the <b><color=#6100b4>sky</color></b> another kind of dirty frogs are approaching, more difficult to reach, named <b><color=#6100b4>Doolittle</color></b>. <b><color=#6100b4>Get an Albatro quickly</color></b>, don't miss them!");

        //descriptions of tutorial in English
        englishDictionary.Add("tutorial1", "Build Defenses");
        englishDictionary.Add("Tdescription1", "<b><color=#C27203>Click and drag</color></b> a card from the right side to the world to <b><color=#C27203>build a new defense</color></b>. Before dropping it, the <b><color=#C27203>range</color></b> of the tower is shown.");
        englishDictionary.Add("tutorial2", "Upgrade Defenses");
        englishDictionary.Add("Tdescription2", "For <b><color=#C27203>upgrading</color></b> a single tower, click it and then <b><color=#C27203>press the upgrade button</color></b>. When you upgrade a tower, its statistics will increase, making it more efficient.");
        englishDictionary.Add("tutorial3", "The cube");
        englishDictionary.Add("Tdescription3", "The Cube and the enemies routes are <b><color=#C27203>generated procedurally</color></b> every time you play. The enemies will always appear in the <b><color=#C27203>lower face</color></b> of the Cube.");
        englishDictionary.Add("tutorial4", "Move the cube I");
        englishDictionary.Add("Tdescription4", "To <b><color=#C27203>rotate</color></b> the Cube, <b><color=#C27203>click and drag</color></b>.");
        englishDictionary.Add("tutorial5", "Move the cube II");
        englishDictionary.Add("Tdescription5", "You can also <b><color=#C27203>zoom in and out</color></b>, using the <b><color=#C27203>mouse wheel</color></b> on <b><color=#C27203>PC</color></b> or using <b><color=#C27203>two fingers</color></b> on <b><color=#C27203>mobile</color></b>.");
        englishDictionary.Add("tutorial6", "Reach the goal");
        englishDictionary.Add("Tdescription6", "The <b><color=#C27203>game's objective</color></b> is to <b><color=#C27203>defend your pond</color></b> from all the approching enemies. If they get in the pond, you'll lose some health. If you <b><color=#C27203>lose all of your health</color></b>, it's game over.");
        englishDictionary.Add("tutorial7", "Catapult");
        englishDictionary.Add("Tdescription7", "If you <b><color=#C27203>deploy your turrets</color></b> in a way that the enemies <b><color=#C27203>can't advance</color></b> in a route, they will throw a <b><color=#C27203>proyectile</color></b> to your towers, destroying them and also part of the cube.");

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
        spanishDictionary.Add("settings", "Configuración");
        spanishDictionary.Add("language", "Idioma");
        spanishDictionary.Add("exit", "Salir");
        spanishDictionary.Add("finish", "Finalizar");
        spanishDictionary.Add("restart", "Reiniciar");
        spanishDictionary.Add("pausedtext", "Pausa");
        spanishDictionary.Add("continue", "Continuar");
        spanishDictionary.Add("round", "RONDA");
        spanishDictionary.Add("nextLevel", "Siguiente nivel");
        spanishDictionary.Add("retry", "Reintentar");
        spanishDictionary.Add("loose", "¡Has perdido!");
        spanishDictionary.Add("win", "¡Has ganado!");
        spanishDictionary.Add("level1", "Pantano");
        spanishDictionary.Add("level2", "Tundra");
        spanishDictionary.Add("level3", "Otoño");
        spanishDictionary.Add("level4", "Cerezo");
        spanishDictionary.Add("level5", "Nevado");
        spanishDictionary.Add("sound", "Sonido");
        spanishDictionary.Add("lessons", "Lecciones");
        spanishDictionary.Add("levelInfoText", "Hábitat natural de:");

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
        spanishDictionary.Add("short", "Corto");

        //credits text in spanish
        spanishDictionary.Add("credits", "Créditos");
        spanishDictionary.Add("developed", "Desarrollado por:");
        spanishDictionary.Add("gonzalo", "Productor, Diseñador, Publicidad");
        spanishDictionary.Add("marta", "Artista");
        spanishDictionary.Add("alberto", "Diseñador");
        spanishDictionary.Add("daniel", "Programador");
        spanishDictionary.Add("adrian", "Programador, Publicidad");
        spanishDictionary.Add("joy", "Programador");
        spanishDictionary.Add("music", "Musica por:");
        spanishDictionary.Add("resources", "Recursos");
        spanishDictionary.Add("sounds", "SFX:");
        spanishDictionary.Add("VFX", "Base VFX por:");

        //structures names in spanish 
        spanishDictionary.Add("towerName", "Nombre Torre");
        spanishDictionary.Add("basicTower", "Flor y Fleta");
        spanishDictionary.Add("psiquicTower", "Beerhouse");
        spanishDictionary.Add("heavyTower", "Batracius");
        spanishDictionary.Add("bomb", "Petaseta");
        spanishDictionary.Add("moneyGatherer", "Especulio");
        spanishDictionary.Add("mountainTower", "Albatro");

        //descriptions of lessons in Spanish
        spanishDictionary.Add("lesson1", "Lección 1");
        spanishDictionary.Add("description1", "Las <b><color=#C27203>Hermanas Flor y Fleta</color></b> son la defensa más barata, pero no por ello la menos útil. Su nivel de <b><color=#C27203>ataque medio</color></b> combinado con su <b><color=#C27203>velocidad media de ataque</color></b> puede salvarte de muchos apuros.");
        spanishDictionary.Add("lesson2", "Lección 2");
        spanishDictionary.Add("description2", "<b><color=#C27203>Beerhouse</color></b> es la defensa perfecta contra enemigos rápidos. Aunque tiene poco rango y daño, su <b><color=#C27203>efecto ralentizador</color></b>, combinado con otras defensas, puede darte la victoria.");
        spanishDictionary.Add("lesson3", "Lección 3");
        spanishDictionary.Add("description3", "<b><color=#C27203>Batracius</color></b> cuenta con una <b><color=#C27203>velocidad de disparo muy lenta</color></b>, pero su alto alcance y su <b><color=#C27203>demoledor daño</color></b> directo pueden ser decisivos. Y aunque no de en el blanco, no pasa nada. El impacto tambien hace <b><color=#C27203>daño en área</color></b>.");
        spanishDictionary.Add("lesson4", "Lección 4");
        spanishDictionary.Add("description4", "<b><color=#C27203>Especulio</color></b> rehuye de combatir. Prefiere estar centrado en <b><color=#C27203>recaudar dinero</color></b> para mantener en funcionamiento las defensas ya construídas. Úsalo sabiamente.");
        spanishDictionary.Add("lesson5", "Lección 5");
        spanishDictionary.Add("description5", "<b><color=#C27203>Albatro</color></b> espera, colocado <b><color=#C27203>sobre las rocas</color></b> y <b><color=#C27203>apuntando al cielo</color></b>. Los enemigos deberán tener siempre los pies en la tierra cerca de él o sufrirán las consecuencias.");
        spanishDictionary.Add("lesson6", "Lección 6");
        spanishDictionary.Add("description6", "¡BUM! será lo último que escucharán tus enemigos. Además <b><color=#C27203>Petaseta</color></b> trae un 2x1 porque dejará a los enemigos <b><color=#C27203>aturdidos</color></b> y se moverán más lento (si sobreviven).");
        spanishDictionary.Add("lesson7", "Lección 7");
        spanishDictionary.Add("description7", "<b><color=#6100b4>Gepe</color></b> tiene una esperanza de <b><color=#6100b4>vida y velocidad promedias</color></b>, aunque no te confíes. Veremos cuanto aguanta contra nuestras defensas.");
        spanishDictionary.Add("lesson8", "Lección 8");
        spanishDictionary.Add("description8", "<b><color=#6100b4>Gunther</color></b> es <b><color=#6100b4>lento</color></b>, pesado, pero aguanta muy bien los golpes. Tiene <b><color=#6100b4>bastante vida</color></b> y sirve de escudo para el resto de enemigos, así que ten cuidado.");
        spanishDictionary.Add("lesson9", "Lección 9");
        spanishDictionary.Add("description9", "Dicen que los cobardes atacan en grupo. Los <b><color=#6100b4>Hermanos Zuleta</color></b> son esos cobardes. Tienen <b><color=#6100b4>poca vida</color></b>, pero son muy <b><color=#6100b4>escurridizas</color></b>. ¡Que no escape ni uno!");
        spanishDictionary.Add("lesson10", "Lección 10");
        spanishDictionary.Add("description10", "Desde el <b><color=#6100b4>cielo</color></b> se acerca otro tipo de rana sucia, más difícil de alcanzar. Su nombre es <b><color=#6100b4>Doolittle</color></b>, que no Stuart. Pilla un <b><color=#6100b4>Albatro</color></b> rápido, no las vayas a perder.");

        //tutorial spanish
        spanishDictionary.Add("tutorial1", "Construir defensas");
        spanishDictionary.Add("Tdescription1", "<b><color=#C27203>Presiona y arrastra</color></b> una de las tarjetas del lateral derecho en la posición que quieras del cubo. Antes de hacerlo, podrás observar su <b><color=#C27203>área de alcance</color></b>.");
        spanishDictionary.Add("tutorial2", "Mejorar defensas");
        spanishDictionary.Add("Tdescription2", "Para <b><color=#C27203>mejorar una torre</color></b>, tócala y presiona el <b><color=#C27203>botón de mejorar</color></b>. Al mejorar una torre aumentan las estadísticas de esta, volviéndose más eficaz.");
        spanishDictionary.Add("tutorial3", "El cubo");
        spanishDictionary.Add("Tdescription3", "El Cubo y los caminos de los enemigos se <b><color=#C27203>generan procedimentalmente</color></b> en cada partida. Los enemigos siempre aparecerán en la <b><color=#C27203>cara inferior</color></b> del cubo.");
        spanishDictionary.Add("tutorial4", "Mover el cubo I");
        spanishDictionary.Add("Tdescription4", "Para <b><color=#C27203>rotar el cubo, <b><color=#C27203>presiona y arrastra</color></b> usando el <b><color=#C27203>ratón</color></b> en el <b><color=#C27203>PC</color></b> o usando un <b><color=#C27203>dedo</color></b> en el <b><color=#C27203>móvil</color></b>.");
        spanishDictionary.Add("tutorial5", "Mover el cubo II");
        spanishDictionary.Add("Tdescription5", "Además, también puedes <b><color=#C27203>ampliar</color></b> el cubo usando la <b><color=#C27203>rueda del ratón</color></b> en el <b><color=#C27203>PC</color></b> o usando <b><color=#C27203>dos dedos</color></b> en el <b><color=#C27203>móvil</color></b>.");
        spanishDictionary.Add("tutorial6", "El objetivo");
        spanishDictionary.Add("Tdescription6", "El <b><color=#C27203>objetivo de la partida</color></b> consiste en <b><color=#C27203>evitar que tu vida llegue a 0</color></b> antes de acabar con todas las <b><color=#C27203>oleadas</color></b> de enemigos que se dirigen al estanque.");
        spanishDictionary.Add("tutorial7", "Catapulta");
        spanishDictionary.Add("Tdescription7", "En el caso de que crear un <b><color=#C27203>tapón de torres</color></b> de tal manera que los enemigos les cueste avanzar en un camino, estos lanzarán desde su estanque un <b><color=#C27203>proyectil</color></b>, rompiendo tus defensas y destruyendo parte del cubo.");
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
