using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")] //menu in unity to create new equipment easily
public class Equipment : Item
{
    public EquipSlot equipSlot; //create new enum so gear slot can be identified

    public int armourModifier; //value armour will be increased by
    public int damageModifier; //value damage will be increased by

    public override void Use()
    {
        base.Use(); //call the base class for the use function
        EquipmentManager.instance.Equip(this); //pass this item into the equip function
        RemoveFromInventory(); //remove item from inventory when equipped
    }
}

public enum EquipSlot { Head, Chest, Legs, Boots, Weapon, Offhand} //new enum to let gear have a specific equip slot