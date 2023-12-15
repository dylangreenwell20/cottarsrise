using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArrowCounter : MonoBehaviour
{
    public int maxArrows = 10; //max number of arrows

    [SerializeField]
    private int currentArrows; //current number of arrows

    [SerializeField]
    private GameObject arrowPanel; //background panel for arrows

    [SerializeField]
    private TextMeshProUGUI arrowText; //text displaying number of arrows left

    [SerializeField]
    private GameObject arrowCountUI; //reference to arrow count ui game object

    private bool isDead; //bool to check if the player is dead or not

    public bool noArrows; //bool if there are 0 arrows or not
    public bool atMaxArrows; //bool to check if player has max arrows or not

    private GameObject player; //reference to player

    private bool classChecked; //if class has been checked or not

    private void Start()
    {
        currentArrows = maxArrows; //current arrows set to max arrows value
        atMaxArrows = true; //user is at max arrows
        UpdateUI(); //update the ui
    }

    private void Update()
    {
        if(!classChecked) //if class has not been checked
        {
            if (StartingWeapon.archerClassSelected) //if selected class is archer
            {
                arrowCountUI.SetActive(true); //enable arrow count ui
                classChecked = true; //class has been checked
            }
            else //else if selected class was not archer
            {
                arrowCountUI.SetActive(false); //disable arrow count ui
                classChecked = true; //class has been checked
            }
        }
        
    }

    public void LoseArrow(int numberOfArrows)
    {
        currentArrows -= numberOfArrows; //decrement current arrow value by numberOfArrows value
        atMaxArrows = false; //player cannot be at max arrows

        if (currentArrows == 0) //if player has no arrows left
        {
            noArrows = true; //player has no arrows left
        }

        UpdateUI(); //update the ui
    }

    public void GainArrow(int numberOfArrows)
    {
        if(atMaxArrows) //if at max arrows
        {
            return; //return function as a player cannot gain arrows if they are holding the max
        }

        currentArrows += numberOfArrows; //increase current number of arrows by amount gained
        noArrows = false; //player has more than 0 arrows

        if(currentArrows >= maxArrows) //if current arrow value exceeds max arrow value
        {
            currentArrows = maxArrows; //set current arrows to max arrow value
            atMaxArrows = true; //player is at max arrows
        }

        //Debug.Log("after" + currentArrows); //for testing

        UpdateUI(); //update the ui
    }

    public void UpdateUI()
    {
        arrowText.text = currentArrows + "/" + maxArrows; //set text to display current arrow and max arrow values
    }
}
