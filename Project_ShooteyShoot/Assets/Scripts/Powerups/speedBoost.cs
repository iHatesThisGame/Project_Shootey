using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedBoost : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] GameObject speedPowerup;
    [SerializeField] int boostAmount;
    [SerializeField] int duration;

    [Header("----- Audio -----")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip audPickup;
    [Range(0, 1)][SerializeField] float audPickupVol;

    Vector3 despawn = new Vector3 (-50, -50, -50);

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

        speedPowerup.transform.position -= despawn;
        gameManager.instance.playerController.playerSpeedOrig *= boostAmount;
        gameManager.instance.playerController.sprintSpeed *= boostAmount;
        gameManager.instance.playerSpeedBoost.SetActive(true);
        yield return new WaitForSeconds(duration);
        gameManager.instance.playerController.playerSpeedOrig /= boostAmount;
        gameManager.instance.playerController.sprintSpeed /= boostAmount;
        gameManager.instance.playerSpeedBoost.SetActive(false);
        Destroy(speedPowerup);
    }
}