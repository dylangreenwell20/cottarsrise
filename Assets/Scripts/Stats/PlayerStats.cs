using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    void Start()
    {
        EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged; //subscribe to callback method
    }

    public void OnEquipmentChanged(Equipment newItem, Equipment currentItem)
    {
        if (newItem != null)
        {
            armour.AddModifier(newItem.armourModifier); //add armour modifier on new equipment
            damage.AddModifier(newItem.damageModifier); //add damage modifier on new equipment
        }

        if(currentItem != null)
        {
            armour.RemoveModifier(currentItem.armourModifier); //remove armour modifier on old equipment
            damage.RemoveModifier(currentItem.damageModifier); //remove damage modifier on old equipment
        }
    }
}
