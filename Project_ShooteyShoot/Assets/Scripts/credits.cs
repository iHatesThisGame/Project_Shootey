using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class credits : MonoBehaviour
{
    [SerializeField] int creditsDuration;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RollCredits());
    }

    private void Update()
    {
        if (Input.GetButton("Pause"))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            SceneManager.LoadScene("Main Menu");
        }
    }

    IEnumerator RollCredits()
    {
        yield return new WaitForSeconds(creditsDuration);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        SceneManager.LoadScene("Main Menu");
    }
}
