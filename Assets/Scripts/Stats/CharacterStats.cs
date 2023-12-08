using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int maxHealth = 100; //max health value

    public int currentHealth {  get; private set; } //current health value which can only be modified from inside this class but can be read from outside the class

    public Stat damage; //stat for damage
    public Stat armour; //stat for armour

    private void Awake()
    {
        currentHealth = maxHealth; //set current health to max health
    }

    private void Update()
    {
        //THIS WILL BECOME A PART OF THE ENEMY AI ATTACK SCRIPT - I AM TEMPORARILY USING T TO DEAL DAMAGE JUST TO TEST IF THE SYSTEM WORKS

        if (Input.GetKeyDown(KeyCode.T)) //if t is pressed
        {
            TakeDamage(10); //take 10 damage
        }
    }

    public void TakeDamage(int damage)
    {
        //armour works as a percentage out of 100 (e.g 30 armour = 30% damage reduction)

        if(armour.GetValue() > 0) //if player has more than 0 armour
        {
            float armourValue = 1;
            armourValue -= ((float)armour.GetValue() / 100); //calculate damage reduction using armour value
            int damageCalculation = (int)Mathf.Round((float)damage * armourValue); //multiply damage by damage reduction (damage is rounded to nearest integer)
            currentHealth -= damageCalculation; //take away damage from current health

            //Debug.Log(armour.GetValue());
            //Debug.Log(armourValue);           FOR TESTING IF DAMAGE WAS BEING CALCULATED PROPERLY
            //Debug.Log(damageCalculation);

            Debug.Log(transform.name + " takes " + damageCalculation + " damage."); //for testing to see what took damage and how much damage
        }
        else //else if player has 0 armour
        {
            currentHealth -= damage; //take away damage from health
            Debug.Log(transform.name + " takes " + damage + " damage."); //for testing to see what took damage and how much damage
        }

        if(currentHealth <= 0) //if current health is less than 0 or equal to 0
        {
            currentHealth = 0; //set current health to 0
            Die(); //die method
        }
    }

    public virtual void Die()
    {
        //this method will be overwritten by other character types (player, enemy etc) as each death will have a different procedure (player death = death screen, enemy death = loot drop)
        Debug.Log(transform.name + " died..."); //debug message for testing
    }
}
