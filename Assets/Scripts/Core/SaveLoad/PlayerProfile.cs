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
    public List<int> inventoryByItemID = new(); 

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

    public void ClearSavedInventory() => inventoryByItemID.Clear();

    public void SaveItemIDToInventory(int itemID )
    {
        Debug.Log("Saving " + itemID + " To player profile");
        inventoryByItemID.Add(itemID);
        SaveData.current.SavePlayerProfile();
    }
    
    

}
