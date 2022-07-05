using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBase : MonoBehaviour
{
    public ShadowDetector ShadowDetector;
    public PlayerController playerController;
    public PlayerInventory playerInventory;


    private void Awake()
    {
        ShadowDetector = GetComponent<ShadowDetector>();
        playerController = GetComponent<PlayerController>();
        playerInventory = GetComponent<PlayerInventory>(); 
    }

    public static PlayerBase Find()
    {
        return FindObjectOfType<PlayerBase>(); 
    }
}
