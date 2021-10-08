using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager levelInstance;

    [Header("Control")]
    [SerializeField] private int startBaseHealthPoints; //Starting Health points the defenders base have
    [SerializeField] private int currentBaseHealthPoints; //Current Health points the defenders base have
    [SerializeField] private int currentMoney; //Amount of money the player can spend
    public bool spawnEnemys = true;


    public event Action OnGameStarted, OnGameLost, OnScoreIncremented;
    //TODO: increment score when kkilling enemys.

    private WorldGenerator world;
    public GameObject worldGeneratorPrefab;
    private ScoreSystem scoreSystem;
    private RoundController roundController;

    private void Awake()
    {
        levelInstance = this;
        roundController = new RoundController();
        currentBaseHealthPoints = startBaseHealthPoints;
    }
    private void Start()
    {
        OnGameStarted?.Invoke();
    }

    public void addMoney(int quantity) 
    {
        currentMoney += quantity;
    }

    public void dealDamageToBase(int damageDealt)
    {

        currentBaseHealthPoints -= damageDealt;
        if (currentBaseHealthPoints <= 0)
        {
            //Game Over
            OnGameLost?.Invoke();
            
            // Show Game Over Screen
            //Go to menu

            
        }
    }
}
