using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasTowardsCamera : MonoBehaviour
{
    [SerializeField]
    private Transform target; //player camera transform

    void Update()
    {
        var directionOfCamera = transform.position - target.transform.position; //calculate direction to look at
        var lookDirection = Quaternion.LookRotation(directionOfCamera); //calculate rotation
        transform.rotation = lookDirection; //point towards the player camera
    }
}
