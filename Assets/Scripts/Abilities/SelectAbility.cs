using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectAbility : MonoBehaviour
{
    public List<Ability> abilityList = new List<Ability>(); //list of abilities in the game

    public Ability selectedAbility; //ability the player selects

    public Button[] abilityButtons = new Button[2]; //buttons to display ability sprites on

    public GameObject[] buttonBackgrounds = new GameObject[2]; //button backgrounds to outline to show selection

    public TextMeshProUGUI[] abilityDescriptions = new TextMeshProUGUI[2]; //ability descriptions

    public Button continueButton; //button to continue to perk select

    public bool ability1, ability2; //which ability was selected

    public Color selectedColour; //selected button colour
    public Color defaultColour; //default button colour

    private void Start()
    {
        continueButton.interactable = false;

        UpdateAbilityUI();
    }

    public void UpdateAbilityUI()
    {
        for(int i = 0; i < 2; i++)
        {
            //change button image

            abilityButtons[i].image.sprite = abilityList[i].abilitySprite;

            //change description text

            abilityDescriptions[i].text = abilityList[i].abilityDescription;
        }
    }

    public void Ability1Selected() //select first ability
    {
        ability1 = true;
        ability2 = false;

        continueButton.interactable = true;

        ColourAbilityButtons();
    }

    public void Ability2Selected() //select 2nd ability
    {
        ability1 = false;
        ability2 = true;

        continueButton.interactable = true;

        ColourAbilityButtons();
    }

    public void ResetSelectedAbilities() //reset ability selection
    {
        ability1 = false;
        ability2 = false;

        continueButton.interactable = false;

        ColourAbilityButtons();
    }

    public void UpdateStaticAbility() //update chosen ability static variable
    {
        if (ability1)
        {
            SelectedAbility.chosenAbility = abilityList[0];
        }
        else if (ability2)
        {
            SelectedAbility.chosenAbility = abilityList[1];
        }

        ResetSelectedAbilities();
    }

    public void ColourAbilityButtons() //colour outline of buttons depending on what ability has been selected
    {
        if (ability1)
        {
            buttonBackgrounds[0].GetComponent<Outline>().effectColor = selectedColour;
        }
        else
        {
            buttonBackgrounds[0].GetComponent<Outline>().effectColor = defaultColour;
        }

        if (ability2)
        {
            buttonBackgrounds[1].GetComponent<Outline>().effectColor = selectedColour;
        }
        else
        {
            buttonBackgrounds[1].GetComponent<Outline>().effectColor = defaultColour;
        }
    }
}
