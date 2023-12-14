using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedClass : MonoBehaviour
{
    public Button warriorButton; //reference to warrior button
    public Button archerButton; //reference to archer button
    public Button mageButton; //reference to mage button
    public Button playButton; //reference to play button

    public Color selectedColour; //reference to colour for selected button
    public Color defaultColour; //reference to colour for non-selected button

    public GameObject mainMenu; //reference to main menu
    public GameObject classMenu; //reference to class menu

    public bool warriorSelected; //if warrior has been selected or not
    public bool archerSelected; //if archer has been selected or not
    public bool mageSelected; //if mage has been selected or not

    public bool classSelected; //if a class has been selected or not

    private void Awake()
    {
        classSelected = false; //no class selected by default
    }

    private void Update()
    {
        if (classSelected) //if a class has been selected
        {
            playButton.gameObject.SetActive(true); //show play button
        }
        else //else if no class has been selected
        {
            playButton.gameObject.SetActive(false); //hide play button
        }
    }

    public void SelectWarrior() //select warrior and deselect other classes
    {
        //set warrior as selected and deselect other classes
        warriorSelected = true;
        archerSelected = false;
        mageSelected = false;

        ColourButtons(); //update colour of button outlines

        classSelected = true; //a class has been selected
    }

    public void SelectArcher() //select archer and deselect other classes
    {
        //set archer as selected and deselect other classes
        warriorSelected = false;
        archerSelected = true;
        mageSelected = false;

        ColourButtons(); //update colour of button outlines

        classSelected = true; //a class has been selected
    }

    public void SelectMage() //select mage and deselect other classes
    {
        //set mage as selected and deselect other classes
        warriorSelected = false;
        archerSelected = false;
        mageSelected = true;

        ColourButtons(); //update colour of button outlines

        classSelected = true; //a class has been selected
    }

    public void BackToMenu()
    {
        classMenu.SetActive(false); //hide class menu
        mainMenu.SetActive(true); //show main menu

        //deselect all classes
        warriorSelected = false;
        archerSelected = false;
        mageSelected = false;

        classSelected = false; //no class is selected

        ColourButtons(); //colour buttons back to default colour
    }

    public void ColourButtons()
    {
        if (warriorSelected) //if warrior selected
        {
            warriorButton.GetComponent<Outline>().effectColor = selectedColour; //set outline of button to a specific colour
        }
        else //else if warrior not selected
        {
            warriorButton.GetComponent<Outline>().effectColor = defaultColour; //set outline of button to a specific colour
        }
        
        if (archerSelected) //if archer selected
        {
            archerButton.GetComponent<Outline>().effectColor = selectedColour; //set outline of button to a specific colour
        }
        else //else if archer not selected
        {
            archerButton.GetComponent<Outline>().effectColor = defaultColour; //set outline of button to a specific colour
        }

        if (mageSelected) //if mage selected
        {
            mageButton.GetComponent<Outline>().effectColor = selectedColour; //set outline of button to a specific colour
        }
        else //else if mage not selected
        {
            mageButton.GetComponent<Outline>().effectColor = defaultColour; //set outline of button to a specific colour
        }
    }

    public void UpdateStaticVariables()
    {
        StartingWeapon.warriorClassSelected = warriorSelected; //update warriorClassSelected variable
        StartingWeapon.archerClassSelected = archerSelected; //update archerClassSelected variable
        StartingWeapon.mageClassSelected = mageSelected; //update mageClassSelected variable
    }
}
