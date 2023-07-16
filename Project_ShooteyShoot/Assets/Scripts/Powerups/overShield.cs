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

    Vector3 despawn = new Vector3(-50, -50, -50);

    //private void OnTriggerEnter(Collider other)
    //{
    //    IShield shielded = other.GetComponent<IShield>();
    //    if (shielded != null)
    //    {
    //        if (gameManager.instance.playerController.shieldHP < gameManager.instance.playerController.shieldMax)
    //        {
    //            aud.PlayOneShot(audPickup, audPickupVol);

    //            currOverShield.transform.position -= despawn;

    //            gameManager.instance.playerController.shieldHP = shieldHP;
    //            gameManager.instance.playerController.updatePlayerUI();
    //            if (gameManager.instance.playerController.shieldHP > gameManager.instance.playerController.shieldMax)
    //            {
    //                gameManager.instance.playerController.shieldHP = gameManager.instance.playerController.shieldMax;
    //            }
    //            Destroy(gameObject);
    //        }
    //    }
    //}

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(PowerupSequence());
        }
    }

    public IEnumerator PowerupSequence()
    {
        aud.PlayOneShot(audPickup, audPickupVol);

        currOverShield.transform.position -= despawn;
        gameManager.instance.playerController.shieldHP += shieldHP;
        if (gameManager.instance.playerController.shieldHP > gameManager.instance.playerController.shieldMax)
        {
            gameManager.instance.playerController.shieldHP = gameManager.instance.playerController.shieldMax;
        }
        yield return new WaitForSeconds(1);
        Destroy(currOverShield);
    }
}