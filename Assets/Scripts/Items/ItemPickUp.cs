using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : InteractableItems
{
    public Item item; //reference to item being picked up

    public override void Interact()
    {
        base.Interact(); //run base code from Interact function - currently this just puts a message into the Debug Log saying an object was interacted with

        PickUpItem(); //pick up the item
    }

    public void PickUpItem()
    {
        Debug.Log(item.name + " picked up..."); //debug to show item was picked up
        bool itemPickedUp = Inventory.instance.AddItem(item); //reference the inventory instance and add the item to the inventory - store result to a bool to check if item was picked up or not

        if (itemPickedUp) //if item was successfully picked up
        {
            Destroy(gameObject); //destroy the game object
        }
    }
}
