using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject levelSelectPanel;
    public void SelectLevel(string levelId)
    {
        SceneManager.LoadScene(levelId);
    }

    public void switchBetweenPanels(int panelId) {

        switch (panelId)
        {
            case 0:
                mainMenuPanel.SetActive(false);
                levelSelectPanel.SetActive(true);
                break;
            case 1:
                levelSelectPanel.SetActive(false);
                mainMenuPanel.SetActive(true);
                break;
            default:
                break;
        }
    }
}
