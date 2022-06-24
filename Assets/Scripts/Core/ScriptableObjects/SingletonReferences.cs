using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonReferences : MonoBehaviour
{
    private void Awake() {
        DontDestroyOnLoad(this);
    }
    
    public MasterManager MasterManager;
 
}