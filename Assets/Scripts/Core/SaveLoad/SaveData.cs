using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class SaveData
{
    private static SaveData _current;
    public static SaveData current
    {
        get
        {
            if(_current == null)
            {
                _current = new SaveData();
            }
            return _current;
        }
        set
        {
            _current = value;
        }
    }

    public PlayerProfile profile = new PlayerProfile();
    

    public void SavePlayerProfile()
    {
        
        SerializationManager.Save(Application.persistentDataPath + "/SaveData/PlayerProfile.save", _current);
    }



}