using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitToMenu : MonoBehaviour
{
    public SelectedWeapon sW;
    public PlayerMovement pM;
    public NormalOrDeadUI uiStatus;

    public void ChangeToMenuScene()
    {
        //stop all music and play main menu music

        AudioManager.Instance.musicSource.Stop();
        AudioManager.Instance.sfxSource.Stop();

        AudioManager.Instance.PlayMusic("MainMenuMusic");

        //reset variables

        sW.weaponFound = false;
        pM.bossButtonPressed = false;
        uiStatus.perkBeingSelected = false;
        uiStatus.onDeadUI = false;

        if (uiStatus.onPauseUI) //if player is on pause ui
        {
            uiStatus.pauseUI.SetActive(false); //hide pause menu
            uiStatus.onPauseUI = false;
            uiStatus.onSettingsUI = false;
        }

        FloorsCompleted.currentFloor = 1; //reset floors completed

        Time.timeScale = 1.0f; //unpause game

        SceneManager.LoadScene(0); //load menu scene
    }
}
