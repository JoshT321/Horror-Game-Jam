using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBase : MonoBehaviour
{
    public ShadowDetector ShadowDetector;
    public PlayerController playerController;


    private void Awake()
    {
        ShadowDetector = GetComponent<ShadowDetector>();
        playerController = GetComponent<PlayerController>(); 
    }
}
