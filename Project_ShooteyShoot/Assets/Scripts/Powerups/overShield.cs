using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class overShield : MonoBehaviour
{
    [SerializeField] int shieldHP;
    [SerializeField] GameObject currOverShield;

    [Header("----- Audio -----")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip audPickup;
    [Range(0, 1)][SerializeField] float audPickupVol;

    private void OnTriggerEnter(Collider other)
    {
        IShield shielded = other.GetComponent<IShield>();
        if (shielded != null)
        {
            if (gameManager.instance.playerController.shieldHP < gameManager.instance.playerController.shieldMax)
            {
                aud.PlayOneShot(audPickup, audPickupVol);

                Destroy(currOverShield);
                gameManager.instance.playerController.shieldHP = shieldHP;
                gameManager.instance.playerController.updatePlayerUI();
                if (gameManager.instance.playerController.shieldHP > gameManager.instance.playerController.shieldMax)
                {
                    gameManager.instance.playerController.shieldHP = gameManager.instance.playerController.shieldMax;
                } 
            }
        }
    }
}