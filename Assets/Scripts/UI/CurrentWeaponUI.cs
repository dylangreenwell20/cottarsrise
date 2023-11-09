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
        UpdateIcon(swordImage); //default starting weapon is the sword
    }

    private void Update()
    {
        if (swordModel.activeInHierarchy) //if sword model and parents are active
        {
            UpdateIcon(swordImage); //make UI show that sword is currently equipped
        }

        if (bowModel.activeInHierarchy) //if bow model and parents are active
        {
            UpdateIcon(bowImage); //make UI show that bow is currently equipped
        }

        if (staffModel.activeInHierarchy) //if staff model and parents are active
        {
            UpdateIcon(staffImage); //make UI show that staff is currently equipped
        }
    }

    public void UpdateIcon(Sprite weaponImage)
    {
        weaponIcon.sprite = weaponImage; //set weaponIcon to weaponImage
    }
}
