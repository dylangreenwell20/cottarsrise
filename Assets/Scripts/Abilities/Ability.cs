using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;


public class Ability : ScriptableObject
{
    new public string name = "Ability name"; //overwrite and create new ability name variable
    public float cooldownTime; //ability cooldown variable
    public float activeTime; //ability active time variable
    public int abilityDamage; //damage value for abilities
    public float abilityVelocity; //velocity value for abilites
    public int manaCost; //mana cost of the ability
    public Sprite abilitySprite; //sprite (image) of ability

    public virtual void Activate(GameObject parent)
    {
        Debug.Log("Ability " + name + " used!"); //for testing
    }
}
