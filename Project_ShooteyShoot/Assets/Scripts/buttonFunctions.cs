using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour//, IDataPersistance
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

    public void Continue()
    {
        gameManager.instance.stateUnpaused();

        if (SceneManager.GetActiveScene().name == "Level 3" || 
            SceneManager.GetActiveScene().name == "Flag Capture" ||
            SceneManager.GetActiveScene().name == "Obstacle Course")
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            SceneManager.LoadScene("Main Menu");
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
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
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        SceneManager.LoadScene("Main Menu");

    }

    //public void LoadData(GameData data)
    //{
    //    gameManager.instance.stateUnpaused();
    //    SceneManager.LoadScene(data.sceneName);
    //}

    //public void SaveData(GameData data)
    //{
     //   data.sceneName = SceneManager.GetActiveScene().name;
    //}
}
