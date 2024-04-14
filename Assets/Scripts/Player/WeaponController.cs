using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public GameObject sword, bow, staff; //create variables for sword, bow and staff

    bool CanAttack = true; //boolean variable if player can attack or not
    float SwordAttackCooldown = 1f; //sword attack cooldown
    float BowAttackCooldown = 0.5f; //bow attack cooldown
    float StaffAttackCooldown = 0.5f; //staff attack cooldown
    public bool IsAttacking = false; //boolean variable if player is currently attacking
    //public GameObject Hitbox; //gets hitbox game object

    public SelectedWeapon sW; //link to the SelectedWeapon script to see what weapons are currently selected

    public Camera cam; //camera for bow and staff projectiles
    private Vector3 destination; //for staff projectile destination
    public GameObject projectile; //particle effect of the projectile
    public Transform staffFirePoint; //location of the projectile to be shot from
    public float projectileSpeed = 30; //speed of projectile

    public GameObject arrow; //arrow object
    public float arrowForce; //arrow force
    public float reloadTime; //reload time for bow
    public Transform arrowFirePoint; //arrow fire point
    public GameObject modelArrow; //default arrow model in bow

    public LayerMask enemyLayer; //layer for attackable enemies
    public float swordRange = 3f; //distance the sword can attack
    public Transform attackPoint; //point where hit reg is calculated from sword

    public int manaCost = 10; //mana cost of the staff attack
    public float currentMana; //current mana the player has
    public GameObject player; //reference to the player

    public float arrowCost; //cost to shoot an arrow - useful for abilities that may cost multiple arrows or just firing normally which costs 1
    public bool noArrows; //if player has no arrows left

    public Inventory inventory; //reference to inventory script
    private bool inventoryOpen; //if inventory is open or not

    public PlayerStats playerStats; //reference to player stats script

    public bool swordFound; //bool to see if sword was found or not
    public bool bowFound; //bool to see if bow was found or not
    public bool staffFound; //bool to see if staff was found or not

    public EquipmentManager eM; //reference to equipment manager

    public Equipment currentWeapon; //current weapon the player has equipped

    private void Update()
    {
        currentWeapon = eM.currentEquipment[4]; //get current weapon

        if (StartingWeapon.warriorClassSelected) //if warrior class selected
        {
            if (sW.weaponFound == false) //if sword has not been found
            {
                if(currentWeapon != null)
                {
                    //get name of weapon

                    string weaponName = (currentWeapon.name + "(Clone)"); //get name to game can search for clone prefab

                    //Debug.Log(weaponName); //for testing

                    sword = sW.meleePosition.Find(weaponName).gameObject; //find weapon game object and store to variable

                    if (sword != null) //if sword was found
                    {
                        sW.weaponFound = true; //sword set to true

                        //Debug.Log("Sword found"); //for testing

                        attackPoint = sword.transform.Find("AttackPoint"); //find attack point of sword and store to variable

                        return; //return function
                    }
                }
            }
        }

        if (StartingWeapon.archerClassSelected) //if archer class selected
        {
            if (sW.weaponFound == false) //if bow hasnt been found
            {
                if(currentWeapon != null)
                {
                    string weaponName = (currentWeapon.name + "(Clone)"); //get name to game can search for clone prefab

                    //Debug.Log(weaponName); //for testing

                    bow = sW.rangePosition.Find(weaponName).gameObject; //find bow
                    Transform bowTransform = bow.transform; //create bow transform variable - used to find child game objects of the bow
                    Transform arrowTransform = bowTransform.gameObject.transform.Find("Arrow"); //find arrow transform
                    modelArrow = arrowTransform.gameObject; //set modelArrow to arrow transform of bow

                    arrowFirePoint = bowTransform.transform.Find("FirePoint"); //find arrowFirePoint from bow transform

                    if (bow != null) //if bow has been found
                    {
                        sW.weaponFound = true; //bow has been found

                        //Debug.Log("Bow found"); //for testing

                        return; //return function
                    }
                }
            }
        }

        if (StartingWeapon.mageClassSelected) //if mage class selected
        {
            if (sW.weaponFound == false) //if staff hasnt been found
            {
                if(currentWeapon != null)
                {
                    string weaponName = (currentWeapon.name + "(Clone)"); //get name to game can search for clone prefab

                    //Debug.Log(weaponName); //for testing

                    staff = sW.magePosition.Find(weaponName).gameObject; //find staff
                    Transform staffTransform = staff.transform; //create transform of staff game object
                    staffFirePoint = staffTransform.transform.Find("FirePoint"); //find staffFirePoint from staff transform

                    if (staff != null) //if staff has been found
                    {
                        sW.weaponFound = true; //staff was found

                        //Debug.Log("Staff found"); //for testing

                        return; //return function
                    }
                }
            }
        }

        inventoryOpen = inventory.inventoryOpen; //get status of whether inventory is open or closed

        if (inventoryOpen) //if inventory is open
        {
            return; //return function so player will not attack when clicking in inventory
        }

        if (Input.GetMouseButton(0) && sW.swordActive && currentWeapon != null) //if left click pressed and sword is equipped
        {
            if (CanAttack) //if player can attack
            {
                SwordAttack(); //attack function
            }
        }

        if (Input.GetMouseButton(0) && sW.bowActive && currentWeapon != null) //if left click pressed and bow is equipped
        {
            if (CanAttack) //if player can attack
            {
                if(!noArrows) //if player has no arrows left
                {
                    BowAttack(); //attack function
                } 
            }
        }

        if (Input.GetMouseButton(0) && sW.staffActive && currentWeapon != null) //if left click pressed and staff is equipped
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

        if (sW.bowActive && currentWeapon != null) //if bow is active
        {
            
            if(CanAttack && !IsAttacking) //if player can attack and is not currently attacking
            {
                noArrows = player.gameObject.GetComponent<ArrowCounter>().noArrows; //check state of player having arrows left in quiver

                if (!noArrows && currentWeapon) //if player has arrows left
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

        Equipment currentWeapon = eM.currentEquipment[4];

        //the code below is to fix a bug where when restarting the game, the sword will be null for some reason

        if (sword == null)
        {
            //get name of weapon

            string weaponName = (currentWeapon.name + "(Clone)"); //get name to game can search for clone prefab

            //Debug.Log(weaponName); //for testing

            sword = sW.meleePosition.Find(weaponName).gameObject; //find weapon game object and store to variable

            if (sword != null) //if sword was found
            {
                sW.weaponFound = true; //sword set to true

                //Debug.Log("Sword found"); //for testing

                attackPoint = sword.transform.Find("AttackPoint"); //find attack point of sword and store to variable
            }
        }

        Animator animator = sword.GetComponent<Animator>(); //get animator
        animator.SetTrigger("Attack"); //trigger animation

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, swordRange, enemyLayer); //store all hit enemies in an array as multiple enemies can be hit at once
        
        int damage = currentWeapon.damage;
        //Debug.Log(damage);

        int damageToDeal = playerStats.DamageToDeal(damage); //get damage value with gear modifiers applied

        foreach(Collider enemy in hitEnemies) //for each enemy in the hitEnemies array
        {
            enemy.GetComponent<HealthController>().ApplyDamage(damageToDeal);

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

        if (bow == null)
        {
            string weaponName = (currentWeapon.name + "(Clone)"); //get name to game can search for clone prefab

            //Debug.Log(weaponName); //for testing

            bow = sW.rangePosition.Find(weaponName).gameObject; //find bow
            Transform bowTransform = bow.transform; //create bow transform variable - used to find child game objects of the bow
            Transform arrowTransform = bowTransform.gameObject.transform.Find("Arrow"); //find arrow transform
            modelArrow = arrowTransform.gameObject; //set modelArrow to arrow transform of bow

            arrowFirePoint = bowTransform.transform.Find("FirePoint"); //find arrowFirePoint from bow transform

            if (bow != null) //if bow has been found
            {
                sW.weaponFound = true; //bow has been found

                //Debug.Log("Bow found"); //for testing

                return; //return function
            }
        }

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

        if(modelArrow != null) //if model arrow exists
        {
            modelArrow.SetActive(false); //disable the model arrow
        }

        GameObject currentArrow = Instantiate(arrow, arrowFirePoint.position, Quaternion.identity); //create an arrow
        currentArrow.transform.forward = angleOfDirection.normalized; //rotate arrow to fire where the player is aiming

        currentArrow.GetComponent<Rigidbody>().AddForce(angleOfDirection.normalized * arrowForce, ForceMode.Impulse); //force of the arrow

        player.gameObject.GetComponent<ArrowCounter>().LoseArrow(1); //take away an arrow from the player's current arrow count

        StartCoroutine(ResetAttackCooldown(BowAttackCooldown)); //reset attack cooldown using bow attack cooldown time

        noArrows = player.gameObject.GetComponent<ArrowCounter>().noArrows; //check state of player having arrows left in quiver

        if (noArrows) //if player has no arrows left
        {
            return; //return attack function

            //the purpose of this function is to fix a bug where the bow would keep shooting arrows if the player had the attack button held down
            //and the game would only check if the player had arrows left as they clicked, so the player would be able to keep shooting the bow
            //even if they had 0 arrows left
        }
    }

    public void StaffAttack()
    {
        IsAttacking = true; //player is currently attacking
        CanAttack = false; //player cannot currently attack

        if (staff == null)
        {
            string weaponName = (currentWeapon.name + "(Clone)"); //get name to game can search for clone prefab

            //Debug.Log(weaponName); //for testing

            staff = sW.magePosition.Find(weaponName).gameObject; //find staff
            Transform staffTransform = staff.transform; //create transform of staff game object
            staffFirePoint = staffTransform.transform.Find("FirePoint"); //find staffFirePoint from staff transform

            if (staff != null) //if staff has been found
            {
                sW.weaponFound = true; //staff was found

                //Debug.Log("Staff found"); //for testing

                return; //return function
            }
        }

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

        InstantiateProjectile(staffFirePoint); //create projectile function

        player.gameObject.GetComponent<PlayerMana>().LoseMana(manaCost); //take away mana cost of the staff from the player's mana value
        //Debug.Log(currentMana); //for testing

        StartCoroutine(ResetAttackCooldown(StaffAttackCooldown)); //reset attack cooldown using staff attack cooldown time
    }

    public void InstantiateProjectile(Transform staffFirePoint)
    {
        var projectileObject = Instantiate(projectile, staffFirePoint.position, Quaternion.identity) as GameObject; //create the projectile as a game object
        projectileObject.GetComponent<Rigidbody>().velocity = (destination - staffFirePoint.position).normalized * projectileSpeed;
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
