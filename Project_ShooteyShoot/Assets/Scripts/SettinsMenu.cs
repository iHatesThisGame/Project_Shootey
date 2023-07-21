using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SettinsMenu : MonoBehaviour
{
    Resolution[] resolutions;
    [SerializeField] TMP_Dropdown resDropdown;
    [SerializeField] int sensitivity;
    [SerializeField] Slider sensitivitySlider;
    [SerializeField] string parSensitivity = "Mouse Sensitivity";

    private void Awake()
    {
        sensitivitySlider.onValueChanged.AddListener(SensSlideValChange);
    }

    public void SensSlideValChange(float arg0)
    {
        sensitivity = (int)sensitivitySlider.value;
    }

    public void Start()
    {
        resolutions = Screen.resolutions;
        resDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currResIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currResIndex = i;
            }
        }
        resDropdown.AddOptions(options);
        resDropdown.value = currResIndex;
        resDropdown.RefreshShownValue();

        PlayerPrefs.GetFloat(parSensitivity, sensitivitySlider.value);

    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(parSensitivity, sensitivitySlider.value);
    }


    public void SetVolume(float vol)
    {

    }

    public void SetFullscreen(bool isFull)
    {
        Screen.fullScreen = isFull;

        if (isFull == true)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void SetRes(int resIndex)
    {
        Resolution resolution = resolutions[resIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }




}
