using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacleCourseStart : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.courseStarted = true;
        }
    }
}
