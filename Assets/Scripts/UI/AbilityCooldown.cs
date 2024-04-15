using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCooldown : MonoBehaviour
{
    public Image abilityIcon; //icon for putting ability image on
    public Image abilityBackground; //background of ability ui
    public Sprite abilitySprite; //ability image (sprite)
    public AbilityHolder abilityHolder; //reference to ability holder to see what current ability is

    private void Start()
    {
        UpdateIcon(); //update ability UI
        RechargedAbilityUI(); //ability is ready to use
    }

    public void UpdateIcon()
    {
        Ability currentAbility;

        if(SelectedAbility.chosenAbility != null)
        {
            currentAbility = SelectedAbility.chosenAbility;
        }
        else
        {
            currentAbility = abilityHolder.ability; //get current ability
        }
        
        abilitySprite = currentAbility.abilitySprite; //get sprite of ability
        abilityIcon.sprite = abilitySprite; //set ability sprite as UI icon
    }

    public void UsedAbilityUI() //when ability is used, turn background of icon red
    {
        abilityBackground.color = Color.red;
    }

    public void RechargedAbilityUI() //when ability is recharged, turn background of icon green
    {
        abilityBackground.color = Color.green;
    }
}
