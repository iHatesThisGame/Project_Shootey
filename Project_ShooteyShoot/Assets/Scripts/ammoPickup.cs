using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoPickup : MonoBehaviour
{
    [SerializeField] public int ammoAmount;

    private void OnTriggerEnter(Collider other)
    {
        IAmmo hasAmmo = other.GetComponent<IAmmo>();

        if(hasAmmo != null)
        {
            hasAmmo.pickupAmmo(ammoAmount, gameObject);
        }
    }
}
