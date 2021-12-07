using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public List<int> worldScores= new List<int>();
    public bool alreadyCreated = true;

    public PlayerData(GameManager gameManager)
    {
        for (int i = 0; i < gameManager.levelSelector.worlds.Length; i++)
        {
            worldScores.Add(-1);
        }
    }

}
