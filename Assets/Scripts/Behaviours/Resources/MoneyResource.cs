using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyResource : MonoBehaviour
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

    // Update is called once per frame
    void Update()
    {
        timeFromLastResource += Time.deltaTime;
   
        if (timeFromLastResource >= timeTakingToSpawnResource)
        {
            //Debug.Log(currentLevelMoney.currentMoney);
            currentLevelMoney.currentMoney += moneyGiven;
            timeFromLastResource = 0;
            currentLevelMoney.moneyText.text= currentLevelMoney.currentMoney.ToString();
        }
    }

}
