using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NormalOrDeadUI : MonoBehaviour
{
    public GameObject player;
    public GameObject normalUI;
    public GameObject victoryUI;
    public GameObject deadUI;
    public GameObject pauseUI;

    public TextMeshProUGUI floorsCompletedText;

    public GameObject playerCamera;

    public GeneratePerks perks;

    public bool perkBeingSelected; //if a perk is being selected so weapon attacks can be cancelled
    public bool onDeadUI; //if dead ui is active
    public bool onPauseUI; //if pause ui is active
    public bool onSettingsUI; //if settings ui is active

    private void Start()
    {
        //show normal ui and hide other ui on game start

        perkBeingSelected = false;
        onDeadUI = false;
        onPauseUI = false;

        NormalUI();
    }

    public void NormalUI() //enable normal ui
    {
        perkBeingSelected = false;
        onDeadUI = false;
        onPauseUI = false;
        onSettingsUI = false;

        normalUI.SetActive(true);
        deadUI.SetActive(false);
        victoryUI.SetActive(false);

        //lock cursor and allow camera movement

        playerCamera.GetComponent<PlayerCamera>().enabled = true;
        playerCamera.GetComponent<PlayerCamera>().LockCursor();
    }

    public void DeadUI() //enable dead ui
    {
        onDeadUI = true;
        onPauseUI = false;

        normalUI.SetActive(false);
        deadUI.SetActive(true);
        victoryUI.SetActive(false);
        onSettingsUI = false;

        //update completed floor text

        floorsCompletedText.text = "Floors Completed: " + (FloorsCompleted.currentFloor - 1);

        //play fail sound effect

        AudioManager.Instance.musicSource.Stop(); //stop current music
        AudioManager.Instance.sfxSource.Stop(); //stop current sfx
        AudioManager.Instance.PlaySFX("Failure", AudioManager.Instance.sfxSource); //play failure sfx

        //unlock cursor and stop camera movement

        playerCamera.GetComponent<PlayerCamera>().enabled = false;
        playerCamera.GetComponent<PlayerCamera>().UnlockCursor();
    }

    public void VictoryUI() //enable victory ui
    {
        //stop music and play victory jingle

        AudioManager.Instance.musicSource.Stop(); //stop current music
        AudioManager.Instance.PlaySFX("VictoryJingle", AudioManager.Instance.sfxSource); //play victory sfx

        onDeadUI = false;
        perkBeingSelected = true;
        onPauseUI = false;
        onSettingsUI = false;

        perks.Generate3Perks(); //generate 3 perks

        normalUI.SetActive(false);
        deadUI.SetActive(false);
        victoryUI.SetActive(true);

        perks.continueButton.interactable = false;

        //unlock cursor and stop camera movement

        playerCamera.GetComponent<PlayerCamera>().enabled = false;
        playerCamera.GetComponent<PlayerCamera>().UnlockCursor();
    }
}
