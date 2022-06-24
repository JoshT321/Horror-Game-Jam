using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[CreateAssetMenu(fileName = "ItemDatabase", menuName = "ScriptableObjects/Databases/ItemDatabase", order = 0)]
public class ItemDatabase : ScriptableObject
{
    public List<Item> Items;


    public Item GetItemByID(int targetID) => Items.Find(x => x.itemID == targetID);
}
