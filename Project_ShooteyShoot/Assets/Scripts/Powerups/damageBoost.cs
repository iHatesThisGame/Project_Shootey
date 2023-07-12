using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageBoost : MonoBehaviour
{
    [SerializeField] GameObject damagePowerup;
    [SerializeField] int boostAmount;
    [SerializeField] int duration;

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
        gameManager.instance.playerController.shootDamage *= boostAmount;
        yield return new WaitForSeconds(duration);
        gameManager.instance.playerController.shootDamage /= boostAmount;
        Destroy(damagePowerup);
    }
}