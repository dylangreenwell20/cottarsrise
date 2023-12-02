using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")] //new header for movement category
    public float moveSpeed; //float for movement speed
    public float sprintSpeed; //float for sprint speed
    public float stealthSpeed; //float for stealth speed

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
    Vector3 moveDirection; //movement direction variable
    Rigidbody rb; //rigidbody variable

    public Camera cam; //reference to camera

    public bool isSprinting; //bool for if the player is sprinting or not
    public bool isStealthing; //bool for if the player is stealthing or not

    public bool isMoving; //bool for if the player is moving or not
    public bool canStealth; //bool for if player can stealth or not

    /*
    public bool canCrouch; //can the player crouch
    public float crouchingHeight; //height of crouching
    public float standingHeight; //height of standing
    */

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); //assigning rigid body
        rb.freezeRotation = true; //freeze rigid body rotation
        readyToJump = true; //player can jump
        canStealth = true; //player can stealth

        /*
        standingHeight = transform.localScale.y; //get current height of player - as they are standing by default, set this value to default standing height
        canCrouch = true; //player can crouch
        */
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

        if(!isStealthing && grounded) //if player is not in stealth mode and is grounded
        {
            if (Input.GetKeyDown(sprintKey)) //if sprint key is pressed down
            {
                isSprinting = true; //player is sprinting
            }
            if (Input.GetKeyUp(sprintKey)) //if sprint key is released
            {
                isSprinting = false; //player is no longer sprinting
            }
        }
        

        //make a tryingToCrouch bool or tryingToUncrouch bool and check if the player has uncrouched. if uncrouched check raycast above player if they are able to uncrouch
        //if cannot uncrouch then keep the player crouched BUT make it so the player height is not changed again in the code below

        if(canStealth && grounded) //if player can enter stealth mode and is grounded
        {
            if (!isStealthing) //if player is not stealthing
            {
                if (Input.GetKeyDown(stealthKey)) //if stealth key is pressed down
                {
                    isStealthing = true; //player is in stealth mode
                    /*
                    transform.localScale = new Vector3(transform.localScale.x, crouchingHeight, transform.localScale.z); //set height to crouchingHeight value
                    rb.AddForce(Vector3.down * 5f, ForceMode.Impulse); //move player downwards as they will be floating initially from the crouch
                    */
                }
            }

            if (isStealthing) //if player is in stealth mode
            {
                if (Input.GetKeyUp(stealthKey)) //if stealth key is released
                {
                    isStealthing = false; //player is not in stealth mode anymore
                    /*
                    transform.localScale = new Vector3(transform.localScale.x, standingHeight, transform.localScale.z); //set height to standingHeight value
                    isCrouching = false; //player is no longer crouching
                    */
                }
            }
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput; //move in the way the player is facing

        if (grounded) //if player is on the ground
        {
            if(isSprinting) //if player is sprinting
            {
                Debug.Log("SPRINTING"); //for testing
                rb.AddForce(moveDirection.normalized * sprintSpeed * 10f, ForceMode.Force); //add movement force for ground movement

                if (isStealthing) //if player begins stealthing
                {
                    isSprinting = false; //player no longer sprinting
                    return; //return function
                }
            }

            else if(isStealthing) //else if player is in stealth mode
            {
                Debug.Log("STEALTHING"); //for testing
                rb.AddForce(moveDirection.normalized * stealthSpeed * 10f, ForceMode.Force); //add movement force for ground movement
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

        if (isSprinting) //if player is sprinting
        {
            if (flatVel.magnitude > sprintSpeed) //if player speed is faster than max sprint speed
            {
                Vector3 limitedVel = flatVel.normalized * sprintSpeed; //calculate what the max velocity is
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z); //apply max velocity to current player speed
            }
        }
        else if (isStealthing) //else if player is stealthing
        {
            if (flatVel.magnitude > stealthSpeed) //if player speed is faster than max stealth speed
            {
                Vector3 limitedVel = flatVel.normalized * stealthSpeed; //calculate what the max velocity is
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

    /*
    private void Crouch()
    {
        StartCoroutine(CrouchOrStand()); //crouch or stand animation
    }

    
    private IEnumerator CrouchOrStand()
    {
        duringCrouchTransition = true; //player is currently in the crouching or standing animation

        float timeElapsed;
        float targetHeight = isCrouching ? normalHeight : crouchHeight;
        float currentHeight = characterController.height;
        Vector3 targetCentre = isCrouching ? standingCentre : crouchingCentre;
        Vector3 currentCentre = characterController.center;

        duringCrouchTransition = false; //player is no longer in the crouching or standing animation

        return null;
    }
    */
}
