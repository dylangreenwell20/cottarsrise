using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballProjectile : MonoBehaviour
{
    private bool hasCollided; //if projectile has collided or not
    private int projectileDamage; //damage of projectile
    private GameObject player; //player game object
    private PlayerStats playerStats; //reference to player stats script

    private void Awake()
    {
        player = GameObject.Find("Player"); //find player game object
        playerStats = player.GetComponent<PlayerStats>(); //get player stats component from player
        projectileDamage = AbilityVariables.abilityDamage; //get ability damage

        Debug.Log("fireball created - damage is " + projectileDamage.ToString()); //for testing
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided with: " + collision.gameObject.name); //for testing

        if (collision.gameObject.tag != "AbilityProjectile" && collision.gameObject.tag != "Player" && collision.gameObject.tag != "EnemyArrow" && collision.gameObject.tag != "EnemyProjectile" && !hasCollided) //if projectile collides with something other than itself, the player, enemy arrows/projectiles and has not already collided
        {
            if (collision.gameObject.tag == "Enemy" && !hasCollided) //if projectile collided with enemy
            {
                hasCollided = true; //projectile has collided
                if (collision.gameObject.GetComponent<HealthController>() != null) //if the collided object has the component "HealthController"
                {
                    int damageToDeal = playerStats.DamageToDeal(projectileDamage); //get damage value with gear modifiers applied

                    collision.gameObject.GetComponent<HealthController>().ApplyDamage(damageToDeal); //apply damage to the collided enemy
                    //Debug.Log("enemy damaged"); //testing to see if enemy was successfully damaged
                }

                //var impact = Instantiate(impactParticles, collision.contacts[0].point, Quaternion.identity) as GameObject; //create impact particles on first collision point
                //Destroy(impact, 2); //destroy impact particles after 2 seconds

                Destroy(gameObject); //destroy the projectile
            }
            else //if projectile did not collide with enemy
            {
                hasCollided = true; //has collided set to true

                //var impact = Instantiate(impactParticles, collision.contacts[0].point, Quaternion.identity) as GameObject; //create impact particles on first collision point
                //Destroy(impact, 2); //destroy impact particles after 2 seconds

                Destroy(gameObject); //destroy the projectile
            }
        }
    }
}
