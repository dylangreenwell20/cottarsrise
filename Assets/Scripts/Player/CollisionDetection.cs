using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public WeaponController wc; //weapon controller being fetched
    //public GameObject HitParticle; //hit particle on collision

    private void OnTriggerEnter(Collider other) //on collision
    {
        if(other.tag == "Enemy" && wc.IsAttacking) //if collision is detected and player is attacking
        {
            //Debug.Log(other.name); //for testing collision
            other.GetComponent<Animator>().SetTrigger("Hit"); //trigger hit animation
            //Instantiate(HitParticle, new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z), other.transform.rotation); //spawn hit particle
        }
    }
}
