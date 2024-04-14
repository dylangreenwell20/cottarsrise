using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Perk", menuName = "Perk/New Perk")]

public class Perk : ScriptableObject
{
    new public string name = "Perk name"; //overwrite and create perk name
    public Sprite perkSprite; //sprite for perk
    public string perkDescription; //description of perk
    public int statValue; //value to increase stat by

    public WhatStat whatStat; //type of stat the perk increases

    public virtual void Apply() //apply the perk based on its type
    {
        Debug.Log("applied " +  name);

        GameObject player = GameObject.Find("Player"); //find player


        if (this.whatStat == WhatStat.Health)
        {
            //add health
        }
        else if (this.whatStat == WhatStat.Mana)
        {
            //add mana
        }
        else if (this.whatStat == WhatStat.Damage)
        {
            //add damage
        }
        else if (this.whatStat == WhatStat.Dodge)
        {
            //add dodge chance
        }
    }


}

public enum WhatStat { Health, Mana, Damage, Dodge}