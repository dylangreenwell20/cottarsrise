using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")] //new header for movement category
    public float moveSpeed; //float for movement speed
    public float sprintSpeed; //float for sprint speed
    public float stealthSpeed; //float for stealth speed
    public float dashSpeed; //float for dash speed

    //public float crouchSpeed; //float for crouch speed

    public float groundDrag; //ground drag variable

    public float jumpForce; //variable for force of jump
    public float jumpCooldown; //variable for jump cooldown
    public float airMultiplier; //variable for air multiplier
    bool readyToJump; //boolean variable if the player is ready to jump or not (true = can jump, false = cannot jump)

    [Header("Keybinds")] //new header for player keybinds
    public KeyCode jumpKey = KeyCode.Space; //jump key set to space bar on keyboard
    public KeyCode sprintKey = KeyCode.LeftShift; //sprint key set to left shift on keyboard
    public KeyCode stealthKey = KeyCode.LeftControl; //stealth key set to left control on keyboard

    [Header("Ground Check")] //new header for checking if player is on the ground
    public Transform groundCheck; //transform variable for ground check
    public float groundDistance = 0f; //float variable for distance to ground
    public LayerMask whatIsGround; //layermask variable for what is the ground
    bool grounded; //boolean variable for grounded (true = on ground, false = off ground)

    public Transform orientation; //orientation variable
    float horizontalInput; //horizontal movement input variable
    float verticalInput; //vertical movement input variable
    public Vector3 moveDirection; //movement direction variable
    Rigidbody rb; //rigidbody variable

    public Camera cam; //reference to camera

    public bool isSprinting; //bool for if the player is sprinting or not
    public bool isMoving; //bool for if the player is moving or not
    public bool isDashing; //if player is dashing or not

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); //assigning rigid body
        rb.freezeRotation = true; //freeze rigid body rotation
        readyToJump = true; //player can jump
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

            if (Physics.Raycast(ray, out hit, 3)) //if raycast hit something within 3 units
            {
                InteractableItems interactable = hit.collider.GetComponent<InteractableItems>(); //get InteractableItems component from what was hit
                if(interactable != null) //if component was valid and variable was created (essentially if the item is interactable)
                {
                    interactable.Interact(); //pick up the item
                    return;
                }

                Chest chest = hit.collider.GetComponent<Chest>(); //get chest component from what was hit

                if(chest != null) //if item has chest component
                {
                    if(chest.isOpen) //if chest is already open
                    {
                        return;
                    }
                    chest.Interact(); //open chest
                    return;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        MovePlayer(); //run MovePlayer function

        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z); //variable for current player speed

        if(flatVel.magnitude > 3.1f) //if player has a speed greater than 0 (essentially if they are currently moving)
        {
            isMoving = true; //isMoving set to true
        }
        else //else if player speed is 0
        {
            isMoving = false; //isMoving set to false
        }
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

        if(grounded) //if player is grounded
        {
            if (Input.GetKey(sprintKey)) //if sprint key is pressed down
            {
                isSprinting = true; //player is sprinting
            }
        }
        if (Input.GetKeyUp(sprintKey)) //if sprint key is released
        {
            isSprinting = false; //player is no longer sprinting
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput; //move in the way the player is facing

        if (grounded) //if player is on the ground
        {
            if(isSprinting) //if player is sprinting
            {
                //Debug.Log("SPRINTING"); //for testing
                rb.AddForce(moveDirection.normalized * sprintSpeed * 10f, ForceMode.Force); //add movement force for ground movement
            }

            else //else if player is not sprinting or crouching (they are walking)
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force); //add movement force for ground movement
            }
        }
        
        else if (!grounded) //if player is not on the ground
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force); //add movement force for non-grounded movement
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z); //variable for current player speed

        if (isDashing) //if player is dashing
        {
            if (flatVel.magnitude > dashSpeed) //if player speed is faster than max dash speed
            {
                Vector3 limitedVel = flatVel.normalized * dashSpeed; //calculate what max velocity is
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z); //apply max velocity to current player speed
            }
        }
        else if (isSprinting) //if player is sprinting
        {
            if (flatVel.magnitude > sprintSpeed) //if player speed is faster than max sprint speed
            {
                Vector3 limitedVel = flatVel.normalized * sprintSpeed; //calculate what the max velocity is
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z); //apply max velocity to current player speed
            }
        }
        else if (flatVel.magnitude > moveSpeed) //else if player speed is faster than max walking speed
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

    public void DashState(float cooldownTime)
    {
        isDashing = true;
        StartCoroutine(DashCooldown(cooldownTime));
    }

    IEnumerator DashCooldown(float cooldownTime)
    {
        yield return new WaitForSeconds(cooldownTime);
        isDashing = false;
    }
}
