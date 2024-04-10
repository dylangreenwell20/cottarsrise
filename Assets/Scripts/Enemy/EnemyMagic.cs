using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMagic : MonoBehaviour
{
    public GameObject impactParticles; //projectile impact particle effect
    private bool hasCollided; //if projectile has collided or not
    public int projectileDamage = 20; //damage of projectile
    public float projectileLife = 3; //life of projectile

    private void Awake()
    {
        Destroy(gameObject, projectileLife); //destroy after a few seconds
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "EnemyProjectile" && collision.gameObject.tag != "Enemy" && collision.gameObject.tag != "Arrow" && collision.gameObject.tag != "StaffProjectile" && collision.gameObject.tag != "AbilityProjectile" && !hasCollided) //if projectile hasnt collided with itself, an enemy or a player projectile
        {
            if(collision.gameObject.tag == "Player" && !hasCollided) //if it has collided with player and hasnt collided yet
            {
                hasCollided = true;

                var playerHealth = collision.transform.Find("PlayerObject").GetComponent<PlayerHealth>(); //get player health component

                if (playerHealth != null) //if player health component exists
                {
                    playerHealth.GetComponent<PlayerHealth>().DamagePlayer(projectileDamage); //damage player
                }

                var impact = Instantiate(impactParticles, collision.contacts[0].point, Quaternion.identity) as GameObject; //create impact particles
                Destroy(impact, 2); //destroy impact particles after 2 seconds
                Destroy(gameObject); //destroy projectile
            }
            else
            {
                hasCollided = true;
                var impact = Instantiate(impactParticles, collision.contacts[0].point, Quaternion.identity) as GameObject; //create impact particles
                Destroy(impact, 2); //destroy impact particles after 2 seconds
                Destroy(gameObject); //destroy projectile
            }
        }
    }
}
