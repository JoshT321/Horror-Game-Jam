using System;
using System.Collections;
using System.IO;
using TMPro;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsHandler : MonoBehaviour
{
  public List<Transform> buttonObjs;
  public GameObject optionsMenuObj;
  public float musicVolume;
  public float sfxVolume;
  public float mouseSensitivity; 
  public int resolutionIndex;
  public bool isFullscreen;
  public TMP_Dropdown resolutionDropdown; 
  private Resolution[] resolutions;
  public bool isDataDirty;
  
  public Slider musicSlider;
  public Slider sfxSlider;
  public Slider sensitivitySlider;
  public Slider brightnessSlider;
  public Toggle fullscreenToggle;
  
  
  

  private void Awake()
  {
    foreach (Transform child in transform.Find("Buttons")) buttonObjs.Add(child);
    optionsMenuObj = transform.Find("Options Background").gameObject;
    //resolutionDropdown = ; 
    resolutions = Screen.resolutions;

    musicSlider = transform.Find("Options Background").Find("Music Volume").GetComponent<Slider>();
    sfxSlider = transform.Find("Options Background").Find("SFX Volume").GetComponent<Slider>();
    sensitivitySlider = transform.Find("Options Background").Find("Mouse Sensitivity").GetComponent<Slider>();
    brightnessSlider = transform.Find("Options Background").Find("Brightness").GetComponent<Slider>();
    fullscreenToggle = transform.Find("Options Background").Find("FullscreenToggle").GetComponent<Toggle>();
    
  }

  private void Start()
  {
    GetResolutions();
    InitializeSaveData();
  }

  private void InitializeSaveData()
  {
    musicSlider.value = SaveData.current.profile.musicVolume;
    sfxSlider.value = SaveData.current.profile.sfxVolume;
    sensitivitySlider.value = SaveData.current.profile.mouseSensitivity;
    fullscreenToggle.isOn = SaveData.current.profile.isFullScreen;
    resolutionDropdown.value = SaveData.current.profile.resolution;
    brightnessSlider.value = SaveData.current.profile.brightness; 

  }

  public void GetResolutions()
  {
    List<string> options = new List<string>();
    int currentResolutionIndex = 0;
    resolutionDropdown = transform.Find("Options Background").Find("ResolutionDropdown").GetComponent<TMP_Dropdown>();
    transform.Find("Options Background").Find("ResolutionDropdown").GetComponent<TMP_Dropdown>().ClearOptions();
        
    for (int i = 0; i < resolutions.Length; i++)
    {
      string option = resolutions[i].width + " x " + resolutions[i].height;
      if (options.Contains(option)) continue;

      options.Add(option);

      if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
      {
        currentResolutionIndex = i;
      }
    }
    resolutionDropdown.AddOptions(options);
    resolutionDropdown.value = SaveData.current.profile.resolution == 0 ?  currentResolutionIndex : SaveData.current.profile.resolution;
    resolutionDropdown.RefreshShownValue();
        
    Screen.fullScreen = SaveData.current.profile.isFullScreen;
  }

  
  public void OnMusicVolumeChanged(float value) => GameManager.Instance.audioMixer.SetFloat("Music", value);

  public void OnSFXVolumeChanged(float newValue) => GameManager.Instance.audioMixer.SetFloat("SFX", newValue);

  public void OnOptionsClickedButton() => optionsMenuObj.gameObject.SetActive(true);

  public void OnOptionsBackClickedButton()
  {
    SaveCurrentOptions(); 
    optionsMenuObj.SetActive(false);
  }

  public void ToggleFullScreenButton(bool newStatus)
  {
    
    //Screen.fullScreen = !Screen.fullScreen;
    if(newStatus)
    {
      Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
    }
    else
    {
      Screen.fullScreenMode = FullScreenMode.Windowed;
    }
    Debug.Log("Set fullscreen to " + Screen.fullScreenMode);
  }
  public void SetResolution(int newResolutionIndex)
  {
    Resolution resolution = resolutions[newResolutionIndex];
    Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    resolutionIndex = newResolutionIndex;
  }

  public void OnMouseSensitivityChanged(float newValue)
  {
    mouseSensitivity = newValue;
    SaveData.current.profile.mouseSensitivity = newValue;
  }

  public void OnBrightnessChanged(float newValue) => PostProcessingHandler.Instance.SetBrightness(newValue);

  public void OnClearSaveDataButton()
  {
    string profilePath = Application.persistentDataPath + "/SaveData/PlayerProfile.save";
    File.Delete(profilePath);
    GameManager.Instance.InitializePlayerProfile();
  }
  

  public void SaveCurrentOptions()
  {
    Debug.Log("Saving Options... Add dirty data checker if time permits");
    SaveData.current.profile.SaveResolution(resolutionIndex);
    SaveData.current.profile.SaveMusicVolume(musicSlider.value);
    SaveData.current.profile.SaveSFXVolume(sfxSlider.value);
    SaveData.current.profile.SaveFullScreen(Screen.fullScreen);
    SaveData.current.profile.SaveMouseSensitivity(sensitivitySlider.value);
    SaveData.current.profile.SaveBrightness(brightnessSlider.value);

  }
}
