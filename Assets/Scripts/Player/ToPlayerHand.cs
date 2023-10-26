using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToPlayerHand : MonoBehaviour
{
    public Transform handPosition; //variable for hand position

    private void Update()
    {
        transform.position = handPosition.transform.position;
    }
}