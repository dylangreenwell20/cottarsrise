using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkHolder : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public PlayerMana playerMana;
    public PlayerStats playerStats;
    public PotionUI potionUI;

    public void ApplyPerk()
    {
        if (PerkChanges.chosenPerk) //if there is a chosen perk
        {
            Perk perkToApply = PerkChanges.chosenPerk; //get chosen perk

            if (perkToApply.whatStat == WhatStat.Health)
            {
                //add health

                playerHealth.GainHealth(perkToApply.statValue); //increase player health by perk amount
            }
            else if (perkToApply.whatStat == WhatStat.Mana)
            {
                //add mana

                playerMana.GainMana(perkToApply.statValue); //increase player mana by perk amount
            }
            else if (perkToApply.whatStat == WhatStat.Damage)
            {
                //add damage

                playerStats.UpdateDamage(perkToApply.statValue); //increase damage by perk amount
            }
            else if (perkToApply.whatStat == WhatStat.Add3HealthPots)
            {
                //add 3 health potions

                Inventory.instance.healthPotCount += 3; //add 3 health potions to inventory
                potionUI.UpdateHealthPotionUI();
            }
            else if (perkToApply.whatStat == WhatStat.Add3ManaPots)
            {
                //add 3 mana potions

                Inventory.instance.manaPotCount += 3; //add 3 mana potions to inventory
                potionUI.UpdateManaPotionUI();
            }
        }
    }
}
