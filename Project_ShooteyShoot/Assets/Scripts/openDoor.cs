using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;

public class openDoor : MonoBehaviour
{
    [SerializeField] GameObject door;

    [Header("----- Audio -----")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip audDoorOpen;
    [Range(0, 1)][SerializeField] float audDoorOpenVol;

    bool doorInRange;

    void Update()
    {
        if (Input.GetButton("Interact") && doorInRange == true)
        {
            door.SetActive(false);
            aud.PlayOneShot(audDoorOpen, audDoorOpenVol);
            gameManager.instance.interactPrompt.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorInRange = true;
            gameManager.instance.interactText.text = ("E to Open");
            gameManager.instance.interactPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            doorInRange = false;
            door.SetActive(true);
            aud.PlayOneShot(audDoorOpen, audDoorOpenVol);
            gameManager.instance.interactPrompt.SetActive(false);
            gameManager.instance.interactText.text = ("E");
        }
    }
}
