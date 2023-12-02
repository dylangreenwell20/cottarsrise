using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")] //new header for movement category
    public float moveSpeed; //float for movement speed

    public float groundDrag; //ground drag variable

    public float jumpForce; //variable for force of jump
    public float jumpCooldown; //variable for jump cooldown
    public float airMultiplier; //variable for air multiplier
    bool readyToJump; //boolean variable if the player is ready to jump or not (true = can jump, false = cannot jump)

    [Header("Keybinds")] //new header for player keybinds
    public KeyCode jumpKey = KeyCode.Space; //jump key set to space bar on keyboard

    [Header("Ground Check")] //new header for checking if player is on the ground
    public Transform groundCheck; //transform variable for ground check
    public float groundDistance = 0f; //float variable for distance to ground
    public LayerMask whatIsGround; //layermask variable for what is the ground
    bool grounded; //boolean variable for grounded (true = on ground, false = off ground)

    public Transform orientation; //orientation variable
    float horizontalInput; //horizontal movement input variable
    float verticalInput; //vertical movement input variable
    Vector3 moveDirection; //movement direction variable
    Rigidbody rb; //rigidbody variable

    public Camera cam; //reference to camera

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); //assigning rigid body
        rb.freezeRotation = true; //freeze rigid body rotation
        readyToJump = true;
    }

    private void Update()
    {
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, whatIsGround); //check if the player is on the ground
        MyInput(); //run MyInput function
        SpeedControl(); //run SpeedControl function

        if (grounded) //if player is on the ground
        {
            rb.drag = groundDrag; //apply ground drag to movement
        }
        else //if player is not on the ground
        {
            rb.drag = 0; //apply no drag to movement
        }

        if (Input.GetKeyDown(KeyCode.E)) //if E is pressed
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //create a ray from the camera where the player is looking
            RaycastHit hit; //create new raycasthit variable

            if (Physics.Raycast(ray, out hit, 3)) //if raycast hit something within 10 units
            {
                InteractableItems interactable = hit.collider.GetComponent<InteractableItems>(); //get InteractableItems component from what was hit
                if(interactable != null) //if component was valid and variable was created (essentially if the item is interactable)
                {
                    interactable.Interact(); //pick up the item
                }
            }
        }
    }

    private void FixedUpdate()
    {
        MovePlayer(); //run MovePlayer function
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal"); //get horizontal input
        verticalInput = Input.GetAxisRaw("Vertical"); //get vertical input

        if(Input.GetKeyDown(jumpKey) && readyToJump && grounded) //if player has pressed jump key and the boolean variables readyToJump and grounded are both true
        { //IF I WANT USER TO BE ABLE TO HOLD SPACE AND KEEP JUMPING CONSTANTLY, CHANGE "Input.GeyKeyDown" TO "Input.GetKey"
            readyToJump = false; //set readyToJump to false
            Jump(); //run Jump function
            Invoke(nameof(ResetJump), jumpCooldown); //invoke the ResetJump function with jumpCooldown
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput; //move in the way the player is facing

        if (grounded) //if player is on the ground
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force); //add movement force for ground movement
        }
        
        else if (!grounded) //if player is not on the ground
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force); //add movement force for non-grounded movement
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z); //variable for current player speed

        if(flatVel.magnitude > moveSpeed) //if player speed is faster than max speed
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed; //calculate what the max velocity is
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z); //apply max velocity to current player speed
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z); //reset the y velocity so player will always jump the same height
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse); //apply jump force to player
    }

    private void ResetJump()
    {
        readyToJump = true; //set readyToJump to true
    }

}
