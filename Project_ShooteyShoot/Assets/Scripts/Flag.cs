using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    [Header("----- Flag -----")]
    [SerializeField] GameObject enemyFlag;

    private void OnTriggerEnter(Collider other)
    {
        ICapture captureable = other.GetComponent<ICapture>(); 
        if (captureable != null)
        {
            Destroy(enemyFlag);
        }
    }
}