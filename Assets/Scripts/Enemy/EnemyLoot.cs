using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLoot : MonoBehaviour
{
    public Transform lootSpawnPoint; //transform of where loot will spawn

    private LootPool lootPool; //reference to loot pool

    public GameObject lootBag; //loot bag prefab

    public EnemyAI enemyAI; //enemy ai script

    private void Awake()
    {
        enemyAI = transform.Find("Capsule").GetComponent<EnemyAI>(); //get enemy ai component
        lootPool = GameObject.Find("LootPool").GetComponent<LootPool>(); //get loot pool component
        lootSpawnPoint = transform.Find("LootSpawn"); //get point to spawn loot
    }


    public void SpawnItem()
    {
        if (enemyAI.isBoss)
        {
            return;
        }

        lootSpawnPoint = transform.Find("LootSpawn"); //get point to spawn loot

        Item itemToSpawn = lootPool.Floor1LootPool(); //generate random item from floor 1 loot pool - change to current floor loot pool in future

        Debug.Log(itemToSpawn);

        int chance = Random.Range(0, 5); //20% chance

        if(chance == 0)
        {
            //instantiate item prefab and add item to it

            //GameObject prefab = itemToSpawn.itemPrefab;
            Instantiate(lootBag, new Vector3(lootSpawnPoint.position.x, lootSpawnPoint.position.y, lootSpawnPoint.position.z), Quaternion.Euler(lootSpawnPoint.rotation.x, lootSpawnPoint.rotation.y, lootSpawnPoint.rotation.z), lootSpawnPoint); //instantiate item at enemy loot location

            GameObject spawnedItem = lootSpawnPoint.GetChild(0).gameObject; //get game object of spawned prefab

            spawnedItem.AddComponent<ItemPickUp>(); //add ItemPickUp script to spawned loot
            spawnedItem.GetComponent<ItemPickUp>().item = itemToSpawn; //give loot the item scriptable object
            spawnedItem.layer = 17; //set to loot layer tag
            //spawnedItem.GetComponent<BoxCollider>().enabled = false; //disable collision
        }
    }
}
