using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentWeaponUI : MonoBehaviour
{
    [SerializeField] private Image weaponIcon; //icon for putting images on

    public Sprite weaponImage; //sprite for weapon image

    public EquipmentManager eM; //reference to equipment manager

    private Equipment currentWeapon; //current weapon the player has equipped

    private void Update()
    {
        //make this only run if player has recently changed weapon

        currentWeapon = eM.currentEquipment[4];

        if(currentWeapon != null) //if player has a weapon equipped
        {
            UpdateIcon(currentWeapon.itemIcon); //update UI with current weapon icon
        }
    }

    public void UpdateIcon(Sprite weaponImage)
    {
        weaponIcon.sprite = weaponImage; //set weaponIcon to weaponImage
    }
}
