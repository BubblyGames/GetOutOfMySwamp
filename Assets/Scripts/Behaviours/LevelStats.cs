using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelStats : MonoBehaviour
{
    public static LevelStats instance;

    [Header("Text References")]
    public Text hpText;
    public Image hpBar;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI scoreText;

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


        LevelManager.OnDamageTaken += receiveDamage;
        LevelManager.instance.OnEnemyKilled += getEnemyRewards;
    }


    private void Start()
    {
        currentBaseHealthPoints = startBaseHealthPoints;
        currentMoney = startMoney;
        hpText.text = currentBaseHealthPoints.ToString();
        moneyText.text = currentMoney.ToString();
        scoreText.text = currentScore.ToString();
    }

    public void getEnemyRewards(int moneyReward, int scoreReward)
    {
        currentMoney += moneyReward;
        moneyText.text = currentMoney.ToString();
        currentScore += scoreReward;
        scoreText.text =currentScore.ToString();
    }

    public void getResource(int resourceId, int amount)
    {
        switch (resourceId)
        {
            case 0:
                currentMoney += amount;
                moneyText.text = currentMoney.ToString();
                break;
        }
    }

    public void receiveDamage(int damageTaken)
    {
        currentBaseHealthPoints -= damageTaken;
        currentBaseHealthPoints = Mathf.Max(0, currentBaseHealthPoints);
        hpText.text = currentBaseHealthPoints.ToString();
        hpBar.fillAmount = (float) currentBaseHealthPoints / (float) startBaseHealthPoints;
    }

    public void SpendMoney(int quantity)
    {
        currentMoney -= quantity;
        moneyText.text =currentMoney.ToString();
    }

    public void EarnMoney(int quantity)
    {
        currentMoney += quantity;
        moneyText.text =currentMoney.ToString();
    }

    public void UpdateScoreText()
    {
        scoreText.text =currentScore.ToString();
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
