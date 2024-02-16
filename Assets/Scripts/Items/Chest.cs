using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public float radius = 1.0f; //radius of interactable area
    public Transform interactionPoint; //transform of where interaction will occur from
    public Transform lootSpawnPoint; //transform of where loot will spawn

    private Animator animator; //reference to animator
    private LootPool lootPool; //reference to loot pool

    public bool isOpen; //is chest open

    private void Awake()
    {
        isOpen = false; //chest is closed
        animator = this.GetComponent<Animator>(); //get animator component
        lootPool = GameObject.Find("LootPool").GetComponent<LootPool>(); //get loot pool component
        lootSpawnPoint = transform.Find("LootSpawn"); //find loot spawn transform
    }

    private void OnDrawGizmosSelected()
    {
        if (interactionPoint == null) //if no interaction point has been set
        {
            interactionPoint = transform; //set interaction point to this object's own transform
        }

        Gizmos.color = Color.yellow; //assign colour to yellow
        Gizmos.DrawWireSphere(interactionPoint.position, radius); //draw a sphere to show interactable area
    }

    public virtual void Interact()
    {
        Debug.Log("Player opened: " + transform.name); //debug to say the player interacted with the item

        isOpen = true;

        SpawnItem(); //spawn item in chest

        animator.SetTrigger("ChestOpen"); //play chest open animation
    }

    public void SpawnItem()
    {
        Item itemToSpawn = lootPool.Floor1LootPool(); //generate random item from floor 1 loot pool - change to current floor loot pool in future

        Debug.Log(itemToSpawn);

        //instantiate item prefab and add item to it

        GameObject prefab = itemToSpawn.itemPrefab;
        Instantiate(prefab, new Vector3(lootSpawnPoint.position.x, lootSpawnPoint.position.y, lootSpawnPoint.position.z), Quaternion.Euler(lootSpawnPoint.rotation.x, lootSpawnPoint.rotation.y, lootSpawnPoint.rotation.z), lootSpawnPoint); //instantiate item at chest location

        GameObject spawnedItem = lootSpawnPoint.GetChild(0).gameObject; //get game object of spawned prefab

        spawnedItem.AddComponent<ItemPickUp>(); //add ItemPickUp script to spawned loot
        spawnedItem.GetComponent<ItemPickUp>().item = itemToSpawn; //give loot the item scriptable object
    }
}
