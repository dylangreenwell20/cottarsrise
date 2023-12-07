using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSlot : MonoBehaviour
{
    Equipment equipment; //reference to equipment script

    public Image icon; //reference to equipment image

    public void AddEquipment(Equipment newItem)
    {
        equipment = newItem; //set item as the new item
        icon.sprite = equipment.itemIcon; //set item icon to new item's icon
        icon.enabled = true; //set icon to enabled state
    }

    public void ClearItemSlot()
    {
        equipment = null; //set item to null
        icon.sprite = null; //set icon sprite to null
        icon.enabled = false; //disable icon
    }

    public void TakeOffEquipment()
    {
        if (equipment != null) //if item exists
        {
            //remove equipment from Equipment instance/array and add it back to inventory - should be able to call a function from EquipmentManager
        }
    }
}
