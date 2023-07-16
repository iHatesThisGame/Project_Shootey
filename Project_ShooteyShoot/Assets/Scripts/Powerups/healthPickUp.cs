using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthPickUp : MonoBehaviour
{
    [SerializeField] GameObject healthPack;

    [Header("----- Audio -----")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip audPickup;
    [Range(0, 1)][SerializeField] float audPickupVol;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameManager.instance.playerController.HP != gameManager.instance.playerController.playerHPOrig)
            {
                aud.PlayOneShot(audPickup, audPickupVol);

                Destroy(healthPack);
                gameManager.instance.playerController.HP = gameManager.instance.playerController.playerHPOrig;
                gameManager.instance.playerController.updatePlayerUI(); 
            }
        }
    }
}
