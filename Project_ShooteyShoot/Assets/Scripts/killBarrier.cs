using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killBarrier : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.loseMessageText.text = "You tried to fly";
            gameManager.instance.youLose();
        }
    }
}
