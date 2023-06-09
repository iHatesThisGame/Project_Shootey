using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{
    [Range(1, 100)][SerializeField] int punchDamage;
    [Range(1, 100)][SerializeField] float punchForce;
    [Range(1, 100)][SerializeField] float punchRange;

    public void ApplyPunch(GameObject player)
    {
        Vector3 punchDirection = player.transform.position - transform.position;

        if (punchDirection.magnitude <= punchRange)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, punchDirection, out hit, punchRange))
            {
                if (hit.collider.gameObject == player)
                {
                    playerController playerHealth = player.GetComponent<playerController>();
                    if (playerHealth != null)
                    {
                        playerHealth.takeDamage(punchDamage);
                    }
                }
            }
        }

        Destroy(gameObject);
    }
}

