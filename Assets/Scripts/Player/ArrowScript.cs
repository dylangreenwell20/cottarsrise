using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public float arrowLife = 3; //seconds an arrow will be alive for
    private bool hasCollided; //has arrow collided yet
    public int bowDamage; //damage of bow
    private GameObject player; //reference to player game object
    private PlayerStats playerStats; //reference to player stats script
    private EquipmentManager equipmentManager; //reference to equipment manager

    private void Awake()
    {
        player = GameObject.Find("Player"); //find player game object
        playerStats = player.GetComponent<PlayerStats>(); //get player stats component from player
        equipmentManager = player.GetComponent<EquipmentManager>(); //get equipment manager component from player

        bowDamage = equipmentManager.currentEquipment[4].damage; //get damage of current weapon

        Destroy(gameObject, arrowLife); //destroy arrow after "arrowLife" seconds
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Arrow" && collision.gameObject.tag != "Player" && collision.gameObject.tag != "EnemyArrow" && collision.gameObject.tag != "EnemyProjectile" && !hasCollided) //if arrow has not collided with itself, the player and has not collided before
        {
            if (collision.gameObject.tag == "Enemy" && !hasCollided) //if arrow collided with enemy
            {
                hasCollided = true; //arrow has collided
                //Debug.Log("enemy tag verified"); //testing to see if the tag was registered as hit

                if (collision.gameObject.GetComponent<HealthController>() != null) //if the collided object has the component "HealthController"
                {
                    int damageToDeal = playerStats.DamageToDeal(bowDamage); //get damage value with gear modifiers applied

                    collision.gameObject.GetComponent<HealthController>().ApplyDamage(damageToDeal); //apply damage to the collided enemy
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
