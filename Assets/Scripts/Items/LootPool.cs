/*
using System;
using System.Collections;
using System.Collections.Generic;
*/
using System.Runtime.CompilerServices;
using UnityEngine;

public class LootPool : MonoBehaviour
{
    public Item[] floor1LootPool;

    private void Awake()
    {

        //floor 1 loot pool

        if(StartingWeapon.warriorClassSelected) //warrior loot pool
        {
            floor1LootPool = Resources.LoadAll<Item>("Floor1Items/Warrior"); //create array for all items in floor 1 loot pool
            return;
        }
        if(StartingWeapon.archerClassSelected) //archer loot pool
        {
            floor1LootPool = Resources.LoadAll<Item>("Floor1Items/Archer"); //create array for all items in floor 1 loot pool
            return;
        }
        if(StartingWeapon.mageClassSelected) //mage loot pool
        {
            floor1LootPool = Resources.LoadAll<Item>("Floor1Items/Mage"); //create array for all items in floor 1 loot pool
            return;
        }
        else //when testing game without loading from main menu
        {
            floor1LootPool = Resources.LoadAll<Item>("Floor1Items/Warrior"); //create array for all items in floor 1 loot pool
        }

        //floor 2

        //floor 3
    }

    public Item Floor1LootPool()
    {
        Random.InitState(System.DateTime.Now.Millisecond); //random seed for truly random items
        Item randomItem = floor1LootPool[Random.Range(0,floor1LootPool.Length)]; //get random item from loot pool

        return randomItem;
    }
}
