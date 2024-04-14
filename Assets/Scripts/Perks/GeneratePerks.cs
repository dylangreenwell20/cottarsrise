using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GeneratePerks : MonoBehaviour
{
    public List<Perk> perkList = new List<Perk>(); //list of perks in the game

    public List<Perk> generatedPerks = new List<Perk>(); //list of generated perks

    public Button[] perkButtons = new Button[3]; //array of perk buttons

    public GameObject[] buttonBackgrounds = new GameObject[3]; //array of backgrounds of buttons

    public Button continueButton; //button to continue to next dungeon floor
    
    public TextMeshProUGUI[] perkDescriptions = new TextMeshProUGUI[3]; //array of perk descriptions

    public bool perk1, perk2, perk3; //which perk has been selected

    public Color selectedColour; //reference to colour for selected button
    public Color defaultColour; //reference to colour for non-selected button

    private void Start()
    {
        continueButton.interactable = false;
    }

    public void UpdatePerksUI() //update perk ui (buttons and descriptions)
    {
        for(int i = 0; i < 3; i++)
        {
            //change button image

            perkButtons[i].image.sprite = generatedPerks[i].perkSprite;

            //change description text

            perkDescriptions[i].text = generatedPerks[i].perkDescription;
        }
    }

    public void Generate3Perks() //generate 3 perks randomly from the perk list
    {
        List<Perk> duplicatePerkList = new List<Perk>(perkList);

        generatedPerks = new List<Perk>(); //reset list of generated perks

        for (int i = 0; i < 3; i++) //pick 3 perks and add to generatedPerks list
        {
            int random = Random.Range(0, duplicatePerkList.Count); //pick random perk

            generatedPerks.Add(duplicatePerkList[random]); //add perk to output list

            duplicatePerkList.RemoveAt(random); //remove perk from duplicate list
        }

        UpdatePerksUI();
    }

    public void Perk1Selected() //first perk selected
    {
        perk1 = true;
        perk2 = false;
        perk3 = false;

        continueButton.interactable = true;

        ColourPerkButtons();
    }

    public void Perk2Selected() //second perk selected
    {
        perk1 = false;
        perk2 = true;
        perk3 = false;

        continueButton.interactable = true;

        ColourPerkButtons();
    }

    public void Perk3Selected() //third perk selected
    {
        perk1 = false;
        perk2 = false;
        perk3 = true;

        continueButton.interactable = true;

        ColourPerkButtons();
    }

    public void UpdateStaticPerk() //update static variable to know what perk has been selected
    {
        if (perk1)
        {
            PerkChanges.chosenPerk = generatedPerks[0];
        }
        else if (perk2)
        {
            PerkChanges.chosenPerk = generatedPerks[1];
        }
        else if (perk3)
        {
            PerkChanges.chosenPerk = generatedPerks[2];
        }
    }

    public void ColourPerkButtons() //change outline colour of perk to indicate it has been selected
    {
        if (perk1) //if perk 1 selected
        {
            buttonBackgrounds[0].GetComponent<Outline>().effectColor = selectedColour; //set outline of button to a specific colour
        }
        else //else if perk 1 not selected
        {
            buttonBackgrounds[0].GetComponent<Outline>().effectColor = defaultColour; //set outline of button to a specific colour
        }

        if (perk2) //if perk 2 selected
        {
            buttonBackgrounds[1].GetComponent<Outline>().effectColor = selectedColour; //set outline of button to a specific colour
        }
        else //else if perk 2 not selected
        {
            buttonBackgrounds[1].GetComponent<Outline>().effectColor = defaultColour; //set outline of button to a specific colour
        }

        if (perk3) //if perk 3 selected
        {
            buttonBackgrounds[2].GetComponent<Outline>().effectColor = selectedColour; //set outline of button to a specific colour
        }
        else //else if perk 3 not selected
        {
            buttonBackgrounds[2].GetComponent<Outline>().effectColor = defaultColour; //set outline of button to a specific colour
        }
    }
}
