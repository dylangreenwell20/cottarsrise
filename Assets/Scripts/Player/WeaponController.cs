using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject Sword; //public GameObject Sword, Bow, Staff; //gets sword game object
    bool CanAttack = true; //boolean variable if player can attack or not
    float SwordAttackCooldown = 0.75f; //sword attack cooldown
    float BowAttackCooldown = 0.6f; //bow attack cooldown
    float StaffAttackCooldown = 0.5f; //staff attack cooldown
    public bool IsAttacking = false; //boolean variable if player is currently attacking
    //public GameObject Hitbox; //gets hitbox game object

    public SelectedWeapon sW; //link to the SelectedWeapon script to see what weapons are currently selected

    public Camera cam; //camera for staff projectile
    private Vector3 destination; //for staff projectile destination
    public GameObject projectile; //particle effect of the projectile
    public Transform firePoint; //location of the projectile to be shot from
    public float projectileSpeed = 30; //speed of projectile

    private void Update()
    {

        if (Input.GetMouseButton(0) && sW.swordActive) //if left click pressed and sword is equipped
        {
            if (CanAttack) //if player can attack
            {
                SwordAttack(); //attack function
            }
        }

        if (Input.GetMouseButton(0) && sW.bowActive) //if left click pressed and bow is equipped
        {
            if (CanAttack) //if player can attack
            {
                BowAttack(); //attack function
            }
        }

        if (Input.GetMouseButton(0) && sW.staffActive) //if left click pressed and staff is equipped
        {
            if (CanAttack) //if player can attack
            {
                StaffAttack(); //attack function
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
        StartCoroutine(ResetAttackCooldown(SwordAttackCooldown)); //reset attack cooldown using sword attack cooldown time
    }

    public void BowAttack()
    {

    }

    public void StaffAttack()
    {
        IsAttacking = true; //player is currently attacking
        CanAttack = false; //player cannot currently attack

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //create a ray from the camera where the player is looking
        RaycastHit hit; //create new raycasthit variable

        if(Physics.Raycast(ray, out hit)) //if raycast hit was successful
        {
            destination = hit.point; //destination set to the raycast hit point
        }
        else //if nothing was hit
        {
            destination = ray.GetPoint(1000); //send projectile a distance of 1000                           MAY NEED TO CHANGE THISSSSSSSSS
        }

        InstantiateProjectile(firePoint); //create projectile function

        StartCoroutine(ResetAttackCooldown(StaffAttackCooldown)); //reset attack cooldown using staff attack cooldown time
    }

    public void InstantiateProjectile(Transform firePoint)
    {
        var projectileObject = Instantiate(projectile, firePoint.position, Quaternion.identity) as GameObject; //create the projectile as a game object
        projectileObject.GetComponent<Rigidbody>().velocity = (destination - firePoint.position).normalized * projectileSpeed;
    }

    IEnumerator ResetAttackCooldown(float AttackCD)
    {
        StartCoroutine(ResetAttackBoolean(AttackCD)); //reset attack boolean variable
        yield return new WaitForSeconds(AttackCD); //wait for cooldown
        CanAttack = true; //set CanAttack to true
        
    }

    IEnumerator ResetAttackBoolean(float AttackCD)
    {
        yield return new WaitForSeconds(AttackCD); //wait for cooldown time
        IsAttacking = false; //player is not currently attacking
    }
}
