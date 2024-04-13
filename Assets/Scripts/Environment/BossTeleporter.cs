using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTeleporter : MonoBehaviour
{
    public GameObject arena; //boss arena
    public Transform arenaSpawn; //arena spawn location

    public Transform MoveToBossRoom()
    {
        arena = GameObject.Find("BossArena"); //find arena
        arenaSpawn = arena.transform.Find("Spawn"); //find arena spawn location

        return arenaSpawn;
    }
}
