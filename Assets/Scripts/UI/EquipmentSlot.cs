using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class EquipmentSlot : MonoBehaviour
{
    Equipment equipment; //reference to equipment script

    public Image icon; //reference to equipment image

    public SlotType slotType; //create new slot type variable

    public EquipmentManager equipmentManager; //reference to EquipmentManager script

    public int slotIndex; //custom slot index for each equipment slot - 0 = helmet, 1 = chestplate, 2 = legs, 3 = boots, 4 = weapon, 5 = offhand

    public void AddEquipment(Equipment newItem)
    {
        equipment = newItem; //set item as the new equipment
        icon.sprite = equipment.itemIcon; //set equipment icon to new item's icon
        icon.enabled = true; //set icon to enabled state

        //check if equipment already exists here
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! wud uiawui da w   waid wa 
    }

    public void ClearEquipmentSlot()
    {
        if(equipment != null) //if equipment exists
        {
            equipment = null; //set item to null
            icon.sprite = null; //set icon sprite to null
            icon.enabled = false; //disable icon

            equipmentManager.Unequip(slotIndex); //unequip the corresponding piece of equipment
        }
    }

    public enum SlotType { Head, Chest, Legs, Boots, Weapon, Offhand } //new enum to determine what type of equipment the slot takes
}
