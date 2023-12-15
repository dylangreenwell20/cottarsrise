using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
    public Ability ability; //reference to Ability script
    float cooldownTime; //ability cooldown time
    float activeTime; //active time for ability - essentially the use time

    public Transform abilityFirePoint; //reference to ability fire point (here to get easily referenced by abilities)

    public GameObject fireballPrefab; //reference to fireball prefab

    public PlayerStats playerStats; //reference to player stats

    public PlayerMana playerMana; //reference to player mana

    enum AbilityState //to get the state an ability is in
    {
        ready,
        active,
        cooldown
    }

    AbilityState state = AbilityState.ready; //create new ability state and set it to ready

    public KeyCode abilityKey; //key to press to use the ability

    private void Update()
    {
        switch (state) //switch statement for ability states
        {
            case AbilityState.ready: //ready state
                if (Input.GetKeyDown(abilityKey)) //if ability key pressed
                {
                    if (playerMana.currentMana >= ability.manaCost) //if player has more mana than the ability mana cost
                    {
                        ability.Activate(gameObject); //activate ability

                        playerMana.LoseMana(ability.manaCost); //take away mana

                        state = AbilityState.active; //set state to active
                        activeTime = ability.activeTime; //get active time of ability
                    }
                }
            break;

            case AbilityState.active:
                if (activeTime > 0) //if active time greater than 0
                {
                    activeTime -= Time.deltaTime; //start counting down until time reaches 0
                }
                else //else if time is 0
                {
                    state = AbilityState.cooldown; //change to cooldown state
                    cooldownTime = ability.cooldownTime; //get cooldown time from ability
                }
            break;

            case AbilityState.cooldown:
                if (cooldownTime > 0) //if cooldown time greater than 0
                {
                    cooldownTime -= Time.deltaTime; //start counting down until time reaches 0
                }
                else //else if time is 0
                {
                    state = AbilityState.ready; //change to ready state
                }
            break;
        }
    }
}
