using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    public GameObject[] walls; //wall prefabs - 0 = north, 1 = south, 2 = east, 3 = west
    public GameObject[] doors; //door prefabs - same indexing as above

    public void UpdateRoom(bool[] status)
    {
        for(int i = 0; i < status.Length; i++) //for length of status array
        {
            doors[i].SetActive(status[i]); //set door status to value of status[i]
            walls[i].SetActive(!status[i]); //set wall status to inverse value of status[i]
        }
    }
}
