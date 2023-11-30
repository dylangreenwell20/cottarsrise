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

    private void Awake()
    {
        inventory.SetActive(false); //hide the inventory on awake
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)){ //if tab pressed
            if (!inventoryOpen)
            {
                inventory.SetActive(true); //open inventory
                
                pc.UnlockCursor(); //unlock the cursor so player can use cursor for inventory

                inventoryOpen = true; //inv is open
            }
            else
            {
                inventory.SetActive(false); //close inventory

                pc.LockCursor(); //lock the cursor so player can look around again

                inventoryOpen = false; ; //inv is closed
            }
        }
    }
}
