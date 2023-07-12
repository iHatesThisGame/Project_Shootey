using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class overShield : MonoBehaviour
{
    [SerializeField] int shieldHP;
    [SerializeField] GameObject currOverShield;

    private void OnTriggerEnter(Collider other)
    {
        IShield shielded = other.GetComponent<IShield>();
        if (shielded != null)
        {
            if (gameManager.instance.playerController.shieldHP < gameManager.instance.playerController.shieldMax)
            {
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