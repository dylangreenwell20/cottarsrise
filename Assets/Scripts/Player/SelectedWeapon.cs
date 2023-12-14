using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedWeapon : MonoBehaviour
{
    public bool swordActive; //if sword is currently equipped
    public bool bowActive; //if bow is currently equipped
    public bool staffActive; //if staff is currently equipped

    public WeaponController wC; //link to WeaponController script to know whether the player is currently attacking or not - the player cannot swap weapon while on attack cooldown

    public GameObject swordPrefab; //reference to sword prefab
    public GameObject bowPrefab; //reference to bow prefab
    public GameObject staffPrefab; //reference to staff prefab


    private void Start()
    {
        if(StartingWeapon.warriorClassSelected == false)
        {
            if(StartingWeapon.archerClassSelected == false)
            {
                if(StartingWeapon.mageClassSelected == false)
                {
                    StartingWeapon.warriorClassSelected = true;
                }
            }
        }

        SelectWeapon(); //give player selected weapon
    }

    /*

    public int selectedWeapon = 0; //starting weapon is 0 (sword = 0, bow = 1, staff = 2)


    private void Start()
    {
        SelectWeapon(); //function to select weapon
    }

    private void Update()
    {
        
        int previousSelectedWeapon = selectedWeapon; //temporary variable for previously selected weapon

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
    */

    public void SelectWeapon()
    {
        bool warriorSelected = StartingWeapon.warriorClassSelected; //get value of warriorClassSelected
        bool archerSelected = StartingWeapon.archerClassSelected; //get value of archerClassSelected
        bool mageSelected = StartingWeapon.mageClassSelected; //get value of mageClassSelected

        if (warriorSelected) //if warrior class was selected
        {
            //spawn sword into player hand and inventory

            GameObject swordObject = Instantiate(swordPrefab) as GameObject; //instantiate new sword prefab
            swordObject.transform.parent = transform; //set prefab parent to WeaponHolder

            swordActive = true;

            //add to inv/equipment here

            return;
        }
        else if (archerSelected) //if archer class was selected
        {
            //spawn bow into player hand and inventory

            GameObject bowObject = Instantiate(bowPrefab) as GameObject; //instantiate new bow prefab
            bowObject.transform.parent = transform; //set prefab parent to WeaponHolder

            bowActive = true;

            //add to inv/equipment here

            return;
        }
        else if (mageSelected) //if mage class was selected
        {
            //spawn staff into player hand and inventory

            GameObject staffObject = Instantiate(staffPrefab) as GameObject; //instantiate new staff prefab
            staffObject.transform.parent = transform; //set prefab parent to WeaponHolder

            staffActive = true;

            //add to inv/equipment here

            return;
        }

        //CALL THIS FUNCTION IN MAIN MENU WHEN THE GAME IS STARTED
        //CALL THIS FUNCTION IN MAIN MENU WHEN THE GAME IS STARTED
        //CALL THIS FUNCTION IN MAIN MENU WHEN THE GAME IS STARTED


        /*
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
        */
    }
}
