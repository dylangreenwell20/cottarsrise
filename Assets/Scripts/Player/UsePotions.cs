using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsePotions : MonoBehaviour
{
    private PlayerHealth playerHealth; //reference to player health script
    private PlayerMana playerMana; //reference to player mana script

    public PotionUI potionUI; //reference to potion UI script

    public int healthAmount = 50; //amount health potions heal
    public int manaAmount = 75; //amount mana potions heal

    private void Start()
    {
        playerHealth = this.GetComponent<PlayerHealth>(); //get player health
        playerMana = this.GetComponent<PlayerMana>(); //get player mana
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.H)) //if H pressed
        {
            if(Inventory.instance.healthPotCount > 0) //if player has more than 1 health pot
            {
                playerHealth.HealPlayer(healthAmount); //heal player's health for health potion amount
                Inventory.instance.healthPotCount--; //decrement amount of health potions
                potionUI.UpdateHealthPotionUI(); //update health potion ui
            }
        }
        if(Input.GetKeyDown(KeyCode.B)) //if B pressed
        {
            if(Inventory.instance.manaPotCount > 0) //if player has more than 1 mana pot
            {
                playerMana.HealMana(manaAmount); //heal player's mana for mana potion amount
                Inventory.instance.manaPotCount--; //decrement mana pot amount
                potionUI.UpdateManaPotionUI(); //update mana potion ui
            }
        }
    }
}
