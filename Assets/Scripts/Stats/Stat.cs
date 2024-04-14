using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] //show fields of class in inspector view
public class Stat
{
    [SerializeField]
    private int baseValue; //base stat value

    private List<int> modifiers = new List<int>(); //list of all modifiers gathered from gear

    public int GetValue()
    {
        int finalValue = baseValue; //final stat value variable created
        modifiers.ForEach(x => finalValue += x); //add each modifier to the finalValue
        return finalValue; //return final value - returning it from this function stops it from being edited
    }

    public void AddModifier(int modifier)
    {
        if (modifier != 0) //if modifier is not equal to 0
        {
            modifiers.Add(modifier); //add modifier to list
        }
    }

    public void RemoveModifier(int modifier)
    {
        if (modifier != 0) //if modifier is not equal to 0
        {
            modifiers.Remove(modifier); //remove modifier from list
        }
    }

    public void IncreaseStat(int increaseAmount) //increase base value of stat
    {
        baseValue += increaseAmount;
    }
}
