using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance; //create singleton instance

    public SelectedWeapon sW; //reference to selected weapon script

    private void Awake()
    {
        instance = this; //set instance to this class
    }

    public Equipment[] currentEquipment; //current equipment array to store what the player has equipped

    public delegate void OnEquipmentChanged(Equipment newItem, Equipment equippedItem); //create delegate for other classes to use the unequip feature
    public OnEquipmentChanged onEquipmentChanged; //create a public version of the delegate

    Inventory inventory; //reference to inventory

    public GameObject headSlot, chestSlot, legSlot, bootSlot, weaponSlot, offhandSlot; //references to equipment buttons

    private void Start()
    {
        inventory = Inventory.instance; //set inventory as instance of inventory class
        int numberOfSlots = System.Enum.GetNames(typeof(EquipSlot)).Length; //get length of Enumerator in Equipment class to know how many slots of equipment there should be
        currentEquipment = new Equipment[numberOfSlots]; //set length of equipment array to amount of equippable items there are

        //give player weapons now from SelectedWeapon script

        sW.StartWeapon(); //give player starting weapon
    }

    public void Equip (Equipment newItem)
    {
        int slotIndex = (int)newItem.equipSlot; //get equipment slot index and cast it to integer (e.g helmet is equal to 0)

        Equipment equippedItem = null; //variable for storing gear that is already equipped and is going to be replaced by new equipment

        if (currentEquipment[slotIndex] != null)
        {
            equippedItem = currentEquipment[slotIndex]; //get currently equipped item
            inventory.AddItem(equippedItem); //add previously equipped item back to inventory
        }

        if(onEquipmentChanged != null) //if delegate is not null
        {
            onEquipmentChanged.Invoke(newItem, equippedItem); //invoke the delegate
        }

        currentEquipment[slotIndex] = newItem; //set specific equipment slot to the new item

        //put item in equipment slot here - TURN INTO FOR LOOP

        if(slotIndex == 0) //if slot index is 0 (helmet index)
        {
            headSlot.GetComponent<EquipmentSlot>().AddEquipment(newItem); //equip the piece of equipment to correct slot
        }
        if (slotIndex == 1) //if slot index is 1 (chestplate index)
        {
            chestSlot.GetComponent<EquipmentSlot>().AddEquipment(newItem); //equip the piece of equipment to correct slot
        }
        if (slotIndex == 2) //if slot index is 2 (leggings index)
        {
            legSlot.GetComponent<EquipmentSlot>().AddEquipment(newItem); //equip the piece of equipment to correct slot
        }
        if (slotIndex == 3) //if slot index is 3 (boots index)
        {
            bootSlot.GetComponent<EquipmentSlot>().AddEquipment(newItem); //equip the piece of equipment to correct slot
        }
        if (slotIndex == 4) //if slot index is 4 (weapon index)
        {
            weaponSlot.GetComponent<EquipmentSlot>().AddEquipment(newItem); //equip the piece of equipment to correct slot
        }
        if (slotIndex == 5) //if slot index is 5 (offhand index)
        {
            offhandSlot.GetComponent<EquipmentSlot>().AddEquipment(newItem); //equip the piece of equipment to correct slot
        }
    }

    public void Unequip(int slotIndex)
    {
        if (currentEquipment[slotIndex] != null) //if an item exists in current equipment slot
        {
            Equipment equippedItem = currentEquipment[slotIndex]; //get item that is currently equipped in that slot
            inventory.AddItem(equippedItem); //add the item back to the inventory

            sW.UnequipWeapon(); //delete weapon prefab

            currentEquipment[slotIndex] = null; //set to null as no item is currently equipped in the slot

            if (onEquipmentChanged != null) //if delegate is not null
            {
                onEquipmentChanged.Invoke(null, equippedItem); //invoke the delegate
            }
        }
    }
}
