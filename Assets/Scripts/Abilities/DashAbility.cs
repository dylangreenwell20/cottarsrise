using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dash Ability", menuName = "Abilities/Dash")]
public class DashAbility : Ability
{
    public override void Activate(GameObject parent)
    {
        base.Activate(parent); //run base Activate() function

        PlayerMovement pm = parent.GetComponent<PlayerMovement>(); //get player movement script
        Rigidbody rb = parent.GetComponent<Rigidbody>(); //get player rigid body

        pm.DashState(activeTime); //enable dash state for ability active duration

        if(rb.velocity == Vector3.zero) //if player not moving
        {
            GameObject playerOrientation = GameObject.Find("CameraHolder").transform.Find("PlayerCamera").gameObject; //get player orientation

            rb.AddForce(playerOrientation.transform.forward * abilityVelocity * 10f, ForceMode.Force); //move towards orientation
        }
        else //else if player is moving
        {
            rb.AddForce(pm.moveDirection.normalized * abilityVelocity * 10f, ForceMode.Force); //dash in the direction the player is moving
        }
    }
}
