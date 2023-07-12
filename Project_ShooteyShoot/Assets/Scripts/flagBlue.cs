using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flagBlue : MonoBehaviour
{
    [Header("----- Flag -----")]
    [SerializeField] GameObject flag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CTF Enemy"))
        {
            Destroy(flag);
            Debug.Log("Flag destroyed");
        }
    }
}
