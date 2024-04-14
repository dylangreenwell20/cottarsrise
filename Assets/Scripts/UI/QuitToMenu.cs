using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitToMenu : MonoBehaviour
{
    public SelectedWeapon sW;

    public void ChangeToMenuScene()
    {
        sW.weaponFound = false;
        SceneManager.LoadScene(0); //load menu scene
    }
}
