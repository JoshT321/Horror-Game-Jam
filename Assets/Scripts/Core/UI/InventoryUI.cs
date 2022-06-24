using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject itemEntry;
    public Transform inventoryBase;
    
    void Start()
    {
        inventoryBase = transform.Find("Inventory").Find("Base");
    }


    public void AddToInventoryUI(Item newItem)
    {
        GameObject newItemEntry = Instantiate(itemEntry, inventoryBase);
        Image newItemEntryImage = newItemEntry.GetComponent<Image>();

        newItemEntry.name = newItem.itemName;
        newItemEntryImage.sprite = newItem.inventorySprite;
    }

    public void RemoveFromInventoryUI(string itemName)
    {
        foreach (Transform child in inventoryBase)
            if (child.name == itemName)
                Destroy(child.gameObject);
    }
}
