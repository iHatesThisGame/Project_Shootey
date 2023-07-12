using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageBoost : MonoBehaviour
{
    [SerializeField] GameObject damagePowerup;
    [SerializeField] int boostAmount;
    [SerializeField] int duration;

    [Header("----- Audio -----")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip audPickup;
    [Range(0, 1)][SerializeField] float audPickupVol;

    Vector3 despawn = new Vector3(-50, -50, -50);

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

        damagePowerup.transform.position -= despawn;
        gameManager.instance.playerController.shootDamage *= boostAmount;
        gameManager.instance.playerDamageBoost.SetActive(true);
        yield return new WaitForSeconds(duration);
        gameManager.instance.playerController.shootDamage /= boostAmount;
        gameManager.instance.playerDamageBoost.SetActive(false);
        Destroy(damagePowerup);
    }
}