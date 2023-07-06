using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectMenu : MonoBehaviour
{
    public void Hub()
    {
        SceneManager.LoadScene("Hub");
    }

    public void Survival()
    {
        SceneManager.LoadScene("Main Level");
    }

    public void ObstacleCourse()
    {
        SceneManager.LoadScene("Obstacle Course");
    }

    public void FlagCapture()
    {
        SceneManager.LoadScene("Flag Capture");
    }

    public void LevelOne()
    {
        SceneManager.LoadScene("Level 1");
    }

    public void LevelTwo()
    {
        SceneManager.LoadScene("Level 2");
    }

    public void LevelThree()
    {
        SceneManager.LoadScene("Level 3");
    }
}
