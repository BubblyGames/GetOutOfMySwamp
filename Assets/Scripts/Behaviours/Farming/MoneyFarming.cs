using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyFarming : MonoBehaviour
{

    [Header("MoneyResrouce Settings")]
    [SerializeField] private int moneyGiven;
    [SerializeField] private float timeTakingToSpawnResource;
    LevelStats currentLevelMoney;
    float timeFromLastResource;

    private void Awake()
    {
        currentLevelMoney = GameObject.Find("LevelManager").GetComponent<LevelStats>();
    }

    void Update()
    {
        timeFromLastResource += Time.deltaTime;
   
        if (timeFromLastResource >= timeTakingToSpawnResource)
        {
            currentLevelMoney.currentMoney += moneyGiven;
            timeFromLastResource = 0;
            currentLevelMoney.moneyText.text= "Money: "+currentLevelMoney.currentMoney.ToString();
        }
    }

}
