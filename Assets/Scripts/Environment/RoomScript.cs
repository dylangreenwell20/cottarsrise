using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    public GameObject[] walls; //wall prefabs - 0 = north, 1 = south, 2 = east, 3 = west
    public GameObject[] doors; //door prefabs - same indexing as above
    public GameObject[] door_walls; //door wall prefabs - same indexing as above
    public GameObject[] enemySpawnLocations; //spawn locations of enemies
    public GameObject[] potionLocations; //locations of potions
    public GameObject[] chestLocations; //locations of chests
    public GameObject[] enemyPrefabs; //enemy prefabs

    public GameObject player; //reference to player
    public Transform dungeonSpawn; //reference to dungeon spawn

    public void UpdateRoom(bool[] status, bool[] invisible)
    {
        for(int i = 0; i < status.Length; i++) //for length of status array
        {
            door_walls[i].SetActive(status[i]); //set door wall status to value of status[i]
            doors[i].SetActive(status[i]); //set door status to value of status[i]
            walls[i].SetActive(!status[i]); //set wall status to inverse value of status[i]

            if (invisible[i]) //if door should be invisible
            {
                door_walls[i].SetActive(false); //hide door wall
                doors[i].SetActive(false); //hide door
            }
        }

        //choose what potions to keep/hide

        if(potionLocations.Length > 0) //if the room has potions
        {
            for(int i = 0; i < potionLocations.Length; i++) //for each potion
            {
                int chance = Random.Range(0, 2); //50/50 chance of 0 or 1

                if(chance == 0) //if 50% chance has rolled to hide the potion
                {
                    Destroy(potionLocations[i]); //destroy the potion
                }
            }
        }

        //choose what chests to keep/hide
        
        if (chestLocations.Length > 0) //if the room has chests
        {
            for (int i = 0; i < chestLocations.Length; i++) //for each chest
            {
                int chance = Random.Range(0, 2); //50/50 chance of 0 or 1

                if (chance == 0) //if 50% chance has rolled to hide the chest
                {
                    Destroy(chestLocations[i]); //destroy the chest
                }
            }
        }
    }

    public void SpawnEnemies()
    {
        //spawn enemies

        if (enemySpawnLocations.Length > 0) //if the room has enemy spawn points
        {
            for (int i = 0; i < enemySpawnLocations.Length; i++) //for each enemy spawn point
            {
                int chance = Random.Range(0, 2); //50/50 chance of 0 or 1

                if (chance == 0)
                {
                    int enemyRNG = Random.Range(0, enemyPrefabs.Length); //pick random enemy to spawn

                    Instantiate(enemyPrefabs[enemyRNG], enemySpawnLocations[i].transform.position, Quaternion.identity, transform); //instantiate an enemy
                }
            }
        }
    }

    public void MovePlayer() //move player to dungeon start
    {
        player = GameObject.Find("Player");
        player.transform.position = dungeonSpawn.position;

        Debug.Log("moved player");
    }
}
