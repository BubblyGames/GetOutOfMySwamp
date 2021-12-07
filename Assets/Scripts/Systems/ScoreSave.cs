using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSave : MonoBehaviour
{
    GameManager gameManager;
    public LevelStats levelStats;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        int score = 1;

        if (levelStats.GetCurrentBaseHealth() == levelStats.GetStartBaseHealth())
        {
            score = 3;
        }
        else if (levelStats.currentScore >= (levelStats.GetStartBaseHealth() / 2))
        {
            score = 2;
        }
        PersistenceManager.SaveScoreData(gameManager.playerData, gameManager.currentWorldId, score);
    }
}
