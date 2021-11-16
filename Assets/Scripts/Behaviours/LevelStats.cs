using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelStats : MonoBehaviour
{
    public static LevelStats instance;

    [Header("Text References")]
    public Text hpText;
    public Text moneyText;
    public Text scoreText;

    [Header("Level Stats")]
    [SerializeField] private int startBaseHealthPoints = 100; //Starting Health points the defenders base have
    [SerializeField] private int currentBaseHealthPoints; //Current Health points the defenders base have

    [SerializeField] private int startMoney = 400; // Starting money the player have
    [SerializeField] public int currentMoney; //Amount of money the player can spend
    
    [SerializeField] public int currentScore; //Score the player has

    public int CurrentMoney { get => currentMoney; set => currentMoney = value; }
    public int CurrentBaseHealthPoints { get => currentBaseHealthPoints; set => currentBaseHealthPoints = value; }

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


        LevelManager.instance.OnDamageTaken += receiveDamage;
        LevelManager.instance.OnEnemyKilled += getEnemyRewards;
    }


    private void Start()
    {
        currentBaseHealthPoints = startBaseHealthPoints;
        currentMoney = startMoney;
        hpText.text = hpText.gameObject.GetComponent<TextReader>().GetText()+":" + currentBaseHealthPoints.ToString();
        moneyText.text = moneyText.gameObject.GetComponent<TextReader>().GetText() + ":" + currentMoney.ToString();
        scoreText.text = scoreText.gameObject.GetComponent<TextReader>().GetText() + ":" + currentScore.ToString();
    }

    public void getEnemyRewards(int moneyReward, int scoreReward)
    {
        currentMoney += moneyReward;
        moneyText.text = moneyText.gameObject.GetComponent<TextReader>().GetText() + ":" + currentMoney.ToString();
        currentScore += scoreReward;
        scoreText.text = scoreText.gameObject.GetComponent<TextReader>().GetText() + ":" + currentScore.ToString();
    }

    public void getResource(int resourceId, int amount)
    {
        switch (resourceId)
        {
            case 0:
                currentMoney += amount;
                moneyText.text = moneyText.gameObject.GetComponent<TextReader>().GetText() + ":" + currentMoney.ToString();
                break;
        }
    }

    public void receiveDamage(int damageTaken)
    {
        currentBaseHealthPoints -= damageTaken;
        currentBaseHealthPoints = Mathf.Max(0, currentBaseHealthPoints);
        hpText.text = hpText.gameObject.GetComponent<TextReader>().GetText() + ":" + currentBaseHealthPoints.ToString();
    }

    public void SpendMoney(int quantity)
    {
        currentMoney -= quantity;
        moneyText.text = moneyText.gameObject.GetComponent<TextReader>().GetText() + ":" + currentMoney.ToString();
    }

    public void EarnMoney(int quantity)
    {
        currentMoney += quantity;
        moneyText.text = moneyText.gameObject.GetComponent<TextReader>().GetText() + ":" + currentMoney.ToString();
    }

    public void UpdateScoreText()
    {
        scoreText.text = scoreText.gameObject.GetComponent<TextReader>().GetText() + ":" + currentScore.ToString();
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }
    public int GetCurrentBaseHealth()
    {
        return currentBaseHealthPoints;
    }

    public int GetCurrentMoney()
    {
        return currentMoney;
    }
}
