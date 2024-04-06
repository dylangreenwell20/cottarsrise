using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

        //check if consumable picked up - this does not get added to inventory but a seperate counter

        Debug.Log(item.GetType()); //for testing item type

        if((item.GetType().ToString()) == "Consumable") //if item type is consumable
        {
            Consumable consumable = (Consumable)item; //get consumable type of item

            if(consumable.consumableType == ConsumableType.Health) //if consumable is of type health
            {
                Debug.Log("health pot + 1");

                Inventory.instance.healthPotCount++; //increment amount of health potions
                Inventory.instance.potionUI.UpdateHealthPotionUI(); //update health potion ui

                Destroy(gameObject); //destroy the game object
            }
            if (consumable.consumableType == ConsumableType.Mana) //if consumable is of type mana
            {
                Debug.Log("mana pot + 1");

                Inventory.instance.manaPotCount++; //increment amount of mana potions
                Inventory.instance.potionUI.UpdateManaPotionUI(); //update mana potion ui

                Destroy(gameObject); //destroy the game object
            }
            if (consumable.consumableType == ConsumableType.Arrow) //if consumable is of type arrow
            {
                Debug.Log("arrows + 1");

                Inventory.instance.arrowCounter.GainArrow(Inventory.instance.arrowCounter.arrowRestoreAmount); //increase amount of arrows by arrow restore amount
                Inventory.instance.arrowCounter.UpdateUI(); //update arrow ui

                Destroy(gameObject);
            }
        }
        else
        {
            bool itemPickedUp = Inventory.instance.AddItem(item); //reference the inventory instance and add the item to the inventory - store result to a bool to check if item was picked up or not

            if (itemPickedUp) //if item was successfully picked up
            {
                Destroy(gameObject); //destroy the game object
            }
        }
    }
}
