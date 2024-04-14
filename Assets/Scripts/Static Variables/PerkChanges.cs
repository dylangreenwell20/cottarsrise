using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkChanges : MonoBehaviour
{
    //the int values will be 0 by default - character stats will check for which is greater than 0 and then apply the perk

    public static int healthChange, manaChange, damageChange, dodgeChange; //different stat change values to be applied
    public static bool perkToApply; //if there is a perk to apply
}
