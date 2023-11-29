using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject Sword; //public GameObject Sword, Bow, Staff; //gets sword game object
    bool CanAttack = true; //boolean variable if player can attack or not
    float SwordAttackCooldown = 1f; //sword attack cooldown
    float BowAttackCooldown = 0.5f; //bow attack cooldown
    float StaffAttackCooldown = 0.5f; //staff attack cooldown
    public bool IsAttacking = false; //boolean variable if player is currently attacking
    //public GameObject Hitbox; //gets hitbox game object

    public SelectedWeapon sW; //link to the SelectedWeapon script to see what weapons are currently selected

    public Camera cam; //camera for staff projectile
    private Vector3 destination; //for staff projectile destination
    public GameObject projectile; //particle effect of the projectile
    public Transform firePoint; //location of the projectile to be shot from
    public float projectileSpeed = 30; //speed of projectile

    public GameObject arrow; //arrow object
    public float arrowForce; //arrow force
    public float reloadTime; //reload time for bow
    public Transform arrowFirePoint; //arrow fire point
    public GameObject modelArrow; //default arrow model in bow

    public float swordDamage = 25; //damage value for sword
    public LayerMask enemyLayer; //layer for attackable enemies
    public float swordRange = 3f; //distance the sword can attack
    public Transform attackPoint; //point where hit reg is calculated from sword

    public float manaCost; //mana cost of the staff attack
    public float currentMana; //current mana the player has
    public GameObject player; //reference to the player

    public float arrowCost; //cost to shoot an arrow - useful for abilities that may cost multiple arrows or just firing normally which costs 1
    public bool noArrows; //if player has no arrows left

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
                if(!noArrows) //if player has no arrows left
                {
                    BowAttack(); //attack function
                } 
            }
        }

        if (Input.GetMouseButton(0) && sW.staffActive) //if left click pressed and staff is equipped
        {
            if (CanAttack) //if player can attack
            {
                currentMana = player.gameObject.GetComponent<PlayerMana>().currentMana; //get current mana value
                //Debug.Log(currentMana); //for testing

                if(currentMana >=  manaCost) //check if player has more mana than the staff requires
                {
                    StaffAttack(); //attack function
                }
            }
        }

        if (sW.bowActive) //if bow is active
        {
            if(CanAttack && !IsAttacking) //if player can attack and is not currently attacking
            {
                noArrows = player.gameObject.GetComponent<ArrowCounter>().noArrows;

                if (!noArrows)
                {
                    modelArrow.SetActive(true); //enable the model arrow in the bow
                }
            }
        }
    }
    public void SwordAttack()
    {
        IsAttacking = true; //player is currently attacking
        CanAttack = false; //cannot currently attack
        Animator animator = Sword.GetComponent<Animator>(); //get animator
        animator.SetTrigger("Attack"); //trigger animation

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, swordRange, enemyLayer); //store all hit enemies in an array as multiple enemies can be hit at once

        foreach(Collider enemy in hitEnemies) //for each enemy in the hitEnemies array
        {
            enemy.GetComponent<HealthController>().ApplyDamage(swordDamage);
            //Debug.Log("Enemy hit: " + enemy.name); //print out who was hit for testing
        }

        StartCoroutine(ResetAttackCooldown(SwordAttackCooldown)); //reset attack cooldown using sword attack cooldown time
    }

    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null) //if no attack point exists
        {
            return; //return function
        }
        Gizmos.DrawWireSphere(attackPoint.position, swordRange); //draw a wire sphere based on the attack range - used for testing to see the actual attack range of the sword
    }

    public void BowAttack()
    {
        IsAttacking = true; //player is currently attacking
        CanAttack = false; //player cannot currently attack

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); //create a ray from the camera where the player is looking
        RaycastHit hit; //create new raycast hit variable
        Vector3 destination; //destination of where the arrow will go

        if(Physics.Raycast(ray, out hit)) //if raycast hit something
        {
            destination = hit.point; //send projectile to the point hit on raycast
        }
        else //if raycast did not hit anything
        {
            destination = ray.GetPoint(75); //send projectile a distance of 75
        }

        Vector3 angleOfDirection = destination - arrowFirePoint.position; //angle to shoot the arrow towards the target destination

        modelArrow.SetActive(false); //disable the model arrow

        GameObject currentArrow = Instantiate(arrow, arrowFirePoint.position, Quaternion.identity); //create an arrow
        currentArrow.transform.forward = angleOfDirection.normalized; //rotate arrow to fire where the player is aiming

        currentArrow.GetComponent<Rigidbody>().AddForce(angleOfDirection.normalized * arrowForce, ForceMode.Impulse); //force of the arrow

        player.gameObject.GetComponent<ArrowCounter>().LoseArrow(1); //take away an arrow from the player's current arrow count

        StartCoroutine(ResetAttackCooldown(BowAttackCooldown)); //reset attack cooldown using bow attack cooldown time
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
            destination = ray.GetPoint(75); //send projectile a distance of 75                             MAY NEED TO CHANGE THE VALUE
        }

        InstantiateProjectile(firePoint); //create projectile function

        player.gameObject.GetComponent<PlayerMana>().LoseMana(manaCost); //take away mana cost of the staff from the player's mana value
        //Debug.Log(currentMana); //for testing

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
