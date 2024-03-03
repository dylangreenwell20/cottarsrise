using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")] //menu in unity to create new equipment easily
public class Equipment : Item
{
    public EquipSlot equipSlot; //create new enum so gear slot can be identified

    public int armourModifier; //value armour will be increased by
    public int damage; //damage value of weapons
    public int damageModifier; //value damage will be increased by

    public bool isWeapon; //if equipment is a weapon
    public bool isOffhand; //if equipment is an offhand
    public bool is2Handed; //if weapon is two handed so an offhand cannot be equipped at the same time

    public WeaponType weaponType; //create new enum so weapons can have a specified type

    public override void Use()
    {
        base.Use(); //call the base class for the use function
        EquipmentManager.instance.Equip(this); //pass this item into the equip function
        RemoveFromInventory(); //remove item from inventory when equipped
    }
}

public enum EquipSlot { Head, Chest, Legs, Boots, Weapon, Offhand} //new enum to let gear have a specific equip slot

public enum WeaponType { Null, Melee, Range, Mage} //new enum to let weapons have a specific weapon type