using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public float arrowLife = 3; //seconds an arrow will be alive for
    private bool hasCollided; //has arrow collided yet
    public float bowDamage = 20; //damage of bow

    private void Awake()
    {
        Destroy(gameObject, arrowLife); //destroy arrow after "arrowLife" seconds
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Arrow" && collision.gameObject.tag != "Player" && !hasCollided) //if arrow has not collided with itself, the player and has not collided before
        {
            if (collision.gameObject.tag == "Enemy" && !hasCollided) //if arrow collided with enemy
            {
                hasCollided = true; //arrow has collided
                //Debug.Log("enemy tag verified"); //testing to see if the tag was registered as hit
                if (collision.gameObject.GetComponent<HealthController>() != null) //if the collided object has the component "HealthController"
                {
                    collision.gameObject.GetComponent<HealthController>().ApplyDamage(bowDamage); //apply damage to the collided enemy
                    //Debug.Log("enemy damaged"); //testing to see if enemy was successfully damaged
                }
                Destroy(gameObject); //destroy the arrow
            }
            else //if the arrow did not collide with an enemy
            {
                hasCollided = true; //arrow has collided
                Destroy(gameObject); //destroy the arrow
                //string tagDebug = collision.gameObject.tag; //for testing to see what tag was hit
                //Debug.Log(tagDebug); //send tag to debug log
                //Debug.Log("enemy not damaged"); //string to debug log
            }
        }
    }
}
