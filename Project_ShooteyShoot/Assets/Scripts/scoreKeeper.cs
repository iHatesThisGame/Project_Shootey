using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scoreKeeper : MonoBehaviour, IDataPersistance
{
    public static int playerScore = 0;

    public void LoadData(GameData data)
    {
        playerScore = data.score;
    }

    public void SaveData(GameData data)
    {
        data.score = playerScore;
    }
}
