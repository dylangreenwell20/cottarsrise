using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NormalOrDeadUI : MonoBehaviour
{
    public GameObject player;
    public GameObject normalUI;
    public GameObject victoryUI;
    public GameObject deadUI;

    public GameObject playerCamera;

    public GeneratePerks perks;

    private bool isPlayerDead;

    public bool perkBeingSelected; //if a perk is being selected so weapon attacks can be cancelled

    private void Start()
    {
        //show normal ui and hide other ui on game start

        perkBeingSelected = false;

        NormalUI();
    }

    public void NormalUI() //enable normal ui
    {
        perkBeingSelected = false;

        normalUI.SetActive(true);
        deadUI.SetActive(false);
        victoryUI.SetActive(false);

        //lock cursor and allow camera movement

        playerCamera.GetComponent<PlayerCamera>().enabled = true;
        playerCamera.GetComponent<PlayerCamera>().LockCursor();
    }

    public void DeadUI() //enable dead ui
    {
        normalUI.SetActive(false);
        deadUI.SetActive(true);
        victoryUI.SetActive(false);

        //unlock cursor and stop camera movement

        playerCamera.GetComponent<PlayerCamera>().enabled = false;
        playerCamera.GetComponent<PlayerCamera>().UnlockCursor();
    }

    public void VictoryUI() //enable victory ui
    {
        perkBeingSelected = true;

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
