using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyGatherer : Gatherer
{
    private void Start()
    {
        LevelStats.instance.numberOfSpeculios++;
        UIController.instance.UpdateCardCosts();
    }

    void Update()
    {
        if (!LevelManager.instance.gameFinished)
        {

            timerToGetResource += Time.deltaTime;

            if (timerToGetResource >= timeToGatherResource)
            {
                LevelStats.instance.getResource(0, resourceGatheredEachCicle);
                TotalResourceGathered += resourceGatheredEachCicle;
                timerToGetResource = 0;
            }
        }
    }

    private void OnDestroy()
    {
        if (!Application.isPlaying)
            return;
        LevelStats.instance.numberOfSpeculios--;
        UIController.instance.UpdateCardCosts();
    }
}
