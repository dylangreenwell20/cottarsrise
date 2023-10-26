using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleRotation : MonoBehaviour
{
    public float sensX; //camera sensitivity variables
    public float sensY;

    public Transform orientation; //orientation variable

    //float xRotation; //rotation variables
    float yRotation;

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked; //lock mouse in middle of screen
        //Cursor.visible = false; //invisible cursor
    }

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * sensX; //variables for storing mouse input (x and y directions)
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.fixedDeltaTime * sensY;

        yRotation += mouseX; //applying mouse rotation
        //xRotation -= mouseY;
        //xRotation = Mathf.Clamp(xRotation, -90f, 90f); //locks camera from looking straight up or straight down

        transform.rotation = Quaternion.Euler(0, yRotation, 0); //rotating the camera
        //orientation.rotation = Quaternion.Euler(0, yRotation, 0); //rotating the orientation
    }
}
