using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosition; //variable for camera position

    private void Update()
    {
        transform.position = cameraPosition.transform.position;
    }
}
