using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentWeaponUI : MonoBehaviour
{
    [SerializeField] private Image weaponIcon; //icon for putting images on

    public Sprite swordImage;
    public Sprite bowImage;
    public Sprite staffImage;

    public GameObject swordModel;
    public GameObject bowModel;
    public GameObject staffModel;

    private void Start()
    {
        if(StartingWeapon.warriorClassSelected)
        {
            UpdateIcon(swordImage); //update icon with sword image
            swordModel = GameObject.Find("Sword(Clone)"); //find sword model
            return;
        }

        if (StartingWeapon.archerClassSelected)
        {
            UpdateIcon(bowImage); //update icon with bow image
            bowModel = GameObject.Find("Bow(Clone)"); //find bow model
            return;
        }

        if (StartingWeapon.mageClassSelected)
        {
            UpdateIcon(staffImage); //update icon with staff image
            staffModel = GameObject.Find("Staff(Clone)"); //find staff model
            return;
        }
    }

    private void Update()
    {
        if (StartingWeapon.warriorClassSelected) //if warrior class selected
        {
            if (swordModel.activeInHierarchy) //if sword model and parents are active
            {
                UpdateIcon(swordImage); //make UI show that sword is currently equipped
                return;
            }
        }

        if (StartingWeapon.archerClassSelected) //if archer class selected
        {
            if (bowModel.activeInHierarchy) //if bow model and parents are active
            {
                UpdateIcon(bowImage); //make UI show that bow is currently equipped
                return;
            }
        }

        if (StartingWeapon.mageClassSelected) //if mage class selected
        {
            if (staffModel.activeInHierarchy) //if staff model and parents are active
            {
                UpdateIcon(staffImage); //make UI show that staff is currently equipped
                return;
            }
        }
    }

    public void UpdateIcon(Sprite weaponImage)
    {
        weaponIcon.sprite = weaponImage; //set weaponIcon to weaponImage
    }
}
