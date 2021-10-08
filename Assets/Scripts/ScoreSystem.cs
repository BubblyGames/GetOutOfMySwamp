using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem scoreSystemInstance;
    public int score { get; private set; } = 0;
    Text scoreText;


    private void Start()
    {
        scoreText = GetComponent<Text>();
        //LevelManager.levelInstance.OnScoreIncremented += IncrementScore; //TODO: pass how much the score will increment
        LevelManager.levelInstance.OnGameStarted += ResetScore;
    }

    void IncrementScore(int quantity)
    {
        score += quantity;
        DisplayScore();
    }

    private void ResetScore()
    {
        score = 0;
        DisplayScore();
    }

    private void DisplayScore()
    {
        scoreText.text = score.ToString();
    }
}