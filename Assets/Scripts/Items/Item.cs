using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]

public class Item : ScriptableObject
{
    new public string name = "Item name"; //overwrite and create new item name variable
    public Sprite itemIcon = null; //create sprite for item
    public bool isDefaultItem; //if item is a default item - essentially if its an item the player starts a dungeon with
    public bool isEquippable; //if item is equippable

    public virtual void Use()
    {
        Debug.Log("Used " + name); //currently just a debug message to show the item was 'used'
    }

    public void RemoveFromInventory()
    {
        Inventory.instance.RemoveItem(this); //remove current item from the inventory
    }
}
