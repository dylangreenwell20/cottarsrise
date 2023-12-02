using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    Item item; //reference to item script

    public Image icon; //reference to item image
    public Button removeButton; //reference to remove item button

    public void AddItem(Item newItem)
    {
        item = newItem; //set item as the new item
        icon.sprite = item.itemIcon; //set item icon to new item's icon
        icon.enabled = true; //set icon to enabled state
        removeButton.interactable = true; //make button interactable when there is an item
    }

    public void ClearItemSlot()
    {
        item = null; //set item to null
        icon.sprite = null; //set icon sprite to null
        icon.enabled = false; //disable icon
        removeButton.interactable = false; //make button not interactable when there is no item
    }

    public void OnRemoveButtonPress()
    {
        Inventory.instance.RemoveItem(item); //remove the current item from the inventory slot
    }

    public void UseItem()
    {
        if (item != null) //if item exists
        {
            item.Use(); //use the item
        }
    }
}
