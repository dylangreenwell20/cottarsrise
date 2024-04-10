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

    public bool isMelee, isRange, isMage; //what attack style the enemy has

    public GameObject enemyArrow; //enemy arrow prefab
    public GameObject enemyMagic; //enemy magic projectile prefab

    public Transform arrowPoint; //spawn point of enemy arrow
    public Transform magicPoint; //spawn point of enemy magic

    private Ray ray; //ray towards player

    public Vector3 playerTarget; //direction towards the player

    private void Awake()
    {
        player = GameObject.Find("PlayerObject"); //find player game object
        playerTransform = player.transform; //find player transform
        nma = GetComponent<NavMeshAgent>(); //get nav mesh agent from enemy
        pm = GameObject.Find("Player").GetComponent<PlayerMovement>(); //get player movement component
        characterStats = GetComponent<CharacterStats>(); //get character stats
        enemyDamage = characterStats.damage.GetValue(); //get damage value

        if(isMelee) //if enemy is melee type
        {
            attackDistance = 1.5f;
            timeBetweenAttacks = 1.0f;
        }
        else if(isRange) //if enemy is range type
        {
            attackDistance = 8.0f;
            timeBetweenAttacks = 1.5f;
        }
        else if(isMage) //if enemy is mage type
        {
            attackDistance = 10.0f;
            timeBetweenAttacks = 2.0f;
        }
    }

    private void Update()
    {
        playerTarget = (player.transform.position - transform.position).normalized; //direction to player
        float distanceToTarget = Vector3.Distance(transform.position, player.transform.position); //calculate distance to the player
        playerInHearDistance = Physics.CheckSphere(transform.position, hearDistance, whatIsPlayer); //sphere around the enemy for the distance they can hear the player
        playerInAttackRange = Physics.CheckSphere(transform.position, attackDistance, whatIsPlayer); //sphere around the enemy for the distance they can attack the player from

        //orientation.rotation = Quaternion.Euler(0, yRotation, 0); //rotating the orientation

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
            if (isMelee)
            {
                //play attack animation




                //attack player

                transform.LookAt(player.transform); //look at player

                player.GetComponent<PlayerHealth>().DamagePlayer(enemyDamage);

                Debug.Log("melee attack"); //for testing

                isAttackingPlayer = true; //enemy currently attacking player
                Invoke(nameof(ResetAttack), timeBetweenAttacks); //attack cooldown
            }
            else if (isRange)
            {
                //play attack animation



                //spawn projectile and send towards player

                transform.LookAt(player.transform); //look at player

                var rangeProjectile = Instantiate(enemyArrow, arrowPoint.position, transform.rotation) as GameObject; //create arrow
                rangeProjectile.GetComponent<Rigidbody>().AddForce(arrowPoint.forward * 15);

                Debug.Log("range attack"); //for testing

                isAttackingPlayer = true; //enemy currently attacking player
                Invoke(nameof(ResetAttack), timeBetweenAttacks); //attack cooldown
            }
            else if (isMage)
            {
                //play attack animation



                //spawn projectile and send towards player

                transform.LookAt(player.transform); //look at player

                var mageProjectile = Instantiate(enemyMagic, magicPoint.position, Quaternion.identity) as GameObject; //create mage projectile
                mageProjectile.GetComponent<Rigidbody>().AddForce(magicPoint.forward * 15);

                Debug.Log("mage attack"); //for testing

                isAttackingPlayer = true; //enemy currently attacking player
                Invoke(nameof(ResetAttack), timeBetweenAttacks); //attack cooldown
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
    }
}
