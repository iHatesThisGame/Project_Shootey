using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthPickUp : MonoBehaviour
{
    [SerializeField] GameObject healthPack;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameManager.instance.playerController.HP != gameManager.instance.playerController.playerHPOrig)
            {
                Destroy(healthPack);
                gameManager.instance.playerController.HP = gameManager.instance.playerController.playerHPOrig;
                gameManager.instance.playerController.updatePlayerUI(); 
            }
        }
    }
}
