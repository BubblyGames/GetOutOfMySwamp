using System.Collections;
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
    public GameObject shopMenu;
    public GameObject endgameMenu;


    [Header("LevelToRestart")]
    public int levelToRestart;
    public enum Menus
    {
        UpgradeMenu,
        ShopMenu,
        EndgameMenu
    }

    public Menus menus;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        levelToRestart = SceneManager.GetActiveScene().buildIndex;
    }

    public void EnableUpdateMenu()
    {
        upgradeMenu.SetActive(true);
    }

    public void DisableUpdateMenu()
    {
        upgradeMenu.SetActive(false);
    }

    public void EnableEndgameMenu()
    {
        upgradeMenu.SetActive(false);
        shopMenu.SetActive(false);
        endgameMenu.SetActive(true);
        GameObject.Find("FinalScoreText").GetComponent<UnityEngine.UI.Text>().text ="Score: "+LevelStats.instance.currentScore;
    }

    public void Restart()
    {
        endgameMenu.SetActive(false);
        SceneManager.LoadScene(levelToRestart);
    }

    public void SetMenuActive()
    {
        switch (menus)
        {
            case Menus.UpgradeMenu:
                upgradeMenu.SetActive(true);

                break;
            case Menus.ShopMenu:
                shopMenu.SetActive(true);
                break;
            case Menus.EndgameMenu:
                EnableEndgameMenu();
                break;
            default:
                break;
        }
    }

    public void SetMenuInactive()
    {
        switch (menus)
        {
            case Menus.UpgradeMenu:
                upgradeMenu.SetActive(false);
                break;
            case Menus.ShopMenu:
                shopMenu.SetActive(false);
                break;
            case Menus.EndgameMenu:
                endgameMenu.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void ChangeToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void GoToNextLevel()
    {
        GameManager.instance.SetNextLevelWorld();
        SceneManager.LoadScene(levelToRestart);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(UIController))]
public class UIControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        UIController uIcontroller = (UIController) target;
        DrawDefaultInspector();
        if (GUILayout.Button("Enable"))
        {
            uIcontroller.SetMenuActive();
        }

        if (GUILayout.Button("Disable"))
        {
            uIcontroller.SetMenuInactive();
        }
    }
}
#endif

