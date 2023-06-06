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

    public void quit()
    {
        Application.Quit();
    }
}
