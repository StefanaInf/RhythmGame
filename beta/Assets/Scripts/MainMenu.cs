using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using Unity.VisualScripting;
using TMPro;
using System;

public class MainMenu : MonoBehaviour
{
    [SerializeField] public AudioMixer audioMixer;
    [SerializeField] public Slider musicSlider;
    [SerializeField] public AudioSource audioSource;
    [SerializeField] public TMP_Dropdown qualityDropdown;
    [SerializeField] public TMP_Dropdown resolutionDropdown;
    [SerializeField] public Toggle fullscreenToggle;
    Resolution[] resolutions;

    private void Start()
    {
        if(PlayerPrefs.HasKey("MainVolume")){ 
            LoadVolume();
        }
        else{
            SetMusicVolume();
        }
        if (PlayerPrefs.HasKey("QualityLevel")){
            LoadQuality();
        }
        else{
            SetQuality(QualitySettings.GetQualityLevel());
        }
        if (PlayerPrefs.HasKey("Fullscreen")){
            LoadFullscreen();
        }
        else{
            SetFullscreen(Screen.fullScreen);
        }

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for(int  i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    //SETTERS
    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("MainVolume", Mathf.Log10(volume)*20);
        audioSource.volume = volume;
        PlayerPrefs.SetFloat("MainVolume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("QualityLevel", qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }

    //LOAD
    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MainVolume");
        SetMusicVolume();
    }

    private void LoadQuality()
    {
        int qualityIndex = PlayerPrefs.GetInt("QualityLevel");
        qualityDropdown.value = qualityIndex;
        SetQuality(qualityIndex);
    } 
    private void LoadFullscreen()
    {
        bool isFullscreen;
        if (PlayerPrefs.GetInt("Fullscreen") == 1){
            isFullscreen = true;
        }
        else{
            isFullscreen = false;
        }
        fullscreenToggle.isOn = isFullscreen;
        SetFullscreen(isFullscreen);
    }
   
    //BUTTONS
    public void QuitGame()
    {
        Debug.Log("Exited the game!");
        Application.Quit();
    }

    public void LoadGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("loadFromJsonTest");

    }
}
