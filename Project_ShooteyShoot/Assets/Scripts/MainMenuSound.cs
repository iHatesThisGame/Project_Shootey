using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSound : MonoBehaviour
{

    [Header("----- Audio -----")]
    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip buttonClickSound;
    [SerializeField] AudioClip buttonHighlightSound;
    [SerializeField] AudioClip screenShiftSound;
    [Range(0, 1)] [SerializeField] float audPauseSoundVol;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"))
        {
            aud.PlayOneShot(buttonHighlightSound, audPauseSoundVol);
        }
        if (Input.GetButtonDown("Shoot"))
        {
            aud.PlayOneShot(buttonClickSound, audPauseSoundVol);
        }
    }

    public void ScreenShiftSound()
    {
        aud.PlayOneShot(screenShiftSound, audPauseSoundVol);
    }
}
