using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public int playerHP;
    public int shieldHP;
    public int score;
    //public string sceneName;

    public GameData()
    {
        this.playerHP = 10;
        this.shieldHP = 0;
        this.score = 0;
        //this.sceneName = "Main Menu";
    }
}
