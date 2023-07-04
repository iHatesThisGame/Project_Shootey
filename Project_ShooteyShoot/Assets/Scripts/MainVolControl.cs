using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainVolControl : MonoBehaviour
{
    [SerializeField] string volParameter = "MasterVolume";
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider slider;
    [SerializeField] Toggle toggle;
    private bool disableToggleEvent;

    private void Awake()
    {
        slider.onValueChanged.AddListener(SlideValChange);
        toggle.onValueChanged.AddListener(ToggleValChange);
    }

    private void ToggleValChange(bool enableSound)
    {
        if (disableToggleEvent)
        {
            return;
        }

        if (enableSound)
        {
            slider.value = 0;
        }
        else
        {
            slider.value = slider.minValue;
        }
    }

    private void SlideValChange(float val)
    {
        mixer.SetFloat(volParameter, val);
        disableToggleEvent = true;
        toggle.isOn = slider.value > slider.minValue;
        disableToggleEvent = false;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(volParameter,slider.value);
    }

    // Start is called before the first frame update
    void Start()
    {
        slider.value = PlayerPrefs.GetFloat(volParameter,slider.value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
