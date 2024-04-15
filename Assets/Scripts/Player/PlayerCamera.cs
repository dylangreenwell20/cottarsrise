using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float sensX; //camera sensitivity variables
    public float sensY;

    public Transform orientation; //orientation variable

    float xRotation; //rotation variables
    float yRotation;

    private bool cursorUnlocked; //bool to check if cursor is currently locked or unlocked

    public PlayerMovement pm; //check if player has entered boss arena

    private void Start()
    {
        LockCursor(); //lock the cursor in the game so player can look around
    }

    private void Update()
    {
        if(pm.bossButtonPressed) //if boss teleport button pressed, make player face north towards boss
        {
            transform.rotation = Quaternion.identity;
            orientation.rotation = Quaternion.identity;

            xRotation = 0;
            yRotation = 0;

            return;
        }

        if (cursorUnlocked) //if cursor is unlocked
        {
            return; //return function so mouse movement will not rotate the camera
        }

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * sensX; //variables for storing mouse input (x and y directions)
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.fixedDeltaTime * sensY;

        yRotation += mouseX; //applying mouse rotation
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //locks camera from looking straight up or straight down

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0); //rotating the camera
        orientation.rotation = Quaternion.Euler(0, yRotation, 0); //rotating the orientation
    }

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked; //lock mouse in middle of screen
        Cursor.visible = false; //invisible cursor
        cursorUnlocked = false; //cursor is invisible and locked
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None; //allow mouse to move around
        Cursor.visible = true; //visible cursor
        cursorUnlocked = true; //cursor is visible and unlocked
    }
}
