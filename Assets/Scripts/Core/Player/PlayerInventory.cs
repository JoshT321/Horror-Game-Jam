using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public List<Item> inventory; 
    private void Awake()
    {
        
    }

    private void Start()
    {
        InitializePlayerInventory();
    }

    private void Update()
    {
        //TODO: Remove this when Autosave spots are made
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (var newItem in inventory)
            {
                SaveData.current.profile.SaveItemIDToInventory(newItem.itemID);
            }
        }
    }

    private void InitializePlayerInventory()
    {
        for (int i = 0; i < SaveData.current.profile.inventoryByItemID.Count; i++)
        {
            AddToInventory(MasterManager.ItemDatabase.GetItemByID(SaveData.current.profile.inventoryByItemID[i]));
            i++;
        }
    }

    public void AddToInventory(Item newItem)
    {
        inventory.Add(newItem);
        UIManager.Instance.InventoryUI.AddToInventoryUI(newItem);
        
        
    }
}
