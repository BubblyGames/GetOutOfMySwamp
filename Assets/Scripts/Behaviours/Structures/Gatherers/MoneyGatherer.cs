using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyGatherer : Gatherer
{

    void Update()
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
