using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class SelectedWeapon : MonoBehaviour
{
    public bool swordActive; //if sword is currently equipped
    public bool bowActive; //if bow is currently equipped
    public bool staffActive; //if staff is currently equipped

    public WeaponController wC; //link to WeaponController script to know whether the player is currently attacking or not - the player cannot swap weapon while on attack cooldown
    public EquipmentManager eM; //reference to equipment manager to get weapon slot

    public Equipment startingSword; //reference to starting sword
    public Equipment startingBow; //reference to starting bow
    public Equipment startingStaff; //reference to starting staff

    public bool warriorSelected, archerSelected, mageSelected, isStartWeapon; //bools to check what class is selected and to see if the start weapon is being selected

    public Transform weaponHolder; //reference to weapon holder transform
    public Transform cameraHolder; //reference to camera holder transform

    public Transform meleePosition;
    public Transform rangePosition;
    public Transform magePosition;

    public bool weaponFound; //used by WeaponController to get weapon fire points

    private void Start()
    {
        //start player as warrior if game started from game scene from editor - useful for testing quickly

        warriorSelected = StartingWeapon.warriorClassSelected; //get value of warriorClassSelected
        archerSelected = StartingWeapon.archerClassSelected; //get value of archerClassSelected
        mageSelected = StartingWeapon.mageClassSelected; //get value of mageClassSelected

        isStartWeapon = true; //script will be equipping the player's starting weapon

        if (warriorSelected == false)
        {
            if(archerSelected == false)
            {
                if(mageSelected == false)
                {
                    StartingWeapon.mageClassSelected = true;
                    mageSelected = true;
                }
            }
        }
    }
    public void StartWeapon()
    {
        //based on player class, add specific weapon to player weapon slot and then run SelectWeapon() to equip it

        if(warriorSelected)
        {
            //add starting sword to inventory

            EquipmentManager.instance.Equip(startingSword); //equip starting sword

            //run SelectWeapon to equip it

            SelectWeapon(); //equip weapon
        }
        else if(archerSelected)
        {
            //add starting bow to inventory

            EquipmentManager.instance.Equip(startingBow); //equip starting bow

            //run SelectWeapon to equip it

            SelectWeapon(); //equip weapon
        }
        else if(mageSelected)
        {
            //add starting staff to inventory

            EquipmentManager.instance.Equip(startingStaff); //equip starting staff

            //run SelectWeapon to equip it

            SelectWeapon(); //equip weapon
        }
    }

    //update position of transform and where to spawn it

    public void SelectWeapon()
    {
        Equipment equippedWeapon = eM.currentEquipment[4]; //get current equipment in weapon slot

        if (equippedWeapon != null) //if equipment exists in weapon slot
        {
            //Debug.Log(isStartWeapon);

            if(isStartWeapon == false) //if it is not the start weapon being equipped
            {
                UnequipWeapon(); //delete previous weapon prefab

            }

            if (warriorSelected) //if warrior class was selected
            {
                //get weapon currently equipped in equipment slot and spawn prefab into player hand

                GameObject swordObject = Instantiate(equippedWeapon.itemPrefab, meleePosition.position, meleePosition.rotation, meleePosition);
                swordObject.GetComponent<BoxCollider>().enabled = false;

                swordActive = true;

                isStartWeapon = false;

                return;
            }
            else if (archerSelected) //if archer class was selected
            {
                //get weapon currently equipped in equipment slot and spawn prefab into player hand

                GameObject bowObject = Instantiate(equippedWeapon.itemPrefab, rangePosition.position, rangePosition.rotation, rangePosition);
                bowObject.GetComponent<BoxCollider>().enabled = false;

                //check if 0 arrows - if true then hide model arrow

                if(wC.noArrows == true) //if player has no arrows
                {
                    bowObject.transform.Find("Arrow").gameObject.SetActive(false); //hide model arrow
                }

                bowActive = true;

                isStartWeapon = false;

                return;
            }
            else if (mageSelected) //if mage class was selected
            {
                //get weapon currently equipped in equipment slot and spawn prefab into player hand

                GameObject staffObject = Instantiate(equippedWeapon.itemPrefab, magePosition.position, magePosition.rotation, magePosition); //Quaternion.Euler(0f, 0f, 0f)
                staffObject.GetComponent<BoxCollider>().enabled = false;

                staffActive = true;

                isStartWeapon = false;

                return;
            }
        }
        else
        {
            Debug.Log("null weapon");
        }
    }

    public void UnequipWeapon()
    {
        Equipment currentWeapon = eM.currentEquipment[4]; //get current equipment in weapon slot

        //find child then delete the game object

        if (warriorSelected)
        {
            if(currentWeapon != null)
            {
                GameObject previousWeapon = meleePosition.transform.GetChild(0).gameObject;
                Destroy(previousWeapon);
                weaponFound = false;
            }
        }
        else if (archerSelected)
        {
            if (currentWeapon != null)
            {
                GameObject previousWeapon = rangePosition.transform.GetChild(0).gameObject;
                Destroy(previousWeapon);
                weaponFound = false;
            }
        }
        else if(mageSelected)
        {
            if (currentWeapon != null)
            {
                GameObject previousWeapon = magePosition.transform.GetChild(0).gameObject;
                Destroy(previousWeapon);
                weaponFound = false;
            }
        }

        
    }
}
