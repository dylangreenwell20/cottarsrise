using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedWeapon : MonoBehaviour
{
    public int selectedWeapon = 0; //starting weapon is 0 (sword = 0, bow = 1, staff = 2)
    public bool swordActive; //if sword is currently equipped
    public bool bowActive; //if bow is currently equipped
    public bool staffActive; //if staff is currently equipped

    public WeaponController wC; //link to WeaponController script to know whether the player is currently attacking or not - the player cannot swap weapon while on attack cooldown

    private void Start()
    {
        SelectWeapon(); //function to select weapon
    }

    private void Update()
    {
        int previousSelectedWeapon = selectedWeapon; //temporary variable for previously selected weapon
        

        /*
        if(Input.GetAxis("Mouse ScrollWheel") < 0f) //if user has scrolled down
        {
            if(selectedWeapon >= transform.childCount - 1) //if selectedWeapon is greater than the amount of children in the WeaponHolder
            {
                selectedWeapon = 0; //select first weapon
            }
            else
            {
                selectedWeapon++; //increment selectedWeapon
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) //if user has scrolled up
        {
            if (selectedWeapon <= 0) //if selectedWeapon is less than or equal to 0
            {
                selectedWeapon = transform.childCount - 1; //set selectedWeapon to child count of WeaponHolder - 1
            }
            else
            {
                selectedWeapon--; //decrement selectedWeapon
            }
        }

        CODE FOR SCROLL WHEEL SWITCHING

        */

        if (Input.GetKeyDown(KeyCode.Alpha1) && wC.IsAttacking == false) //if 1 pressed
        {
            selectedWeapon = 0; //select sword
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && wC.IsAttacking == false && transform.childCount >= 2) //if 2 pressed
        {
            selectedWeapon = 1; //bow selected
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && wC.IsAttacking == false && transform.childCount >= 3) //if 3 pressed
        {
            selectedWeapon = 2; //staff selected
        }

        if (previousSelectedWeapon != selectedWeapon) //if previous weapon is not the same as current weapon
        {
            SelectWeapon(); //run code to select new weapon
        }
    }

    void SelectWeapon()
    {
        int i = 0; //used for going through the for loop
        foreach (Transform weapon in transform) //for each weapon in the weapon holder
        {
            if(i == selectedWeapon) //if i is equal to selectedWeapon
            {
                weapon.gameObject.SetActive(true); //set that weapon to active
                if(selectedWeapon == 0)
                {
                    swordActive = true;
                    bowActive = false;
                    staffActive = false;
                }

                else if (selectedWeapon == 1)
                {
                    swordActive = false;
                    bowActive = true;
                    staffActive = false;
                }

                else if (selectedWeapon == 2)
                {
                    swordActive = false;
                    bowActive = false;
                    staffActive = true;
                }
            }
            else
            {
                weapon.gameObject.SetActive(false); //set that weapon to not active
            }
            i++; //increment i
        }
    }
}
