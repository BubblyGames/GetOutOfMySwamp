using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetFinalScore : MonoBehaviour
{
    public LevelStats levelStats;
    public Image container;
    public List<Sprite> starSprites;
    void Start()
    {
        if (levelStats.GetCurrentBaseHealth() == levelStats.GetStartBaseHealth())
        {
            container.sprite = starSprites[2];
        }
        else if (levelStats.GetCurrentBaseHealth() >= levelStats.GetStartBaseHealth() / 2)
        {
            container.sprite = starSprites[1];
        }
    }
}
