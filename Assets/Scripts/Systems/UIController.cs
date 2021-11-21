using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    [Header("MainScene Menus")]
    public GameObject playButton;
    public GameObject settingsMenu;
    public GameObject settingsButton;
    public GameObject tutorialMenu;
    public GameObject gameTutorialMenu;
    public GameObject creditsMenu;
    public GameObject creditsButton;
    

    [Header("GameScene Menus")]
    public GameObject upgradeMenu;
    public GameObject upgradeButton;
    public GameObject sellButton;
    public GameObject shopMenu;
    public GameObject pauseMenu;
    public GameObject endgameMenuLoose;
    public GameObject endgameMenuWin;
    public GameObject pauseButton;
    public List<GameObject> fixedTexts;
    public List<GameObject> statsTexts;
    public GameObject towerName;
    public GameObject upgradeCostText;
    public TextMeshProUGUI numberOfWaves;
    public TextMeshProUGUI currentWave;

    [Header("UpgradeMenu Sprites")]
    public Sprite basicTowerSprite;
    public Sprite psiquicTowerSprite;
    public Sprite heavyTowerSprite;
    public Sprite bombTowerSprite;
    public Sprite moneyStructureSprite;
    public Sprite aerialStructureSprite;
    public List<Sprite> upgradeLevels;

    public GameObject shopContainer;
    private int gameSceneId;
    public Image upgradeButtonImage;
    private List<StructureBlueprint> structures;

    public enum GameMenu
    {
        UpgradeMenu,
        PauseMenu,
        EndgameMenuLoose,
        EndgameMenuWin,
        Game,
        Settings,
        Tutorial,
        GameTutorial,
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
        if (gameSceneId == 1)
        {
            upgradeButtonImage = upgradeButton.GetComponent<Image>();
            Debug.Log("Image:" + upgradeButtonImage);
            structures = shopContainer.GetComponent<Shop>().defenseBlueprints;
        }

    }

    private void Start()
    {
        LevelManager.OnWaveCleared += UpdateWaveText;
        LevelManager.OnGameStart += SetWaveText;
        LevelManager.OnStructureUpgraded += SetUpgradeButton;
    }


    private void SetUpgradeButton(int level)
    {
        UpdateUpgradeButton(level, BuildManager.instance.SelectedStructure.structureId);
    }

    private void SetWaveText()
    {
        numberOfWaves.text = WaveController.instance.waves.Length.ToString();
        currentWave.text = (WaveController.instance.waveCount + 1).ToString();
    }

    public void UpdateWaveText()
    {
        currentWave.GetComponent<TextMeshProUGUI>().text = (WaveController.instance.waveCount + 1).ToString();
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

                break;
            case GameMenu.PauseMenu:
                pauseMenu.SetActive(true);
                upgradeMenu.SetActive(false);
                pauseButton.SetActive(false);
                Time.timeScale = 0;
                break;
            case GameMenu.EndgameMenuLoose:
                endgameMenuLoose.SetActive(true);
                upgradeMenu.SetActive(false);
                pauseMenu.SetActive(false);
                pauseButton.SetActive(false);
                Time.timeScale = 1;
                break;
            case GameMenu.EndgameMenuWin:
                endgameMenuWin.SetActive(true);
                upgradeMenu.SetActive(false);
                pauseMenu.SetActive(false);
                pauseButton.SetActive(false);
                GameObject.Find("FinalScoreText").GetComponent<TextMeshProUGUI>().text = LevelStats.instance.currentScore.ToString();
                Time.timeScale = 1;
                break;
            case GameMenu.Game:
                shopMenu.SetActive(true);
                upgradeMenu.SetActive(false);
                pauseMenu.SetActive(false);
                endgameMenuLoose.SetActive(false);
                endgameMenuWin.SetActive(false);
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
                        endgameMenuLoose.SetActive(false);
                        endgameMenuWin.SetActive(false);
                        pauseButton.SetActive(false);
                        tutorialMenu.SetActive(false);
                        gameTutorialMenu.SetActive(false);
                        Time.timeScale = 0;
                    }
                    else
                    {
                        settingsMenu.SetActive(true);
                        playButton.SetActive(false);
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
                        endgameMenuLoose.SetActive(false);
                        endgameMenuWin.SetActive(false);
                        pauseButton.SetActive(true);
                        tutorialMenu.SetActive(false);
                        gameTutorialMenu.SetActive(false);
                        Time.timeScale = 1;
                    }
                    else
                    {
                        settingsMenu.SetActive(false);
                        playButton.SetActive(true);
                        settingsButton.SetActive(true);
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
            case GameMenu.GameTutorial:
                if (gameTutorialMenu.activeSelf == true)
                {
                    gameTutorialMenu.SetActive(false);
                    settingsMenu.SetActive(true);
                }
                else
                {
                    gameTutorialMenu.SetActive(true);
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
        upgradeMenu.SetActive(false);
    }

    public void ShowTutorial()
    {
        ShowMenu(GameMenu.Tutorial);
    }

    public void ShowGameTutorial()
    {
        ShowMenu(GameMenu.GameTutorial);
    }
    public void ShowCredits()
    {
        ShowMenu(GameMenu.Credits);
    }


    private void Update()
    {
        if (LevelManager.instance != null)
            FPSText.text = Mathf.Round((1 / Time.deltaTime)).ToString(); //FpS 
    }


    public void SetUpgradeMenu(Structure structure)
    {
        towerName.GetComponent<TextReader>().SetKey(structure.structureName);
        if (structure.structureId != 4)
        {
            activateUpgradeTexts();
            fixedTexts[0].GetComponent<TextReader>().SetKey("target");
            statsTexts[0].GetComponent<TextReader>().SetKey(structure.Blueprint.targetDescription);
            statsTexts[1].GetComponent<TextReader>().SetKey(structure.Blueprint.rangeDescription);
            statsTexts[2].GetComponent<TextReader>().SetKey(structure.Blueprint.fireRateDescription);
            statsTexts[3].GetComponent<TextReader>().SetKey(structure.Blueprint.damageDescription);

        }
        else
        {
            desactivateUpgradeTexts();
            fixedTexts[0].SetActive(true);
            statsTexts[0].SetActive(true);
            fixedTexts[0].GetComponent<TextReader>().SetKey("moneyGathered");
            statsTexts[0].GetComponent<TextMeshProUGUI>().text = structure.gameObject.GetComponent<Gatherer>().TotalResourceGathered.ToString();
        }

        switch (structure.structureId)
        {
            case 0:
                upgradeMenu.GetComponent<Image>().sprite = basicTowerSprite;
                break;
            case 1:
                upgradeMenu.GetComponent<Image>().sprite = psiquicTowerSprite;
                break;
            case 2:
                upgradeMenu.GetComponent<Image>().sprite = heavyTowerSprite;
                break;
            case 3:
                upgradeMenu.GetComponent<Image>().sprite = bombTowerSprite;
                break;
            case 4:
                upgradeMenu.GetComponent<Image>().sprite = moneyStructureSprite;
                break;
        }
        upgradeButton.SetActive(true);
        sellButton.SetActive(true);
        UpdateUpgradeButton(structure.GetLevel(), structure.structureId);

        if (structure.structureId == 4)
        {
            UpdateUpgradeButton(3, structure.structureId);
        }
    }
    /* public void SetUpgradeMenu(int structureId, string structureName, int level, string target, string range, string fireRate, string damage, int moneyGiven)
     {
         towerName.GetComponent<TextReader>().SetKey(structureName);
         if (structureId!=4)
         {
             activateUpgradeTexts();
             fixedTexts[0].GetComponent<TextReader>().SetKey("target");
             statsTexts[0].GetComponent<TextReader>().SetKey(target);
             statsTexts[1].GetComponent<TextReader>().SetKey(range);
             statsTexts[2].GetComponent<TextReader>().SetKey(fireRate);
             statsTexts[3].GetComponent<TextReader>().SetKey(damage);

         }
         else
         {
             desactivateUpgradeTexts();
             fixedTexts[0].SetActive(true);
             statsTexts[0].SetActive(true);
             fixedTexts[0].GetComponent<TextReader>().SetKey("moneyGathered");
             statsTexts[0].GetComponent<TextMeshProUGUI>().text = moneyGiven.ToString();
         }

         switch (structureId)
         {
             case 0:
                 upgradeMenu.GetComponent<Image>().sprite = basicTowerSprite;
                 upgradeButton.SetActive(true);
                 sellButton.SetActive(true);
                 switch (level)
                 {
                     case 0:
                         UpdateUpgradeButton(0, 0);
                         break;
                     case 1:
                         UpdateUpgradeButton(1, 0);
                         break;
                     case 2:
                         UpdateUpgradeButton(2, 0);
                         break;
                     case 3:
                         UpdateUpgradeButton(3, 0);
                         break;

                 }
                 break;
             case 1:
                 upgradeMenu.GetComponent<Image>().sprite = psiquicTowerSprite;
                 upgradeButton.SetActive(true);
                 sellButton.SetActive(true);
                 switch (level)
                 {
                     case 0:
                         UpdateUpgradeButton(0, 1);
                         break;
                     case 1:
                         UpdateUpgradeButton(1, 1);
                         break;
                     case 2:
                         UpdateUpgradeButton(2, 1);
                         break;
                     case 3:
                         UpdateUpgradeButton(3, 1);
                         break;

                 }
                 break;
             case 2:
                 upgradeMenu.GetComponent<Image>().sprite = heavyTowerSprite;
                 upgradeButton.SetActive(true);
                 sellButton.SetActive(true);
                 switch (level)
                 {
                     case 0:
                         UpdateUpgradeButton(0, 2);
                         break;
                     case 1:
                         UpdateUpgradeButton(1, 2);
                         break;
                     case 2:
                         UpdateUpgradeButton(2, 2);
                         break;
                     case 3:
                         UpdateUpgradeButton(3, 2);
                         break;

                 }
                 break;
             case 3:
                 upgradeMenu.GetComponent<Image>().sprite = bombTowerSprite;
                 upgradeButton.SetActive(false);
                 sellButton.SetActive(true);

                 fixedTexts[2].SetActive(false);
                 statsTexts[2].SetActive(false);
                 break;
             case 4:
                 upgradeMenu.GetComponent<Image>().sprite = moneyStructureSprite;
                 upgradeButton.SetActive(false);
                 sellButton.SetActive(false);
                 break;
             case 5:
                 upgradeMenu.GetComponent<Image>().sprite = aerialStructureSprite;
                 upgradeButton.SetActive(true);
                 sellButton.SetActive(true);
                 switch (level)
                 {
                     case 0:
                         UpdateUpgradeButton(0, 5);
                         break;
                     case 1:
                         UpdateUpgradeButton(1, 5);
                         break;
                     case 2:
                         UpdateUpgradeButton(2, 5);
                         break;
                     case 3:
                         UpdateUpgradeButton(3, 5);
                         break;

                 }
                 break;


         }
     }*/

    public void UpdateUpgradeButton(int level, int structureId)
    {
        if (!GameManager.instance)
            return;

        upgradeButtonImage.sprite = GameManager.instance.upgradeLevels[level];
        if (structures[structureId].upgrades.Length > 0 && level < structures[structureId].upgrades.Length)
        {
            upgradeButton.GetComponent<Button>().interactable = true;
            upgradeCostText.GetComponent<TextMeshProUGUI>().text = structures[structureId].upgrades[level].cost.ToString();
        }
        else
        {
            upgradeCostText.GetComponent<TextMeshProUGUI>().text = "MAX";
            upgradeButton.GetComponent<Button>().interactable = false;
        }
    }

    void desactivateUpgradeTexts()
    {
        foreach (GameObject g in fixedTexts)
        {
            g.SetActive(false);
        }


        foreach (GameObject g in statsTexts)
        {
            g.SetActive(false);
        }
    }

    void activateUpgradeTexts()
    {
        foreach (GameObject g in fixedTexts)
        {
            g.SetActive(true);
        }


        foreach (GameObject g in statsTexts)
        {
            g.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        LevelManager.OnWaveCleared -= UpdateWaveText;
        LevelManager.OnGameStart -= SetWaveText;
        LevelManager.OnStructureUpgraded -= SetUpgradeButton;
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

