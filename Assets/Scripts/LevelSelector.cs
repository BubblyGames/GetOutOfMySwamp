using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;
using System.Collections;

public class LevelSelector : MonoBehaviour
{
    public static LevelSelector instance;

    int selectedWorld = 0;
    internal bool changing = false;

    public GameObject mainMenuPanel;
    public GameObject levelSelectPanel;
    public CubeWorldGenerator[] worlds;

    [Header("Enemies waves setup")]
    public Wave[][] enemies;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CreateWorldList();
    }
    public void SelectLevel(string levelId)
    {
        SceneManager.LoadScene(levelId);
    }

    public void NextWorld()
    {
        if (!changing && selectedWorld < worlds.Length - 1)
        {
            changing = true;
            selectedWorld++;
            MainMenuCamera.instance.GoTo(selectedWorld);
        }
    }
    public void PreviousWorld()
    {
        if (!changing && selectedWorld > 0)
        {
            changing = true;
            selectedWorld--;
            MainMenuCamera.instance.GoTo(selectedWorld);
        }
    }

    public void CreateWorldList()
    {
        for (int i = 0; i < worlds.Length; i++)
        {
            //insert the level world settings in the list containing the different levels
            WorldInfo worldInfo = new WorldInfo();
            worldInfo.nPaths = worlds[i].nPaths;
            worldInfo.wallDensity = worlds[i].wallDensity;
            worldInfo.rocksVisualReduction = worlds[i].rocksVisualReduction;
            worldInfo.rockSize = worlds[i].rockSize;
            worldInfo.numberOfMidpoints = worlds[i].numberOfMidpoints;
            worldInfo.material = worlds[i].GetComponent<MeshRenderer>().material;
            GameManager.instance.worldList.Add(worldInfo);

            //set the waves of enemies for that level
            GameManager.instance.wavesLevelsList[i]= enemies[i];
        }
    }

    public void SelectWorld()
    {
        GameManager.instance.worldInfo = GameManager.instance.worldList[selectedWorld];
        GameManager.instance.actualLevel = selectedWorld;
        SceneManager.LoadScene("level01");
    }

    public void DoneChanging()
    {
        changing = false;
    }

    public void switchBetweenPanels(int panelId)
    {

        switch (panelId)
        {
            case 0:
                levelSelectPanel.SetActive(false);
                mainMenuPanel.SetActive(true);
                //MainMenuCamera.instance.MoveRight();
                break;
            case 1:
                mainMenuPanel.SetActive(false);
                levelSelectPanel.SetActive(true);
                //MainMenuCamera.instance.MoveLeft();
                break;
            default:
                break;
        }
    }
}
