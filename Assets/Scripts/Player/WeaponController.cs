using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject Sword; //public GameObject Sword, Bow, Staff; //gets sword game object
    bool CanAttack = true; //boolean variable if player can attack or not
    float AttackCooldown = 0.75f; //attack cooldown
    public bool IsAttacking = false; //boolean variable if player is currently attacking
    //public GameObject Hitbox; //gets hitbox game object

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) //if left click pressed
        {
            if (CanAttack) //if player can attack
            {
                SwordAttack(); //attack function
            }
        }
    }
    public void SwordAttack()
    {
        //Hitbox.SetActive(true); //activate sword hitbox
        IsAttacking = true; //player is currently attacking
        CanAttack = false; //cannot currently attack
        Animator animator = Sword.GetComponent<Animator>(); //get animator
        animator.SetTrigger("Attack"); //trigger animation
        StartCoroutine(ResetAttackCooldown()); //reset attack cooldown
    }

    IEnumerator ResetAttackCooldown()
    {
        StartCoroutine(ResetAttackBoolean()); //reset attack boolean variable
        yield return new WaitForSeconds(AttackCooldown); //wait for cooldown
        //Hitbox.SetActive(false); //deactivate hitbox of sword
        CanAttack = true; //set CanAttack to true
        
    }

    IEnumerator ResetAttackBoolean()
    {
        yield return new WaitForSeconds(0.75f); //wait for 0.75 seconds
        IsAttacking = false; //player is not currently attacking
    }
}
