using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    public void resume()
    {
        gameManager.instance.stateUnpaused();
    }

    public void respawnPlayer()
    {
        gameManager.instance.stateUnpaused();
        gameManager.instance.playerController.spawnPlayer();
    }

    public void Restart()
    {
        gameManager.instance.stateUnpaused();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void loadFirstScene()
    {
        gameManager.instance.stateUnpaused();
        scoreKeeper.playerScore = 0;
        SceneManager.LoadScene(0);
    }

    public void quit()
    {
        Application.Quit();
    }

    public void LoadHub()
    {
        gameManager.instance.stateUnpaused();
        scoreKeeper.playerScore = 0;
        SceneManager.LoadScene("Hub");
    }

    public void loadLevel2()
    {
        gameManager.instance.stateUnpaused();
        SceneManager.LoadScene("Level 2");
    }

    public void loadLevel3()
    {
        gameManager.instance.stateUnpaused();
        SceneManager.LoadScene("Level 3");
    }

    public void loadMainMenu()
    {
        gameManager.instance.stateUnpaused();
        SceneManager.LoadScene("Main Menu");
    }
}
