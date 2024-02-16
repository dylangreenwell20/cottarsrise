using System;
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
    public EquipmentManager eM; //reference to equipment manager to get weapon slot

    public Equipment startingSword; //reference to starting sword
    public Equipment startingBow; //reference to starting bow
    public Equipment startingStaff; //reference to starting staff

    public bool warriorSelected, archerSelected, mageSelected, isStartWeapon; //bools to check what class is selected and to see if the start weapon is being selected

    public Transform weaponHolder; //reference to weapon holder transform
    public Transform cameraHolder; //reference to camera holder transform

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

    //get weapon from (equipment slot - weapon) and create it using item prefab
    //make it so when a player equips a new weapon, it will call a function in here to delete the old weapon prefab and then spawn the new one - bool hasChangedWeapon?

    //void update for select weapon and use the hasChangedWeapon bool to only run the select weapon script when it is true

    //need bool for if the player has just started so it will know to equip the starting weapon and add it to equipment, otherwise get weapon from equipment

    //UNLESS add starting weapon to equipment in another method in this script THEN run SelectWeapon - that way selectweapon can be kept the same

    //check if player has recently swapped weapon
    //if yes then SelectWeapon();

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
            Debug.Log(isStartWeapon);
            if(isStartWeapon == false) //if it is not the start weapon being equipped
            {
                UnequipWeapon(); //delete previous weapon prefab
            }

            if (warriorSelected) //if warrior class was selected
            {
                //get weapon currently equipped in equipment slot and spawn prefab into player hand

                GameObject swordObject = Instantiate(equippedWeapon.itemPrefab, weaponHolder.position + equippedWeapon.itemPrefab.transform.position, weaponHolder.rotation, weaponHolder) as GameObject; //get weapon prefab and instantiate it as game object
                swordObject.GetComponent<BoxCollider>().enabled = false;
                //swordObject.transform.parent = transform; //set prefab parent to WeaponHolder

                swordActive = true;

                isStartWeapon = false;

                return;
            }
            else if (archerSelected) //if archer class was selected
            {
                //get weapon currently equipped in equipment slot and spawn prefab into player hand

                //GameObject bowObject = Instantiate(equippedWeapon.itemPrefab) as GameObject; //get weapon prefab and instantiate it as game object
                //GameObject bowObject = Instantiate(equippedWeapon.itemPrefab, new Vector3(this.transform.position.x + equippedWeapon.itemPrefab.transform.position.x, this.transform.position.y + equippedWeapon.itemPrefab.transform.position.y, this.transform.position.z + equippedWeapon.itemPrefab.transform.position.z), Quaternion.Euler(this.transform.rotation.x + equippedWeapon.itemPrefab.transform.rotation.x, this.transform.rotation.y + equippedWeapon.itemPrefab.transform.rotation.y, this.transform.rotation.z + equippedWeapon.itemPrefab.transform.rotation.z), this.transform) as GameObject; //get weapon prefab and instantiate it as game object
                //GameObject bowObject = Instantiate(equippedWeapon.itemPrefab, new Vector3(equippedWeapon.itemPrefab.transform.position.x, equippedWeapon.itemPrefab.transform.position.y, equippedWeapon.itemPrefab.transform.position.z), Quaternion.Euler(equippedWeapon.itemPrefab.transform.rotation.x, equippedWeapon.itemPrefab.transform.rotation.y, equippedWeapon.itemPrefab.transform.rotation.z), weaponHolder) as GameObject; //get weapon prefab and instantiate it as game object
                //GameObject bowObject = Instantiate(equippedWeapon.itemPrefab, new Vector3(cameraHolder.position.x + equippedWeapon.itemPrefab.transform.position.x, cameraHolder.position.y + equippedWeapon.itemPrefab.transform.position.y, cameraHolder.position.z + equippedWeapon.itemPrefab.transform.position.z), Quaternion.Euler(cameraHolder.rotation.x + equippedWeapon.itemPrefab.transform.rotation.x, cameraHolder.rotation.y + equippedWeapon.itemPrefab.transform.rotation.y, cameraHolder.rotation.z + equippedWeapon.itemPrefab.transform.rotation.z), weaponHolder) as GameObject; //get weapon prefab and instantiate it as game object
                GameObject bowObject = Instantiate(equippedWeapon.itemPrefab, weaponHolder.position + equippedWeapon.itemPrefab.transform.position, weaponHolder.rotation, weaponHolder) as GameObject; //get weapon prefab and instantiate it as game object
                bowObject.GetComponent<BoxCollider>().enabled = false;
                //bowObject.transform.parent = transform; //set prefab parent to WeaponHolder

                bowActive = true;

                isStartWeapon = false;

                return;
            }
            else if (mageSelected) //if mage class was selected
            {
                //get weapon currently equipped in equipment slot and spawn prefab into player hand

                //GameObject staffObject = Instantiate(equippedWeapon.itemPrefab) as GameObject; //get weapon prefab and instantiate it as game object
                //GameObject staffObject = Instantiate(equippedWeapon.itemPrefab, new Vector3(this.transform.position.x + equippedWeapon.itemPrefab.transform.position.x, this.transform.position.y + equippedWeapon.itemPrefab.transform.position.y, this.transform.position.z + equippedWeapon.itemPrefab.transform.position.z), Quaternion.Euler(this.transform.rotation.x + equippedWeapon.itemPrefab.transform.rotation.x, this.transform.rotation.y + equippedWeapon.itemPrefab.transform.rotation.y, this.transform.rotation.z + equippedWeapon.itemPrefab.transform.rotation.z), this.transform) as GameObject; //get weapon prefab and instantiate it as game object
                //GameObject staffObject = Instantiate(equippedWeapon.itemPrefab, new Vector3(equippedWeapon.itemPrefab.transform.position.x, equippedWeapon.itemPrefab.transform.position.y, equippedWeapon.itemPrefab.transform.position.z), Quaternion.Euler(equippedWeapon.itemPrefab.transform.rotation.x, equippedWeapon.itemPrefab.transform.rotation.y, equippedWeapon.itemPrefab.transform.rotation.z), weaponHolder) as GameObject; //get weapon prefab and instantiate it as game object
                //GameObject staffObject = Instantiate(equippedWeapon.itemPrefab, new Vector3(cameraHolder.position.x + equippedWeapon.itemPrefab.transform.position.x, cameraHolder.position.y + equippedWeapon.itemPrefab.transform.position.y, cameraHolder.position.z + equippedWeapon.itemPrefab.transform.position.z), Quaternion.Euler(cameraHolder.rotation.x + equippedWeapon.itemPrefab.transform.rotation.x, cameraHolder.rotation.y + equippedWeapon.itemPrefab.transform.rotation.y, cameraHolder.rotation.z + equippedWeapon.itemPrefab.transform.rotation.z), weaponHolder) as GameObject; //get weapon prefab and instantiate it as game object
                GameObject staffObject = Instantiate(equippedWeapon.itemPrefab, weaponHolder.position + equippedWeapon.itemPrefab.transform.position, weaponHolder.rotation, weaponHolder) as GameObject; //get weapon prefab and instantiate it as game object
                staffObject.GetComponent<BoxCollider>().enabled = false;
                //staffObject.transform.parent = transform; //set prefab parent to WeaponHolder

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
        //find child then delete the game object

        GameObject previousWeapon = this.gameObject.transform.GetChild(0).gameObject;
        Destroy(previousWeapon);
        weaponFound = false;
    }
}
