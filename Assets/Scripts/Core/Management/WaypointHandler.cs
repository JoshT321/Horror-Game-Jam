using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointHandler : MonoBehaviour
{
    //public static WaypointHandler Instance;
    public List<Vector3> WaypointDestinations;


    private void Awake()
    {
        Initialize();
                
        // if (Instance != null && Instance != this)
        // {
        //     Debug.Log("Destroying WaypointHandler");
        //     Destroy(this.gameObject);
        // }
        // else
        //     Instance = this;
    }

    private void Initialize()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            WaypointDestinations.Add(transform.GetChild(i).position);
        }
        
        
    }

    public int GetIndexAtPosition(Vector3 targetPosition)
    {
        for (int i = 0; i < WaypointDestinations.Count; i++)
        {
            if (WaypointDestinations[i] == targetPosition)
            {
                return i; 
            }
        }
        return 0; 
    }

    public Vector3 GetPositionAtIndex(int targetIndex) => WaypointDestinations[targetIndex];
}
