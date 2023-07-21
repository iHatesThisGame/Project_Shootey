using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainVolControl : MonoBehaviour
{
    [SerializeField] string parMasterVol = "MasterVolume";
    [SerializeField] string parMusicVol = "MusicVolume";
    [SerializeField] string parSFXVol = "SFXVolume";
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Toggle masterToggle;
    [SerializeField] Toggle musicToggle;
    [SerializeField] Toggle sfxToggle;
    private bool disableMasterToggleEvent;
    private bool disableMusicToggleEvent;
    private bool disableSFXToggleEvent;

    //[SerializeField] AudioSource aud;
    //[SerializeField] AudioClip PreviewSound;
    //[Range(0, 1)] [SerializeField] float audPreviewSoundVol;

    private void Awake()
    {
        masterSlider.onValueChanged.AddListener(MasterSlideValChange);
        masterToggle.onValueChanged.AddListener(ToggleMasterValChange);
        musicSlider.onValueChanged.AddListener(MusicSlideValChange);
        musicToggle.onValueChanged.AddListener(ToggleMMusicValChange);
        sfxSlider.onValueChanged.AddListener(SFXSlideValChange);
        sfxToggle.onValueChanged.AddListener(ToggleSFXValChange);
    }


    private void OnDisable()
    {
        PlayerPrefs.SetFloat(parMasterVol, masterSlider.value);
        PlayerPrefs.SetFloat(parMusicVol, musicSlider.value);
        PlayerPrefs.SetFloat(parSFXVol, sfxSlider.value);
    }

    // Start is called before the first frame update
    void Start()
    {
        masterSlider.value = PlayerPrefs.GetFloat(parMasterVol,masterSlider.value);
        musicSlider.value = PlayerPrefs.GetFloat(parMusicVol, musicSlider.value);
        sfxSlider.value = PlayerPrefs.GetFloat(parSFXVol, sfxSlider.value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ToggleMasterValChange(bool enableSound)
    {
        if (disableMasterToggleEvent)
        {
            return;
        }

        if (enableSound)
        {
            masterSlider.value = .8f;
        }
        else
        {
            masterSlider.value = masterSlider.minValue;
        }
    }

    private void MasterSlideValChange(float val)
    {
        mixer.SetFloat(parMasterVol, Mathf.Log10(val) * 20);
        disableMasterToggleEvent = true;
        masterToggle.isOn = masterSlider.value > masterSlider.minValue;
        disableMasterToggleEvent = false;
        //aud.PlayOneShot(PreviewSound, audPreviewSoundVol);
    }

    private void ToggleMMusicValChange(bool enableSound)
    {
        if (disableMusicToggleEvent)
        {
            return;
        }

        if (enableSound)
        {
            musicSlider.value = .8f;
        }
        else
        {
            musicSlider.value = musicSlider.minValue;
        }
    }

    private void MusicSlideValChange(float val)
    {
        mixer.SetFloat(parMusicVol, Mathf.Log10(val) * 20);
        disableMusicToggleEvent = true;
        musicToggle.isOn = musicSlider.value > musicSlider.minValue;
        disableMusicToggleEvent = false;
        //aud.PlayOneShot(PreviewSound, audPreviewSoundVol);
    }

    private void ToggleSFXValChange(bool enableSound)
    {
        if (disableSFXToggleEvent)
        {
            return;
        }

        if (enableSound)
        {
            sfxSlider.value = .8f;
        }
        else
        {
            sfxSlider.value = sfxSlider.minValue;
        }
    }

    private void SFXSlideValChange(float val)
    {
        mixer.SetFloat(parSFXVol, Mathf.Log10(val) * 20);
        disableSFXToggleEvent = true;
        sfxToggle.isOn = sfxSlider.value > sfxSlider.minValue;
        disableSFXToggleEvent = false;
        //aud.PlayOneShot(PreviewSound, audPreviewSoundVol);
    }
}
