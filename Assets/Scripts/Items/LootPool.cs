/*
using System;
using System.Collections;
using System.Collections.Generic;
*/
using UnityEngine;

public class LootPool : MonoBehaviour
{
    public Item[] floor1LootPool;

    private void Awake()
    {

        //floor 1 loot pool
        floor1LootPool = Resources.LoadAll<Item>("Floor1Items"); //create array for all items in floor 1 loot pool

        //floor 2

        //floor 3
    }

    public Item Floor1LootPool()
    {
        Item randomItem = floor1LootPool[Random.Range(0,floor1LootPool.Length)]; //get random item from loot pool

        return randomItem;
    }
}
