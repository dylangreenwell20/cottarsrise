using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLootSpawn : MonoBehaviour
{
    private Transform lootPosition; //reference to loot position (on capsule)

    private void Awake()
    {
        lootPosition = this.transform.parent.Find("Capsule/LootPosition"); //get loot position transform
    }

    private void Update()
    {
        this.transform.position = lootPosition.position; //move to position of loot spawn
    }
}
