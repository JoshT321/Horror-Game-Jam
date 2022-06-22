using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class PlayerProfile
{
    public float musicVolume;
    public float sfxVolume;
    public float mouseSensitivity;
    public float brightness; 
    public int checkpointIndex;
    public string currentSceneName; 
    public int resolution;
    public bool isFullScreen;
    
    public void SaveMusicVolume(float newValue)
    {
        musicVolume = newValue;
        SaveData.current.SavePlayerProfile();
    }

    public void SaveSFXVolume(float newValue)
    {
        sfxVolume = newValue;
        SaveData.current.SavePlayerProfile();
    }

    public void SaveResolution(int resolutionIndex)
    {
        resolution = resolutionIndex; 
        SaveData.current.SavePlayerProfile();
    }

    public void SaveFullScreen(bool _isFullScreen)
    {
        isFullScreen = _isFullScreen;
        SaveData.current.SavePlayerProfile(); 
    }

    public void SaveMouseSensitivity(float newValue)
    {
        mouseSensitivity = newValue;
        SaveData.current.SavePlayerProfile();
    }

    public void SaveBrightness(float newValue)
    {
        brightness = newValue;
        SaveData.current.SavePlayerProfile();
    }
    
    

}
