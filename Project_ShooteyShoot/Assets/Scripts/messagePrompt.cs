using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class messagePrompt : MonoBehaviour
{
    [SerializeField] string message;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.messagePromptText.text = message;
            gameManager.instance.messagePromptPopup.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.messagePromptText.text = "message";
            gameManager.instance.messagePromptPopup.SetActive(false);
        }
    }
}
