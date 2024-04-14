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
        sW.weaponFound = false;
        pM.bossButtonPressed = false;
        uiStatus.perkBeingSelected = false;
        SceneManager.LoadScene(0); //load menu scene
    }
}
