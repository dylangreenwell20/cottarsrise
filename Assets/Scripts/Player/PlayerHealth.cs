using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private float maxHealth; //max health of player

    private float currentHealth; //current health of player

    [SerializeField]
    private GameObject healthPanel; //reference to health panel

    [SerializeField]
    private TextMeshProUGUI healthText; //reference to health text

    [SerializeField]
    private RectTransform healthBar; //reference to actual health bar

    private float healthBarStartWidth; //starting width of health bar (at max health)

    public bool isDead; //is the health at 0 or not

    private void Start()
    {
        currentHealth = maxHealth; //set current hp to max hp
        healthBarStartWidth = healthBar.sizeDelta.x; //starting size of the hp bar
        UpdateUI(); //update the ui
    }

    public void DamagePlayer(float damage)
    {
        if(isDead) //if player is already dead
        {
            return; //exit function as player is dead
        }

        currentHealth -= damage; //take damage away from current health

        if(currentHealth <= 0) //if current health is less than or equal to 0
        {
            currentHealth = 0; //set current health to 0 to avoid negative hp
            isDead = true; //player is dead

            //GAME OVER SCRIPT HERE
            //GAME OVER SCRIPT HERE
            //GAME OVER SCRIPT HERE
        }

        UpdateUI(); //update the ui
    }

    public void UpdateUI()
    {
        float percent = (currentHealth / maxHealth) * 100; //calculate percent of hp left
        float newWidth = (percent / 100) * healthBarStartWidth; //new width for the green hp bar

        healthBar.sizeDelta = new Vector2(newWidth, healthBar.sizeDelta.y); //calculate width of the amount of green hp left in the bar
        healthText.text = currentHealth + "/" + maxHealth; //update ui text to show health value
    }
}
