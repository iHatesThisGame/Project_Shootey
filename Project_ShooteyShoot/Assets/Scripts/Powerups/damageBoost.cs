using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageBoost : MonoBehaviour
{
    [SerializeField] GameObject damagePowerup;

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
        damagePowerup.transform.position -= despawn;
        gameManager.instance.playerController.shootDamage *= 2;
        yield return new WaitForSeconds(5);
        gameManager.instance.playerController.shootDamage /= 2;
        Destroy(damagePowerup);
    }
}