using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent nma; //enemy nav mesh agent
    private Transform playerTransform; //transform for the player

    public Vector3 walkPoint; //walk point for enemies when patroling
    private bool walkPointSet; //if walk point is set
    public float walkPointRange; //range the ai can generate a walk point from

    public float timeBetweenAttacks; //time between enemy attacks
    public bool isAttackingPlayer; //is enemy attacking player currently

    public float attackDistance; //distance the enemy can attack player from
    public bool playerInSight; //if player is actively seen by the enemy
    public bool playerInHearDistance; //player within hear distance
    public bool playerHeard; //player actually heard
    public bool playerInAttackRange; //is player in attack range or not

    public float viewDistance; //view distance of the enemy
    public float viewAngle; //angle the enemy can see (FOV)
    public float hearDistance; //distance an enemy can hear the player

    public LayerMask whatIsPlayer; //player layer
    public LayerMask whatIsObject; //object layer
    public LayerMask whatIsGround; //ground layer
    private GameObject player; //reference to player object

    public float patrolCooldownLower; //lower cooldown number for random time between enemy patrols
    public float patrolCooldownHigher; //higher cooldown number for random time between enemy patrols
    public bool cdActive; //patrol cooldown is active
    public float patrolCD; //cooldown between patrol points

    public int enemyDamage; //damage the enemy deals
    private bool isPlayerDead; //is player dead or alive

    public PlayerMovement pm; //reference to PlayerMovement script

    private CharacterStats characterStats; //reference to character stats script

    public bool isMelee, isRange, isMage, isBoss; //what attack style the enemy has

    public GameObject enemyArrow; //enemy arrow prefab
    public GameObject enemyMagic; //enemy magic projectile prefab

    public Transform arrowPoint; //spawn point of enemy arrow
    public Transform magicPoint; //spawn point of enemy magic

    private Ray ray; //ray towards player

    public Vector3 playerTarget; //direction towards the player

    public Animator animator; //enemy animator

    public HealthController healthController; //enemy health controller

    private void Awake()
    {
        animator = this.transform.parent.GetComponent<Animator>(); //get animator from parent
        healthController = this.GetComponent<HealthController>(); //get health controller component

        player = GameObject.Find("PlayerObject"); //find player game object
        playerTransform = player.transform; //find player transform
        nma = GetComponent<NavMeshAgent>(); //get nav mesh agent from enemy
        pm = GameObject.Find("Player").GetComponent<PlayerMovement>(); //get player movement component
        characterStats = GetComponent<CharacterStats>(); //get character stats
        enemyDamage = characterStats.damage.GetValue(); //get damage value

        if(isMelee) //if enemy is melee type
        {
            attackDistance = 1.5f;
            timeBetweenAttacks = 2.0f;
        }
        else if(isRange) //if enemy is range type
        {
            attackDistance = 5.0f;
            timeBetweenAttacks = 2.5f;
        }
        else if(isMage) //if enemy is mage type
        {
            attackDistance = 5.0f;
            timeBetweenAttacks = 2.0f;
        }
        else if (isBoss) //if enemy is boss type
        {
            attackDistance = 2.0f;
            timeBetweenAttacks = 3.0f;
        }
    }

    private void Update()
    {
        if (healthController.isDead) //if enemy is dead
        {
            return;
        }

        isPlayerDead = player.GetComponent<PlayerHealth>().isDead; //check if player is dead and assign the result to 'isPlayerDead' bool

        playerTarget = (player.transform.position - transform.position).normalized; //direction to player
        float distanceToTarget = Vector3.Distance(transform.position, player.transform.position); //calculate distance to the player
        playerInHearDistance = Physics.CheckSphere(transform.position, hearDistance, whatIsPlayer); //sphere around the enemy for the distance they can hear the player
        playerInAttackRange = Physics.CheckSphere(transform.position, attackDistance, whatIsPlayer); //sphere around the enemy for the distance they can attack the player from

        if (isBoss && !isPlayerDead) //if is boss enemy type and player is not dead, chase player
        {
            if(!isAttackingPlayer) //if is not attacking player
            {
                ChasePlayer();
            }

            return;
        }

        if (!playerInSight) //if player is not in sight
        {
            Patroling(); //patrol the area
        }

        if (!isPlayerDead) //if player is alive
        {
            if (distanceToTarget <= viewDistance) //if the distance from the player is less than or equal to the enemy's view distance
            {
                if (Vector3.Angle(transform.forward, playerTarget) < viewAngle / 2) //if player is in the view cone infront of the enemy
                {
                    if (Physics.Raycast(transform.position, playerTarget, distanceToTarget, whatIsObject) == false) //if the enemy has a clear raycast to the player (it is false because the statement checks for collision with objects in the environment)
                    {
                        playerInSight = true; //player is in sight
                        ChasePlayer(); //chase the player

                        //Debug.Log("ENEMY SEES YOU!!!"); //for testing
                    }
                }
                else if (playerInHearDistance) //if the player is not in the view cone but within hear distance
                {
                    ChasePlayer(); //chase the player

                    //Debug.Log("IN ENEMY HEAR RANGE!!!"); //for testing
                }
                else //if player is not in view distance
                {
                    NotInSight(); //player is not in sight of enemy
                }
            }
            else //if player is not in view distance
            {
                NotInSight(); //player is not in sight of enemy
            }
        }
        else //else if the player is dead
        {
            playerInSight = false; //player no longer in sight - this is for enemies that were attacking or chasing the player as the player died and this will make them patrol again
        }
    }

    private void NotInSight()
    {
        playerInSight = false; //player not in sight
        playerInAttackRange = false; //player not in attack range
    }

    private void Patroling()
    {
        if (!walkPointSet) //if no walk point is set
        {
            SearchWalkPoint(); //create a walk point
        }

        if (walkPointSet) //if walk point is set
        {
            nma.SetDestination(walkPoint); //set enemy destination to the walk point
            animator.SetTrigger("Walk"); //play walk animation
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint; //calculate distance to the walk point

        if (!cdActive)
        {
            if (distanceToWalkPoint.magnitude < 1f) //if enemy has reached the walk point
            {
                //if cd isnt active then check distance 
                //if distance is <1f then generate random time - put cdActive to true then after a count down move the enemy

                animator.SetTrigger("StopWalk"); //stop walking animation - go back to idle

                cdActive = true; //set patrol walkpoint cooldown to true
                patrolCD = Random.Range(patrolCooldownLower, patrolCooldownHigher); //pick a random number between the cooldown times

                //Debug.Log("cd is active"); //for testing
                //Debug.Log(patrolCD); //for testing
            }
        }
        else //if cooldown is active
        {
            patrolCD -= Time.deltaTime; //count down from the random cooldown number
            if (patrolCD <= 0) //if the cooldown is less than or equal to 0
            {
                cdActive = false; //cooldown is no longer active
                ResetPatrol(); //prepare to get another walk point
            }
        }
        
    }

    private void SearchWalkPoint()
    {
        float randomX = Random.Range(-walkPointRange, walkPointRange); //random x value for a walk point
        float randomZ = Random.Range(-walkPointRange, walkPointRange); //random z value for a walk point

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ); //create new walk point using random X and Z values previously calculated

        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) //check if walk point is on walkable surface
        {
            walkPointSet = true; //walk point has been created successfully
        }
    }

    private void ResetPatrol()
    {
        walkPointSet = false; //walkPointSet is false so a new patrol point will be searched for
    }

    private void ChasePlayer()
    {
        if(!isAttackingPlayer)
        {
            animator.SetTrigger("Chase"); //play chase animation

            nma.SetDestination(playerTransform.position); //set destination to the player's location

            if (playerInAttackRange) //if player is in attack range
            {
                AttackPlayer(); //attack player
            }
        }
    }

    private void AttackPlayer()
    {
        nma.SetDestination(transform.position); //stop enemy from moving

        transform.LookAt(new Vector3(playerTransform.position.x, 0.0f, playerTransform.position.z)); //look at player but at their own Y level to stop looking up

        if (!isAttackingPlayer) //if isnt currently attacking the player
        {
            if (isMelee)
            {
                //play attack animation

                animator.SetTrigger("Attack"); //play attack animation

                //attack player

                player.GetComponent<PlayerHealth>().DamagePlayer(enemyDamage);

                //Debug.Log("melee attack"); //for testing

                isAttackingPlayer = true; //enemy currently attacking player
                Invoke(nameof(ResetAttack), timeBetweenAttacks); //attack cooldown
            }
            else if (isRange)
            {
                //play attack animation

                animator.SetTrigger("StopChase");

                //spawn projectile and send towards player

                var rangeProjectile = Instantiate(enemyArrow, arrowPoint.position, transform.rotation) as GameObject; //create arrow
                rangeProjectile.GetComponent<Rigidbody>().AddForce(arrowPoint.forward * 12);

                //Debug.Log("range attack"); //for testing

                isAttackingPlayer = true; //enemy currently attacking player
                Invoke(nameof(ResetAttack), timeBetweenAttacks); //attack cooldown
            }
            else if (isMage)
            {
                //play attack animation

                animator.SetTrigger("StopChase");

                //spawn projectile and send towards player

                var mageProjectile = Instantiate(enemyMagic, magicPoint.position, Quaternion.identity) as GameObject; //create mage projectile
                mageProjectile.GetComponent<Rigidbody>().AddForce(magicPoint.forward * 12);

                //Debug.Log("mage attack"); //for testing

                isAttackingPlayer = true; //enemy currently attacking player
                Invoke(nameof(ResetAttack), timeBetweenAttacks); //attack cooldown
            }
            else if (isBoss)
            {
                //pick an attack randomly

                int attack = Random.Range(0, 3); //random number from 0 - 2

                if(attack == 0) //melee attack
                {
                    //play attack animation

                    animator.SetTrigger("BossMelee");

                    //simple melee hit

                    player.GetComponent<PlayerHealth>().DamagePlayer(enemyDamage);

                    //Debug.Log("boss melee"); //for testing

                    isAttackingPlayer = true; //enemy currently attacking player
                    Invoke(nameof(ResetAttack), timeBetweenAttacks); //attack cooldown
                }
                else if(attack == 1) //kick attack
                {
                    //play attack animation

                    animator.SetTrigger("BossKick");

                    //apply force to player rigidbody and send them away from the boss

                    player.GetComponent<PlayerHealth>().DamagePlayer(enemyDamage);

                    player.transform.parent.GetComponent<Rigidbody>().AddForce((transform.forward) * 100, ForceMode.Impulse); //launch player backwards

                    //Debug.Log("boss kick"); //for testing

                    isAttackingPlayer = true; //enemy currently attacking player
                    Invoke(nameof(ResetAttack), timeBetweenAttacks); //attack cooldown
                }
                else if(attack == 2) //jump attack
                {
                    //play attack animation

                    animator.SetTrigger("BossJump");

                    //wait until animation over then check if player is grounded - if yes then damage

                    isAttackingPlayer = true; //enemy currently attacking player

                    Invoke(nameof(JumpAttack), 1.5f);

                    //Debug.Log("boss jump"); //for testing

                    Invoke(nameof(ResetAttack), timeBetweenAttacks); //attack cooldown
                }
            }
            else
            {
                Debug.Log("no enemy class - bug");
            }
        }
    }

    private void ResetAttack()
    {
        isAttackingPlayer = false; //enemy can attack again

        animator.SetTrigger("BossChase");
    }

    private void JumpAttack()
    {
        if (pm.grounded) //if player is on the ground
        {
            player.GetComponent<PlayerHealth>().DamagePlayer(enemyDamage); //damage player
        }
    }
}
