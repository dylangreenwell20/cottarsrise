using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Inventory/Consumable")] //menu in unity to create new consumables easily
public class Consumable : Item
{
    public ConsumableType consumableType; //create new enum for consumable type

    public int consumableValue; //value which the consumable gives (e.g 50 with a ConsumableType of Health would mean it gives the player 50 health)

    private GameObject player; //reference to player game object
    private PlayerHealth playerHealth; //reference to player health script
    private PlayerMana playerMana; //reference to player mana script
    private ArrowCounter arrowCounter; //reference to arrow counter script

    public override void Use()
    {
        base.Use(); //run base Use() function

        player = GameObject.Find("PlayerObject"); //find player game object

        if (this.consumableType == ConsumableType.Health) //if consumable type is health
        {
            //Debug.Log("heal"); //for testing
            playerHealth = player.GetComponent<PlayerHealth>(); //get playerHealth component
            playerHealth.HealPlayer(consumableValue); //heal player's health for consumable value
        }

        if(this.consumableType == ConsumableType.Mana) //if consumable type is mana
        {
            //Debug.Log("mana"); //for testing
            playerMana = player.GetComponent<PlayerMana>(); //get playerMana component
            playerMana.HealMana(consumableValue); //heal player's mana for consumable value
        }

        if(this.consumableType == ConsumableType.Arrow) //if consumable type is arrow
        {
            //REMOVE THIS IN THE FUTURE AS I WILL MAKE IT SO ARROWS ARE ADDED WHEN THEY ARE PICKED UP

            //Debug.Log("arrow"); //for testing
            arrowCounter = player.GetComponent<ArrowCounter>(); //get ArrowCounter component
            arrowCounter.GainArrow(consumableValue); //give player arrows equal to consumable value
        }

        RemoveFromInventory(); //remove the item from the inventory
    }
}

public enum ConsumableType { Health, Mana, Arrow } //new enum to let consumables change specific things (e.g health or mana)