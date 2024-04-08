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


    public GameObject enemyPrefab; //enemy prefab - CHANGE TO ARRAY IN THE FUTURE WITH MANY ENEMIES AND RANDOMLY PICK ENEMY TYPES TO SPAWN






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

        //spawn chests/potions/enemies

        //spawn enemies

        if(enemySpawnLocations.Length > 0) //if the room has enemy spawn points
        {
            for(int i = 0; i < enemySpawnLocations.Length; i++) //for each enemy spawn point
            {
                int chance = Random.Range(0, 2); //50/50 chance of 0 or 1

                if(chance == 0)
                {
                    Instantiate(enemyPrefab, enemySpawnLocations[i].transform.position, Quaternion.identity, transform); //instantiate an enemy
                }
            }
        }

    }
}
