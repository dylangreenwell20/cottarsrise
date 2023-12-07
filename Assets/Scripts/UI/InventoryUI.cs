using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    Inventory inventory; //inventory reference

    public Transform itemsParent; //reference to parent of the item slots

    InventorySlot[] slots; //create array of inventory slots

    private void Start()
    {
        inventory = Inventory.instance; //set inventory to the instance of Inventory
        inventory.onItemChangedCallback += UpdateItemUI; //call UpdateUI whenever onItemChangedCallback is run

        slots = itemsParent.GetComponentsInChildren<InventorySlot>(); //set amount of inventory slots to the array 'slots'
    }

    public void UpdateItemUI()
    {
        for(int i = 0; i < slots.Length; i++) //for the length of the 'slots' array
        {
            if(i < inventory.items.Count) //if there are items to add
            {
                slots[i].AddItem(inventory.items[i]); //add item to specific inventory slot
            }
            else //else if there are no items to add
            {
                slots[i].ClearItemSlot(); //clear the specific item slot
            }
        }
    }
}
