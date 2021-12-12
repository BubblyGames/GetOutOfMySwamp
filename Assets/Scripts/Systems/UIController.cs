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
    public GameObject sellValueText;
    public TextMeshProUGUI numberOfWaves;
    public TextMeshProUGUI currentWave;
    public GameObject lesson;

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

    [Header("Cards")]
    public GameObject basicTowerCard;
    public GameObject psiquicTowerCard;
    public GameObject heavyTowerCard;
    public GameObject bombTowerCard;
    public GameObject moneyStructureCard;
    public GameObject aerialStructureCard;

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
            structures = shopContainer.GetComponent<Shop>().defenseBlueprints;
        }

    }

    public ScrollCard[] scrollCards;
    private void Start()
    {
        LevelManager.OnWaveCleared += UpdateWaveText;
        LevelManager.OnGameStart += SetWaveText;
        LevelManager.OnStructureUpgraded += SetUpgradeButton;
        if (gameSceneId == 0 || !GameManager.instance)
            return;

        RectTransform rectTransform = basicTowerCard.GetComponentInParent<RectTransform>();
        VerticalLayoutGroup verticalLayout = basicTowerCard.GetComponentInParent<VerticalLayoutGroup>();

        WorldInfo w = GameManager.instance.GetCurrentWorld();
        GameObject[] cards = new GameObject[] {
        basicTowerCard,
        psiquicTowerCard,
        heavyTowerCard,
        bombTowerCard,
        moneyStructureCard,
        aerialStructureCard};

        bool[] cardsAllowed = new bool[] {
        w.basicDef,
        w.PoisonDef,
        w.HeavyDef,
        w.Bomb,
        w.MoneyDef,
        w.AerealDef};

        List<ScrollCard> sCards = new List<ScrollCard>();
        for (int i = 0; i < 6; i++)
        {
            if (!cardsAllowed[i])
            {
                Destroy(cards[i]);
            }
            else
                sCards.Add(cards[i].GetComponent<ScrollCard>());
        }

        scrollCards = sCards.ToArray();

        //rectTransform.sizeDelta = new Vector2(218, (float)scrollCards.Length * (262 + verticalLayout.spacing));

    }

    public void UpdateCardCosts()
    {
        foreach (ScrollCard s in scrollCards)
        {
            if (!s)
                continue;
            TextMeshProUGUI t = s.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            t.text = Shop.instance.defenseBlueprints[s.indexCard].CreationCost.ToString();
        }
    }

    private void SetUpgradeButton(int level)
    {
        UpdateUpgradeButton(level, BuildManager.instance.SelectedStructure.structureId);
    }

    private void SetWaveText()
    {
        numberOfWaves.text = WaveController.instance.waves.Length.ToString();
        currentWave.text = (WaveController.instance.currentWave + 1).ToString();
    }

    public void UpdateWaveText()
    {
        currentWave.GetComponent<TextMeshProUGUI>().text = (WaveController.instance.currentWave + 1).ToString();
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
                LevelManager.instance.gameFinished = true;
                endgameMenuLoose.SetActive(true);
                upgradeMenu.SetActive(false);
                pauseMenu.SetActive(false);
                pauseButton.SetActive(false);
                Time.timeScale = 0;

                break;
            case GameMenu.EndgameMenuWin:
                LevelManager.instance.gameFinished = true;
                endgameMenuWin.SetActive(true);
                upgradeMenu.SetActive(false);
                pauseMenu.SetActive(false);
                pauseButton.SetActive(false);
                Time.timeScale =0;
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
        LevelManager.instance.ready = true;
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
        LevelManager.instance.ready = true;
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

        if (upgradeMenu.activeSelf && BuildManager.instance.SelectedStructure.structureId == 3)
        {
            statsTexts[0].GetComponent<TextMeshProUGUI>().text = BuildManager.instance.SelectedStructure.gameObject.GetComponent<Gatherer>().TotalResourceGathered.ToString();
        }
    }


    public void SetUpgradeMenu(Structure structure)
    {
        towerName.GetComponent<TextReader>().SetKey(structure.structureName);
        if (structure.structureId != 3)
        {
            switch (structure.structureId)
            {
                case 0:
                case 1:
                case 2:
                case 4:
                    SetActiveUpgradeTexts(true, -1);
                    statsTexts[3].GetComponent<TextReader>().SetKey(structure.Blueprint.fireRateDescription);
                    break;
                case 5:
                    SetActiveUpgradeTexts(false, -1);
                    SetActiveUpgradeTexts(true, 2);
                    break;
            }

            fixedTexts[0].GetComponent<TextReader>().SetKey("target");
            statsTexts[0].GetComponent<TextReader>().SetKey(structure.Blueprint.targetDescription);
            statsTexts[1].GetComponent<TextReader>().SetKey(structure.Blueprint.rangeDescription);
            statsTexts[2].GetComponent<TextReader>().SetKey(structure.Blueprint.damageDescription);
        }
        else
        {
            SetActiveUpgradeTexts(false, -1);
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
                upgradeMenu.GetComponent<Image>().sprite = moneyStructureSprite;
                break;
            case 4:
                upgradeMenu.GetComponent<Image>().sprite = aerialStructureSprite;
                break;
            case 5:
                upgradeMenu.GetComponent<Image>().sprite = bombTowerSprite;
                break;
        }
        upgradeButton.SetActive(true);
        sellButton.SetActive(true);
       

        if (structure.structureId == 3 || structure.structureId == 5)
        {
            UpdateUpgradeButton(3, structure.structureId);
        }
        else
        {
            UpdateUpgradeButton(structure.GetLevel(), structure.structureId);
        }
    }


    public void UpdateUpgradeButton(int level, int structureId)
    {
        if (!GameManager.instance)
            return;

        upgradeButtonImage.sprite = GameManager.instance.upgradeLevels[level];
        if (structures[structureId].upgrades.Length >= 0 && level < structures[structureId].upgrades.Length)
        {
            upgradeButton.GetComponent<Button>().interactable = true;
            upgradeCostText.GetComponent<TextMeshProUGUI>().text = structures[structureId].upgrades[level].cost.ToString();
        }
        else
        {
            upgradeCostText.GetComponent<TextMeshProUGUI>().text = "MAX";
            upgradeButton.GetComponent<Button>().interactable = false;
        }
        sellValueText.GetComponent<TextMeshProUGUI>().text = structures[structureId].sellValues[level].ToString();
       
    }


    /// <summary>
    /// Set fixed text active or inactive, from start to a certain index
    /// </summary>
    /// <param name="active">This bool sets if fixed text will becom active or inactive</param>
    /// <param name="index">index = -1, al elements, any other number, number + 1 element will become active</param>
    void SetActiveUpgradeTexts(bool active, int index)
    {
        int length = fixedTexts.Count;
        if (index != -1)
        {
            length = index + 1;
        }
        for (int i = 0; i < length; i++)
        {
            fixedTexts[i].SetActive(active);
            statsTexts[i].SetActive(active);
        }
    }

    public void SetUpgradeMenuActive(bool active)
    {
        if (upgradeMenu)
            upgradeMenu.SetActive(active);
    }

    private void OnDestroy()
    {
        LevelManager.OnWaveCleared -= UpdateWaveText;
        LevelManager.OnGameStart -= SetWaveText;
        LevelManager.OnStructureUpgraded -= SetUpgradeButton;
    }

    public void ShowLesson()
    {
        lesson.SetActive(true);
    }

    public void ShowTowerData(int index)
    {
        lesson.SetActive(true);
        lesson.GetComponent<TutorialMenu>().StartWithSpecificKey(index);
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

