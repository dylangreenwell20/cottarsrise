using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTeleporter : MonoBehaviour
{
    public Transform arenaSpawn; //arena spawn location

    public Transform SpawnLocation()
    {
        arenaSpawn = GameObject.Find("BossArena").transform.Find("Spawn"); //find spawn location

        return arenaSpawn; //return spawn location
    }
}
