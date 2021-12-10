using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelInfoMenu : MonoBehaviour
{
    public Image[] enemiesSpritesContainers;
    public Image[] DefensesSpritesContainers;
    public Sprite[] enemiesSprites;
    public Sprite[] DefensesSprites;

    private void Awake()
    {
        DisableAllContainers();
    }

    public void DisableAllContainers()
    {
        foreach (Image i in enemiesSpritesContainers)
        {
            i.enabled = false;
        }
        foreach (Image j in DefensesSpritesContainers)
        {
            j.enabled = false;
        }
    }
}
