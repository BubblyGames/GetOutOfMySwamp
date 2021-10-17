using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStats : MonoBehaviour
{
    public static LevelStats levelStatsInstance;

    [Header("Level Stats")]
    [SerializeField] private int startBaseHealthPoints = 100; //Starting Health points the defenders base have
    [SerializeField] public int currentBaseHealthPoints; //Current Health points the defenders base have

    [SerializeField] private int startMoney = 400; // Starting money the player have
    [SerializeField] public int currentMoney; //Amount of money the player can spend
    
    [SerializeField] private int currentScore; //Score the player has


    [Header("God Mode control")]
    [SerializeField] public bool infinteMoney;
    [SerializeField] public bool infinteHP;

    private void Awake()
    {
        levelStatsInstance = this;
    }

    private void Start()
    {
        currentBaseHealthPoints = startBaseHealthPoints;
        currentMoney = startMoney;

    }

    public void ReceiveDamage(int damageTaken)
    {
        currentBaseHealthPoints -= damageTaken;
    }

    public void EarnMoney(int quantity)
    {
        currentMoney += quantity;
    }

    public void SpendMoney(int quantity)
    {
        currentMoney -= quantity;
    }

    private void OnValidate()
    {
        
    }
}
