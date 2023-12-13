using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu; //reference to main menu
    public GameObject settingsMenu; //reference to settings menu
    public GameObject classMenu; //reference to class menu

    private void Awake()
    {
        mainMenu.SetActive(true); //enable main menu
        settingsMenu.SetActive(false); //disable settings menu
        classMenu.SetActive(false); //disable class select menu
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //load next scene
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
    }

    public void LoadSettings()
    {
        mainMenu.SetActive(false); //disable main menu
        settingsMenu.SetActive(true); //enable settings menu
    }

    public void LoadMain()
    {
        mainMenu.SetActive(true); //enable main menu
        settingsMenu.SetActive(false); //disable settings menu
        classMenu.SetActive(false); //disable class menu
    }
}
