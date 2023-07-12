using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedBoost : MonoBehaviour
{
    [SerializeField] GameObject speedPowerup;
    [SerializeField] int boostAmount;
    [SerializeField] int duration;

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
        speedPowerup.transform.position -= despawn;
        gameManager.instance.playerController.playerSpeedOrig *= boostAmount;
        gameManager.instance.playerController.sprintSpeed *= boostAmount;
        yield return new WaitForSeconds(duration);
        gameManager.instance.playerController.playerSpeedOrig /= boostAmount;
        gameManager.instance.playerController.sprintSpeed /= boostAmount;
        Destroy(speedPowerup);
    }
}