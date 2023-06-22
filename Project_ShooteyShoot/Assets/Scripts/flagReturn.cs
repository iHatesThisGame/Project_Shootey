using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flagReturn : MonoBehaviour
{
    public bool captured;
    private void OnTriggerEnter(Collider other)
    {
        if (gameManager.instance.playerController.hasFlag == true)
        {
            gameManager.instance.isCaptured = true;
        }
    }
}