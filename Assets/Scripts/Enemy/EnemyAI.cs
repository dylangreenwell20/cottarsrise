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
    private bool isAttackingPlayer; //is enemy attacking player currently

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

    private void Awake()
    {
        player = GameObject.Find("PlayerObject"); //find player game object
        playerTransform = player.transform; //find player transform
        nma = GetComponent<NavMeshAgent>(); //get nav mesh agent from enemy
        characterStats = GetComponent<CharacterStats>(); //get character stats
        enemyDamage = characterStats.damage.GetValue(); //get damage value
    }

    private void Update()
    {
        Vector3 playerTarget = (player.transform.position - transform.position).normalized; //direction to player
        float distanceToTarget = Vector3.Distance(transform.position, player.transform.position); //calculate distance to the player
        playerInHearDistance = Physics.CheckSphere(transform.position, hearDistance, whatIsPlayer); //sphere around the enemy for the distance they can hear the player
        playerInAttackRange = Physics.CheckSphere(transform.position, attackDistance, whatIsPlayer); //sphere around the enemy for the distance they can attack the player from

        if (!playerInSight) //if player is not in sight
        {
            Patroling(); //patrol the area
        }

        isPlayerDead = player.GetComponent<PlayerHealth>().isDead; //check if player is dead and assign the result to 'isPlayerDead' bool
        
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

                        Debug.Log("ENEMY SEES YOU!!!"); //for testing
                    }
                }
                else if (playerInHearDistance) //if the player is not in the view cone but within hear distance
                {
                    bool isStealthing = pm.isStealthing; //check if player is stealthing or not
                    if (!isStealthing) //if player is not stealthing (so if they are either walking or sprinting)
                    {
                        if(pm.isMoving) //if player is moving and making noise
                        {
                            ChasePlayer(); //chase the player
                        }
                    }

                    Debug.Log("IN ENEMY HEAR RANGE!!!"); //for testing
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
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint; //calculate distance to the walk point

        if (!cdActive)
        {
            if (distanceToWalkPoint.magnitude < 1f) //if enemy has reached the walk point
            {
                //if cd isnt active then check distance 
                //if distance is <1f then generate random time - put cdActive to true then after a count down move the enemy

                cdActive = true; //set patrol walkpoint cooldown to true
                patrolCD = Random.Range(patrolCooldownLower, patrolCooldownHigher); //pick a random number between the cooldown times
                Debug.Log("cd is active"); //for testing
                Debug.Log(patrolCD); //for testing
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
        nma.SetDestination(playerTransform.position); //set destination to the player's location

        if (playerInAttackRange) //if player is in attack range
        {
            AttackPlayer(); //attack player
        }
    }

    private void AttackPlayer()
    {
        nma.SetDestination(transform.position); //stop enemy from moving
        transform.LookAt(playerTransform); //look at player

        if (!isAttackingPlayer) //if isnt currently attacking the player
        {
            player.GetComponent<PlayerHealth>().DamagePlayer(enemyDamage);
            //ENEMY ATTACK CODE HERE - DO MELEE FIRST

            //if isRange then range attack
            //if isMage then mage attack
            //if isMelee then melee attack

            Debug.Log("YOU HAVE BEEN ATTACKED!!!"); //for testing

            isAttackingPlayer = true; //enemy currently attacking player
            Invoke(nameof(ResetAttack), timeBetweenAttacks); //attack cooldown
        }
    }

    private void ResetAttack()
    {
        isAttackingPlayer = false; //enemy can attack again
    }
}
