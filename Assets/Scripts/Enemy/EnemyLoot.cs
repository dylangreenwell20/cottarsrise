using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLoot : MonoBehaviour
{
    public Transform lootSpawnPoint; //transform of where loot will spawn

    private LootPool lootPool; //reference to loot pool

    private void Awake()
    {
        lootPool = GameObject.Find("LootPool").GetComponent<LootPool>(); //get loot pool component
        lootSpawnPoint = transform.Find("LootSpawn"); //get point to spawn loot
    }


    public void SpawnItem()
    {
        lootSpawnPoint = transform.Find("LootSpawn"); //get point to spawn loot

        Item itemToSpawn = lootPool.Floor1LootPool(); //generate random item from floor 1 loot pool - change to current floor loot pool in future

        Debug.Log(itemToSpawn);

        //instantiate item prefab and add item to it

        GameObject prefab = itemToSpawn.itemPrefab;
        Instantiate(prefab, lootSpawnPoint); //instantiate item at chest location

        GameObject spawnedItem = lootSpawnPoint.GetChild(0).gameObject; //get game object of spawned prefab

        spawnedItem.AddComponent<ItemPickUp>(); //add ItemPickUp script to spawned loot
        spawnedItem.GetComponent<ItemPickUp>().item = itemToSpawn; //give loot the item scriptable object
    }
}
