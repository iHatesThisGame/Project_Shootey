using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class showFlag : MonoBehaviour
{
    public Image flag;
    public bool hasFlag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hasFlag = true;
            flag.enabled = true;
        }
    }
}
