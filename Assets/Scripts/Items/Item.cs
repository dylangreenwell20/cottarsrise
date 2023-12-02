using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]

public class Item : ScriptableObject
{
    new public string name = "Item name"; //overwrite and create new item name variable
    public Sprite itemIcon = null; //create sprite for item
    public bool isDefaultItem; //if item is a default item - essentially if its an item the player starts a dungeon with

    public virtual void Use()
    {
        Debug.Log("Used " + name); //currently just a debug message to show the item was 'used'
    }
}
