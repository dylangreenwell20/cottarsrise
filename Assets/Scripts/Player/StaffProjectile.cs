using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffProjectile : MonoBehaviour
{
    public GameObject impactParticles; //projectile impact particle effect
    private bool hasCollided; //if projectile has collided or not

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "StaffProjectile" && collision.gameObject.tag != "Player" && !hasCollided) //if projectile collides with something other than itself, the player and has not already collided
        {
            hasCollided = true; //has collided set to true
            var impact = Instantiate(impactParticles, collision.contacts[0].point, Quaternion.identity) as GameObject; //create impact particles on first collision point
            Destroy(impact, 2); //destroy impact particles after 2 seconds
            Destroy(gameObject); //destroy the projectile
        }
    }
}
