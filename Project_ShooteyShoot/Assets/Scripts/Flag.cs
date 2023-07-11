using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    [Header("----- Flag -----")]
    [SerializeField] GameObject enemyFlag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(enemyFlag);
            gameManager.instance.playerController.hasFlag = true; 
        }
        if (!other.CompareTag("CTF Enemy"))
        {
            Destroy(enemyFlag);
        }
    }
}