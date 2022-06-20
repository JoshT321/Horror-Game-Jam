using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class PlayerProfile
{
    public float musicVolume;
    public float sfxVolume;
    public int checkpointIndex;
    public string currentSceneName; 
    public int resolution;
    public bool isFullScreen;
    
    public void SetMusicVolume(float newValue)
    {
        musicVolume = newValue;
        SaveData.current.SavePlayerProfile();
    }

    public void SetSFXVolume(float newValue)
    {
        sfxVolume = newValue;
        SaveData.current.SavePlayerProfile();
    }

    public void SetResolution(int resolutionIndex)
    {
        resolution = resolutionIndex; 
        SaveData.current.SavePlayerProfile();
    }

    public void SetFullScreen(bool _isFullScreen)
    {
        isFullScreen = _isFullScreen;
        SaveData.current.SavePlayerProfile(); 
    }
    
    

}
