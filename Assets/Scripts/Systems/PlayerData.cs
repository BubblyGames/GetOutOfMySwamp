using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public List<int> worldScores= new List<int>();
    public bool alreadyCreated = true;
    ///sumary
    ///Array that contains booleans that indicate if a tower has been used for the first time.
    ///If false, tutorial will spawn in first used
    ///0 basic, 1 heavy, 2 poison, 3 money, 4 areal, 5 bombs
    ///</summary>
    public bool[] defenses;

    public PlayerData(GameManager gameManager)
    {

        for (int i = 0; i < gameManager.levelSelector.worlds.Length; i++)
        {
            worldScores.Add(0);
        }
        defenses = new bool[6];

    }

    /// <summary>
    /// Sets the defense of index x as used for the first time.
    /// </summary>
    /// <param name="index"></param>
    public void DefenseUsed(int index)
    {
        defenses[index] = true;
    }

    public void SaveScore(int index, int score)
    {
        worldScores[index] = score;
    }
}
