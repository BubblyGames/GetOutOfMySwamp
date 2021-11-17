using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    [Header("MainScene Menus")]
    public GameObject tittleText;
    public GameObject playButton;
    public GameObject settingsMenu;
    public GameObject settingsButton;
    public GameObject tutorialMenu;
    public GameObject creditsMenu;
    public GameObject creditsButton;

    [Header("GameScene Menus")]
    public GameObject upgradeMenu;
    public GameObject upgradeButton;
    public Text damageText;
    public Text rangeText;
    public Text FRText;
    public Text TargetText;
    public GameObject sellButton;
    public GameObject shopMenu;
    public GameObject pauseMenu;
    public GameObject endgameMenu;
    public GameObject pauseButton;

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
        Tutorial,
        Credits
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
                if (upgradeMenu.activeSelf == false)
                {
                    upgradeMenu.SetActive(true);
                    pauseMenu.SetActive(false);
                }
                else
                {
                    upgradeMenu.SetActive(false);
                    pauseMenu.SetActive(false);
                }
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
                        playButton.SetActive(false);
                        settingsButton.SetActive(false);
                        tittleText.SetActive(false);
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
                        playButton.SetActive(true);
                        settingsButton.SetActive(true);
                        tittleText.SetActive(true);
                    }
                }
                break;
            case GameMenu.Tutorial:
                if (tutorialMenu.activeSelf == true)
                {
                    tutorialMenu.SetActive(false);
                    settingsMenu.SetActive(true);
                }
                else
                {
                    tutorialMenu.SetActive(true);
                    settingsMenu.SetActive(false);
                }
                break;
            case GameMenu.Credits:
                if (creditsMenu.activeSelf == true)
                {
                    creditsMenu.SetActive(false);
                    settingsMenu.SetActive(true);
                }
                else
                {
                    creditsMenu.SetActive(true);
                    settingsMenu.SetActive(false);
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

    public void CloseUpgrade()
    {
        ShowMenu(GameMenu.UpgradeMenu);
    }

    public void ShowTutorial()
    {
        ShowMenu(GameMenu.Tutorial);
    }

    public void ShowCredits()
    {
        ShowMenu(GameMenu.Credits);
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
                upgradeButton.SetActive(true);
                sellButton.SetActive(true);
                break;
            case "slow":
                upgradeMenu.GetComponent<Image>().sprite = slowTowerSprite;
                upgradeButton.SetActive(true);
                sellButton.SetActive(true);
                break;
            case "area":
                upgradeMenu.GetComponent<Image>().sprite = areaTowerSprite;
                upgradeButton.SetActive(true);
                sellButton.SetActive(true);
                break;
            case "bomb":
                upgradeMenu.GetComponent<Image>().sprite = bombTowerSprite;
                upgradeButton.SetActive(true);
                sellButton.SetActive(true);
                break;
            case "money":
                upgradeMenu.GetComponent<Image>().sprite = moneyStructureSprite;
                upgradeButton.SetActive(false);
                sellButton.SetActive(false);
                break;

        }

        switch (level)
        {
            case 0:
                UpdateUpgradeButton(0);
                break;
            case 1:
                UpdateUpgradeButton(1);
                break;
            case 2:
                UpdateUpgradeButton(2);
                break;
            case 3:
                UpdateUpgradeButton(3);
                break;
              
        }
    }
    
    public void UpdateUpgradeButton(int level)
    {
        upgradeButtonImage.sprite = upgradeLevels[level];
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

