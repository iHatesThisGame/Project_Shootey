using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedBoost : MonoBehaviour
{
    [SerializeField] GameObject speedPowerup;

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
        gameManager.instance.playerController.playerSpeedOrig *= 2;
        gameManager.instance.playerController.sprintSpeed *= 2;
        yield return new WaitForSeconds(5);
        gameManager.instance.playerController.playerSpeedOrig /= 2;
        gameManager.instance.playerController.sprintSpeed /= 2;
        Destroy(speedPowerup);
    }
}