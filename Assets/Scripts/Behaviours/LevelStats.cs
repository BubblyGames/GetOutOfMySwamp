using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class LevelStats : MonoBehaviour
{
    public static LevelStats instance;

    [Header("Text References")]
    public Text hpText;
    public Image hpBar;
    public Image hpBarBG;
    public TextMeshProUGUI moneyText;

    [Header("Level Stats")]
    [SerializeField] private int startBaseHealthPoints = 100; //Starting Health points the defenders base have
    [SerializeField] private int currentBaseHealthPoints; //Current Health points the defenders base have

    [SerializeField] private int startMoney = 400; // Starting money the player have
    [SerializeField] public int currentMoney; //Amount of money the player can spend
    
    [SerializeField] public int currentScore; //Score the player has

    [HideInInspector] public int numberOfSpeculios = 0;

    public int moneyPTick;
    public int totalMaintenance;

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

    }


    private void Start()
    {
        currentBaseHealthPoints = startBaseHealthPoints;
        currentMoney = startMoney;
        hpText.text = currentBaseHealthPoints.ToString();
        moneyText.text = currentMoney.ToString();
    }

    private void Update()
    {
        if (moneyPTick < totalMaintenance)
        {
            moneyText.color = new Color(1, 0, 0);
        }
        else
        {
            moneyText.color = new Color(0.345098f, 0.1058824f, 0.1490196f);
        }
    }

    public void getEnemyRewards(int moneyReward, int scoreReward)
    {
        
        currentMoney += moneyReward;
        moneyText.text = currentMoney.ToString();
        

        currentScore += scoreReward;
    }

    public void getResource(int resourceId, int amount)
    {
        switch (resourceId)
        {
            case 0:
                currentMoney += amount;
                moneyPTick = amount * numberOfSpeculios;

                moneyText.text = currentMoney.ToString();                
                break;
        }
    }

    public void receiveDamage(int damageTaken)
    {
        Sequence damage = DOTween.Sequence();
        damage.Append(hpBarBG.DOColor(InputManager.instance.wrongColor, 0.5f));
        damage.AppendCallback(() =>
        {
            currentBaseHealthPoints -= damageTaken;
            currentBaseHealthPoints = Mathf.Max(0, currentBaseHealthPoints);
            hpText.text = currentBaseHealthPoints.ToString();
            hpBar.fillAmount = (float)currentBaseHealthPoints / (float)startBaseHealthPoints;
        });
        damage.Append(hpBarBG.DOColor(new Color(1, 1, 1), 0.7f));

    }

    public void SpendMoney(int quantity)
    {
        currentMoney -= quantity;
        moneyText.text = currentMoney.ToString();
        /*Color colorMoneyText = moneyText.color;
        Sequence moneyS = DOTween.Sequence();
        moneyS.Append(moneyText.DOColor(new Color(1, 0, 0), 0.5f));
        moneyS.AppendCallback(() =>
        {
            currentMoney -= quantity;
            moneyText.text = currentMoney.ToString();
        });
        moneyS.Append(moneyText.DOColor(colorMoneyText, 0.5f));*/
        
    }

    public void EarnMoney(int quantity)
    {
        currentMoney += quantity;
        moneyText.text = currentMoney.ToString();
        /*Color colorMoneyText = moneyText.color;
        Sequence moneyS = DOTween.Sequence();
        moneyS.Append(moneyText.DOColor(new Color(0, 0.7f, 0), 0.5f));
        moneyS.AppendCallback(() =>
        {
            currentMoney += quantity;
            moneyText.text = currentMoney.ToString();
        });
        moneyS.Append(moneyText.DOColor(colorMoneyText, 0.5f));*/
    }

    public void UpdateScoreText()
    {
        //scoreText.text =currentScore.ToString();
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

    private void OnDestroy()
    {
        LevelManager.OnDamageTaken -= receiveDamage;

    }

    public int GetStartBaseHealth()
    {
        return startBaseHealthPoints;
    }
}
