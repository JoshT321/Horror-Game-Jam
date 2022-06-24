using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public PlayerCursor PlayerCursor;
    public InventoryUI InventoryUI;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.Log("Destroying UIManager");
            Destroy(this.gameObject);
        }
        else
            Instance = this;
        PlayerCursor = transform.Find("Cursor").GetComponent<PlayerCursor>();
        InventoryUI = GetComponent<InventoryUI>();
    }

    void Start()
    {



    }



}
