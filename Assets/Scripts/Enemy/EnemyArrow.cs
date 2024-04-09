using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : MonoBehaviour
{
    public float arrowLife = 3; //seconds an arrow will be alive for
    private bool hasCollided; //has arrow collided yet
    public int arrowDamage = 20; //damage of arrow

    private void Awake()
    {
        Destroy(gameObject, arrowLife); //destroy the arrow after a few seconds
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "EnemyArrow" && collision.gameObject.tag != "Enemy" && collision.gameObject.tag != "Arrow" && collision.gameObject.tag != "StaffProjectile" && collision.gameObject.tag != "AbilityProjectile" && !hasCollided) //if arrow hasnt hit itself, another enemy or player projectiles
        {
            if(collision.gameObject.tag == "Player" && !hasCollided) //if arrow hit player and hasnt collided yet
            {
                hasCollided = true; //arrow collided

                if(collision.gameObject.GetComponent<PlayerHealth>() != null) //if player health component exists
                {
                    collision.gameObject.GetComponent<PlayerHealth>().DamagePlayer(arrowDamage); //damage player
                }

                Destroy(gameObject); //destroy arrow
            }
            else
            {
                hasCollided = true; //arrow collided
                Destroy(gameObject); //destroy arrow
            }
        }
    }
}
