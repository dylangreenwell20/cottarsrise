using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour
{
    public GameObject inventory; //reference to inventory ui
    public bool inventoryOpen; //is inventory open or not

    public PlayerCamera pc; //reference to player camera for unlocking the cursor

    public List<Item> items = new List<Item>(); //create new list for storing items

    public static Inventory instance; //create singleton of inventory

    public int inventorySize = 25; //size of inventory
    public int healthPotCount, manaPotCount, arrowCount; //number of health/mana potions and arrows

    public delegate void OnItemChanged(); //create new delegate type
    public OnItemChanged onItemChangedCallback; //create new delegate callback to implement the delegate

    public GameObject arrowCountUI, currentWeaponUI; //references to UI elements which need to be hidden when inventory is open

    public PotionUI potionUI; //reference to potion ui script
    public ArrowCounter arrowCounter; //reference to arrow counter script

    private void Awake()
    {
        if(instance != null) //if instance already created
        {
            Debug.LogWarning("More than one Inventory instance was found..."); //send debug message to inform of the error
            return; //return function
        }

        instance = this; //set instance to this (inventory)
        inventory.SetActive(false); //hide the inventory on awake
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)){ //if tab pressed
            if (!inventoryOpen)
            {
                inventory.SetActive(true); //open inventory
                
                pc.UnlockCursor(); //unlock the cursor so player can use cursor for inventory

                if(StartingWeapon.archerClassSelected) //if archer class selected
                {
                    arrowCountUI.SetActive(false); //hide arrow count UI
                }
                
                currentWeaponUI.SetActive(false); //hide current weapon ui

                inventoryOpen = true; //inv is open
            }
            else
            {
                inventory.SetActive(false); //close inventory

                pc.LockCursor(); //lock the cursor so player can look around again

                if (StartingWeapon.archerClassSelected) //if archer class selected
                {
                    arrowCountUI.SetActive(true); //show arrow count UI
                }
                
                currentWeaponUI.SetActive(true); //show current weapon ui

                inventoryOpen = false; ; //inv is closed
            }
        }
    }

    public bool AddItem (Item item)
    {
        if(!item.isDefaultItem) //if item is not a default (starting) item
        {
            if(items.Count >= inventorySize) //if inventory size has been reached
            {
                Debug.Log("Not enough room in inventory!"); //output message that inventory is full
                return false; //return false as item was not added
            }
            items.Add(item); //add item to list

            if(onItemChangedCallback != null) //if the callback exists
            {
                onItemChangedCallback.Invoke(); //invoke the delegate callback
            }
        }
        return true; //return true as item was added
    }

    public void RemoveItem (Item item)
    {
        items.Remove(item); //remove item from list

        if (onItemChangedCallback != null) //if the callback exists
        {
            onItemChangedCallback.Invoke(); //invoke the delegate callback
        }
    }

    public void AddConsumable (Item item)
    {

    }
}
