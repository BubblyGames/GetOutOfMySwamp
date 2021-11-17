using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    [Header("Menus")]
    public GameObject upgradeMenu;
    public GameObject upgradeButton;
    public GameObject shopMenu;
    public GameObject pauseMenu;
    public GameObject endgameMenu;
    public GameObject pauseButton;
    public GameObject settingsMenu;
    public GameObject settingsButton;
    public GameObject tutorialMenu;

    [Header("UpgradeMenu Sprites")]
    public Sprite basicTowerSprite;
    public Sprite slowTowerSprite;
    public Sprite areaTowerSprite;
    public Sprite bombTowerSprite;
    public Sprite moneyStructureSprite;
    public List<Sprite> upgradeLevels;


    private int gameSceneId;
    private Image upgradeButtonImage;
    public enum GameMenu
    {
        UpgradeMenu,
        PauseMenu,
        EndgameMenu,
        Game,
        Settings,
        Tutorial
    }

    public GameMenu selectedMenu;

    public Text FPSText;

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

        gameSceneId = SceneManager.GetActiveScene().buildIndex;
        upgradeButtonImage=upgradeButton.GetComponent<Image>();
}

    public virtual void ShowMenu(GameMenu menu)
    {
        //Shows the selected manu and hides all other menus
        //Also sets up time scale
        switch (menu)
        {
            case GameMenu.UpgradeMenu:
                upgradeMenu.SetActive(true);
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
                break;
            case GameMenu.PauseMenu:
                pauseMenu.SetActive(true);
                upgradeMenu.SetActive(false);
                pauseButton.SetActive(false);
                Time.timeScale = 0;
                break;
            case GameMenu.EndgameMenu:
                endgameMenu.SetActive(true);
                upgradeMenu.SetActive(false);
                pauseMenu.SetActive(false); 
                pauseButton.SetActive(false);
                settingsMenu.SetActive(false);
                settingsButton.SetActive(false);
                GameObject.Find("FinalScoreText").GetComponent<UnityEngine.UI.Text>().text = "Score: " + LevelStats.instance.currentScore;
                Time.timeScale = 1;
                break;
            case GameMenu.Game:
                shopMenu.SetActive(true);
                upgradeMenu.SetActive(false);
                pauseMenu.SetActive(false);
                endgameMenu.SetActive(false);
                pauseButton.SetActive(true);
                Time.timeScale = 1;
                break;
            case GameMenu.Settings:
                if (settingsMenu.activeSelf == false)
                {
                    if (LevelManager.instance != null)
                    {
                        settingsMenu.SetActive(true);
                        settingsButton.SetActive(false);
                        upgradeMenu.SetActive(false);
                        pauseMenu.SetActive(false);
                        endgameMenu.SetActive(false);
                        pauseButton.SetActive(false);
                        tutorialMenu.SetActive(false);
                        Time.timeScale = 0;
                    }
                    else
                    {
                        settingsMenu.SetActive(true);
                        settingsButton.SetActive(false);
                    }
                }
                else
                {
                    if (LevelManager.instance != null)
                    {
                        settingsMenu.SetActive(false);
                        settingsButton.SetActive(true);
                        upgradeMenu.SetActive(false);
                        pauseMenu.SetActive(false);
                        endgameMenu.SetActive(false);
                        pauseButton.SetActive(true);
                        tutorialMenu.SetActive(false);
                        Time.timeScale = 1;
                    }
                    else
                    {
                        settingsMenu.SetActive(false);
                        settingsButton.SetActive(true);
                    }
                }
                break;
            case GameMenu.Tutorial:
                if (tutorialMenu.activeSelf == true)
                {
                    tutorialMenu.SetActive(false);
                }
                else
                {
                    tutorialMenu.SetActive(true);
                }
                break;
            default:
                break;
        }
    }

    internal void ShowSelectedMenu()
    {
        //Shows selected menu
        ShowMenu(selectedMenu);
    }

    public void Restart()
    {
        //Restarts game
        Time.timeScale = 1;
        if (SceneController.instance)
        {
            SceneController.instance.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void Exit()
    {
        //Goes back to main menu
        Time.timeScale = 1;
        if (SceneController.instance)
        {
            SceneController.instance.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    public void GoToNextLevel()
    {
        //Goes to next level
        Time.timeScale = 1;
        if (SceneController.instance && GameManager.instance.IncreaseCurrentWorldId())
        {
            SceneController.instance.LoadScene(1);
        }
        else
        {
            SceneController.instance.LoadScene(0);
        }
    }

    public void SlowGame()
    {
        //TODO try to slow to stop game when gameover
    }

    public void Pause()
    {
        ShowMenu(GameMenu.PauseMenu);
    }

    public void Continue()
    {
        ShowMenu(GameMenu.Game);
    }

    public void EnterSettings()
    {
        ShowMenu(GameMenu.Settings);
    }

    public void CloseSettings()
    {
        ShowMenu(GameMenu.Settings);
    }

    public void ShowTutorial()
    {
        ShowMenu(GameMenu.Tutorial);
    }


    private void Update()
    {
        if(LevelManager.instance!=null)
        FPSText.text = Mathf.Round((1 / Time.deltaTime)).ToString(); //FpS 
    }

    public void SetUpgradeMenu(string structureName, int level)
    {
        switch (structureName)
        {
            case "basic":
                upgradeMenu.GetComponent<Image>().sprite = basicTowerSprite;
                break;
            case "slow":
                upgradeMenu.GetComponent<Image>().sprite = slowTowerSprite;
                break;
            case "area":
                upgradeMenu.GetComponent<Image>().sprite = areaTowerSprite;
                break;
            case "bomb":
                upgradeMenu.GetComponent<Image>().sprite = bombTowerSprite;
                break;

        }

        switch (level)
        {
            case 0:
                upgradeButtonImage.sprite = upgradeLevels[0];
                break;
            case 1:
                upgradeButtonImage.sprite = upgradeLevels[1];
                break;
            case 2:
                upgradeButtonImage.sprite = upgradeLevels[2];
                break;
            case 3:
                upgradeButtonImage.sprite = upgradeLevels[3];
                upgradeButton.SetActive(false);
                break;
              
        }
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(UIController))]
public class UIControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        UIController uIcontroller = (UIController)target;
        DrawDefaultInspector();
        if (GUILayout.Button("Enable"))
        {
            uIcontroller.ShowSelectedMenu();
        }
    }
}
#endif

