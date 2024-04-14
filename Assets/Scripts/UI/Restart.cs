using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
    public SelectedWeapon sW;

    public void RestartGame()
    {
        sW.weaponFound = false;
        string currentScene = SceneManager.GetActiveScene().name; //get name of current scene
        SceneManager.LoadScene(currentScene); //load current scene
    }
}
