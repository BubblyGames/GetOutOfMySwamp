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


    [Header("God Mode control")]
    [SerializeField] public bool infinteMoney;
    [SerializeField] public bool infinteHP;

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
            Destroy(this);
        }


        LevelManager.instance.OnDamageTaken += receiveDamage;
        LevelManager.instance.OnEnemyKilled += getEnemyRewards;
    }


    private void Start()
    {
        currentBaseHealthPoints = startBaseHealthPoints;
        currentMoney = startMoney;
        hpText.text = "Health: " + currentBaseHealthPoints.ToString();
        moneyText.text = "Money:" + currentMoney.ToString();
        scoreText.text = "Score:" + currentScore.ToString();

    }

    public void getEnemyRewards(int moneyReward, int scoreReward)
    {
        currentMoney += moneyReward;
        moneyText.text = "Money:" + currentMoney.ToString();
        currentScore += scoreReward;
        scoreText.text = "Score:" + currentScore.ToString();
    }

    public void getResource(int resourceId, int amount)
    {
        switch (resourceId)
        {
            case 0:
                currentMoney += amount;
                moneyText.text = "Money:" + currentMoney.ToString();
                break;
        }
    }

    public void receiveDamage(int damageTaken)
    {
        currentBaseHealthPoints -= damageTaken;
        hpText.text = "Health: " + currentBaseHealthPoints.ToString();
    }

    public void SpendMoney(int quantity)
    {
        currentMoney -= quantity;
        moneyText.text = "Money:" + currentMoney.ToString();
    }

    public void EarnMoney(int quantity)
    {
        currentMoney += quantity;
        moneyText.text = "Money:" + currentMoney.ToString();
    }

    public void UpdateScoreText()
    {
        scoreText.text = "Score:" + currentScore.ToString();
    }
}
