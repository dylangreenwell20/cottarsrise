using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int maxHealth; //max health value

    public int currentHealth {  get; private set; } //current health value which can only be modified from inside this class but can be read from outside the class

    public Stat damage; //stat for damage
    public Stat armour; //stat for armour

    public Stat health; //stat for health
    public Stat mana; //stat for mana

    private void Awake()
    {
        currentHealth = maxHealth; //set current health to max health
    }

    public int DamageToTake(int damage)
    {
        if (armour.GetValue() == 0) //if player has 0 armour value
        {
            return damage; //return damage as no armour reduction can be applied
        }

        float armourValue = 1; //create armour value float
        armourValue -= ((float)armour.GetValue() / 100); //calculate damage reduction using armour value
        int damageCalculation = (int)Mathf.Round((float)damage * armourValue); //multiply damage by damage reduction (damage is rounded to nearest integer)

        return damageCalculation; //return damage with armour reduction applied
    }

    public int DamageToDeal(int characterDamage)
    {
        if(damage.GetValue() == 0) //if damage modifiers are equal to 0
        {
            return characterDamage; //return damage as there are no modifiers
        }

        float damageValue = 1; //base damage multiplier
        damageValue += ((float)damage.GetValue() / 100); //add decimal of damage multiplier (e.g 30 damage = 0.30 so this damageValue would become 1.30 which is 1.3x damage)
        int damageToDeal = (int)Mathf.Round((float)characterDamage * damageValue);

        return damageToDeal; //return updated value
    }

    public void UpdateHealth(int healthValue) //update health value when health should be increased
    {
        maxHealth += healthValue;

        currentHealth = maxHealth;
    }

    public void UpdateDamage(int damageValue) //increase base damage by perk value
    {
        damage.IncreaseStat(damageValue);
    }
}
