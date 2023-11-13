using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthController : MonoBehaviour
{
    [SerializeField]
    private float maxHealth; //max health of enemy

    private float currentHealth; //current health of enemy

    [SerializeField]
    private GameObject healthPanel; //reference to health panel

    [SerializeField]
    private TextMeshProUGUI healthText; //reference to health text

    [SerializeField]
    private RectTransform healthBar; //reference to actual health bar

    private float healthBarStartWidth; //starting width of health bar (at max health)

    private MeshRenderer mR; //mesh renderer of the enemy

    private CapsuleCollider cC; //capsule collider of the enemy

    private bool isDead; //is the health at 0 or not

    private void Start()
    {
        mR = GetComponent<MeshRenderer>(); //get mesh renderer
        cC = GetComponent<CapsuleCollider>(); //get capsule collider
        currentHealth = maxHealth; //current health is max health
        healthBarStartWidth = healthBar.sizeDelta.x; //width of health bar
        UpdateUI(); //update the health bar UI
    }

    public void ApplyDamage(float damage)
    {
        if(isDead == true) //if enemy is dead
        {
            return; //return
        }

        //HERE I WOULD APPLY DODGE CHANCE OR BLOCK CHANCE FOR ENEMIES

        currentHealth -= damage; //take away damage from current health

        if(currentHealth <= 0) //if health is less than or equal to 0 after damage applied
        {
            currentHealth = 0; //set health to 0 to avoid negative health
            isDead = true; //enemy is dead
            Destroy(gameObject); //destroy the enemy gameobject

            //cC.enabled = false; //disable the collision of the enemy                      --- these lines are for just disabling the capsule collider, mesh renderer and health canvas on death
            //mR.enabled = false; //disable mesh renderer to hide enemy                         the game object would still persist so this is just mainly for testing what happens
            //healthPanel.SetActive(false); //disable health bar                                when enemies die and what code would still run - basically this is for testing
        }

        UpdateUI(); //update the health bar UI
    }

    private void UpdateUI()
    {
        float percent = (currentHealth / maxHealth) * 100; //calculate the percent of the health bar which should be filled
        float newWidth = (percent / 100) * healthBarStartWidth; //new width of health bar

        healthBar.sizeDelta = new Vector2(newWidth, healthBar.sizeDelta.y); //calculate size of health bar
        healthText.text = currentHealth + "/" + maxHealth; //change health bar number
    }
}
