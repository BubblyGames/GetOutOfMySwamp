using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public static LevelSelector instance;

    int selectedWorld = 0;
    internal bool changing = false;

    public GameObject mainMenuPanel;
    public GameObject levelSelectPanel;
    public CubeWorldGenerator[] worlds;

    private void Awake()
    {
        instance = this;
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

    public void SelectWorld()
    {
        WorldInfo worldInfo = new WorldInfo();
        worldInfo.nPaths = worlds[selectedWorld].nPaths;
        worldInfo.wallDensity = worlds[selectedWorld].wallDensity;
        worldInfo.rocksVisualReduction = worlds[selectedWorld].rocksVisualReduction;
        worldInfo.rockSize = worlds[selectedWorld].rockSize;
        worldInfo.numberOfMidpoints = worlds[selectedWorld].numberOfMidpoints;
        worldInfo.material = worlds[selectedWorld].GetComponent<MeshRenderer>().material;

        GameManager.instance.worldInfo = worldInfo;

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
