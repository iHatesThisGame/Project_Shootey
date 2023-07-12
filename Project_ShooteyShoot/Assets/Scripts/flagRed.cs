using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flagRed : MonoBehaviour
{
    [Header("----- Flag -----")]
    [SerializeField] GameObject flag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(flag);
            gameManager.instance.playerController.hasFlag = true;
        }
    }
}
