using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunPickup : MonoBehaviour
{
    [SerializeField] gunStats gun;
    private bool pickupInRange;

    // Start is called before the first frame update
    void Start()
    {
        gun.ammoCur = gun.ammoMax;
    }

    void Update()
    {
        if (Input.GetButton("Interact") && pickupInRange == true)       //pickup weapon when button is pressed
        {
            gameManager.instance.playerController.gunPickup(gun);
            Destroy(gameObject);
            gameManager.instance.interactPrompt.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.interactText.text = ("E to Pickup");
            gameManager.instance.interactPrompt.SetActive(true);          //display interact message
            pickupInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.interactPrompt.SetActive(false);
            gameManager.instance.interactText.text = ("E");
        }
    }
}
