using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu; //reference to main menu
    public GameObject settingsMenu; //reference to settings menu
    public GameObject classMenu; //reference to class menu
    public GameObject abilityMenu;
    public GameObject perkMenu;
    public GameObject controlsMenu;

    public SelectedClass selectedClass; //reference to SelectedClass script
    public SelectAbility selectedAbility;
    public GeneratePerks selectedPerks;

    private void Awake()
    {
        mainMenu.SetActive(true); //enable main menu
        settingsMenu.SetActive(false); //disable settings menu
        classMenu.SetActive(false); //disable class select menu
        abilityMenu.SetActive(false);
        perkMenu.SetActive(false);
        controlsMenu.SetActive(false);
    }

    public void PlayGame()
    {
        selectedClass.UpdateStaticVariables(); //update static variables so game scene can know what class was picked in menu scene
        selectedAbility.UpdateStaticAbility();
        selectedPerks.UpdateStaticPerk();

        FloorsCompleted.currentFloor = 1; //set to floor 1

        SceneManager.LoadScene("GameScene"); //load next scene
    }

    public void QuitGame()
    {
        Debug.Log("Exiting game..."); //for testing
        Application.Quit(); //quit application
    }

    public void LoadClassSelect()
    {
        mainMenu.SetActive(false); //disable main menu
        classMenu.SetActive(true); //enable class select menu
        settingsMenu.SetActive(false);
        abilityMenu.SetActive(false);
        perkMenu.SetActive(false);
        controlsMenu.SetActive(false);
    }

    public void LoadSettings()
    {
        AudioManager.Instance.volumeController.UpdateSliderPositions();

        mainMenu.SetActive(false); //disable main menu
        settingsMenu.SetActive(true); //enable settings menu
        classMenu.SetActive(false);
        abilityMenu.SetActive(false);
        perkMenu.SetActive(false);
        controlsMenu.SetActive(false);
    }

    public void LoadMain()
    {
        mainMenu.SetActive(true); //enable main menu
        settingsMenu.SetActive(false); //disable settings menu
        classMenu.SetActive(false); //disable class menu
        abilityMenu.SetActive(false);
        perkMenu.SetActive(false);
        controlsMenu.SetActive(false);
    }

    public void LoadAbilitySelect()
    {
        mainMenu.SetActive(false); //disable main menu
        settingsMenu.SetActive(false); //disable settings menu
        classMenu.SetActive(false); //disable class menu
        abilityMenu.SetActive(true);
        perkMenu.SetActive(false);
        controlsMenu.SetActive(false);
    }

    public void LoadPerkSelect()
    {
        selectedPerks.Generate3Perks();

        mainMenu.SetActive(false); //disable main menu
        settingsMenu.SetActive(false); //disable settings menu
        classMenu.SetActive(false); //disable class menu
        abilityMenu.SetActive(false);
        perkMenu.SetActive(true);
        controlsMenu.SetActive(false);
    }

    public void LoadControls()
    {
        mainMenu.SetActive(false); //disable main menu
        settingsMenu.SetActive(false); //disable settings menu
        classMenu.SetActive(false); //disable class menu
        abilityMenu.SetActive(false);
        perkMenu.SetActive(false);
        controlsMenu.SetActive(true);
    }
}
