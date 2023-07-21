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
    bool isOpen = false;
    bool isFacingDoor;

    void Update()
    {
        FacingDoor();
        OpenDoor();
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
            isOpen = false;
            gameManager.instance.interactPrompt.SetActive(false);
            gameManager.instance.interactText.text = ("E");
        }
    }

    private void OpenDoor()
    {
        if (Input.GetButtonDown("Interact") && doorInRange == true && isFacingDoor == true)
        {
            if (isOpen == false)
            {
                aud.PlayOneShot(audDoorOpen, audDoorOpenVol);
            }
            door.SetActive(false);
            isOpen = true;
            gameManager.instance.interactPrompt.SetActive(false);
        }
    }

    public void FacingDoor()
    {
        var direction = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ViewportPointToRay(direction), out hit, 5))
        {
            //Debug.Log("Raycast works");
            if (hit.collider.name == "Door")
            {
                //Debug.Log("Tag works");
                isFacingDoor = true;
            }
            else
            {
                isFacingDoor = false;
            }
           
        }
    }
}
