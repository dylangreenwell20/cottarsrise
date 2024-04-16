using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public GameObject pauseUI, mainUI, menuContainer, settingsContainer; //different ui to show/hide

    public NormalOrDeadUI uiStatus; //check if player is on victory ui

    public VolumeController volumeController;

    private void Start()
    {
        volumeController.UpdateSliderPositions();

        menuContainer.SetActive(true);
        settingsContainer.SetActive(false);
        pauseUI.SetActive(false);
        
        uiStatus.onPauseUI = false;
        uiStatus.onSettingsUI = false;
    }

    //if inv open, do not open pause menu
    //if player dead, do not open pause menu
    //if player on victory ui, do not open pause menu

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //if pause button pressed
        {
            if (uiStatus.onPauseUI) //if on pause ui already
            {
                ResumeGame();
            }
            else 
            {
                if (Inventory.instance.inventoryOpen == false && uiStatus.onDeadUI == false && uiStatus.perkBeingSelected == false)//if inv is not open, not on dead ui and not on perk ui
                {
                    //update slider positions

                    volumeController.UpdateSliderPositions();

                    //open menu

                    uiStatus.onPauseUI = true;

                    pauseUI.SetActive(true);
                    mainUI.SetActive(false);

                    //unlock camera/cursor

                    uiStatus.playerCamera.GetComponent<PlayerCamera>().enabled = false;
                    uiStatus.playerCamera.GetComponent<PlayerCamera>().UnlockCursor();

                    //pause music/sfx

                    AudioManager.Instance.musicSource.Pause();
                    AudioManager.Instance.sfxSource.Pause();

                    //set timescale to 0 to pause game

                    PauseOrUnpause();
                }
            }
        }
    }

    public void PauseOrUnpause() //either set timescale to 0 to pause  game or set it to 1 to unpause game
    {
        if (Time.timeScale == 1.0f) //if game is unpaused
        {
            Time.timeScale = 0.0f; //pause
            return;
        }

        if(Time.timeScale == 0.0f) //if game is paused
        {
            Time.timeScale = 1.0f; //unpause
            return;
        }
    }

    public void ResumeGame()
    {
        //close menu

        uiStatus.onPauseUI = false;
        uiStatus.onSettingsUI = false;

        settingsContainer.SetActive(false);
        menuContainer.SetActive(true);
        pauseUI.SetActive(false);
        
        mainUI.SetActive(true);

        //play music/sfx

        AudioManager.Instance.musicSource.Play();
        AudioManager.Instance.sfxSource.Play();

        //set timescale to 1 to unpause game

        PauseOrUnpause();

        uiStatus.playerCamera.GetComponent<PlayerCamera>().enabled = true;
        uiStatus.playerCamera.GetComponent<PlayerCamera>().LockCursor();
    }

    public void SettingsMenu() //go to settings menu from pause menu
    {
        uiStatus.onSettingsUI = true;

        settingsContainer.SetActive(true);
        menuContainer.SetActive(false);
    }

    public void BackToPauseMenu() //go back to pause menu from settings menu
    {
        uiStatus.onSettingsUI = false;

        settingsContainer.SetActive(false);
        menuContainer.SetActive(true);
    }
}
