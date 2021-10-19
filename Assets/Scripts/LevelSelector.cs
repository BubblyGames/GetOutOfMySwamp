using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{

    public void SelectLevel(string levelId)
    {
        SceneManager.LoadScene(levelId);
    }
}
