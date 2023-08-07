/*
 * Author: 
 * Date: 
 * Description: 
 */

using UnityEngine;
using UnityEngine.AI;

public class Crawler : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    private Transform Companion;
    public LayerMask whatIsGround, whatIsPlayer, whatIsNPC;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    public float timeBetweenAttacks;
    bool alreadyAttacked;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange, NPCInSightRange, NPCInAttackRange;

    public int health;
    public float dmg;
    private bool isDead = false;
    public Collider trigger;



    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        player = FindObjectOfType<NewBehaviourScript>().transform;
        if (isDead == false)
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            NPCInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsNPC);
            NPCInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsNPC);

            if ((!playerInSightRange && !playerInAttackRange) && (!NPCInSightRange && !NPCInAttackRange))
            {
                Patrolling();
            }
            else if ((playerInSightRange && !playerInAttackRange) && (!NPCInSightRange && !NPCInAttackRange))
            {
                ChasePlayer();
            }
            else if ((playerInSightRange && playerInAttackRange) && (!NPCInSightRange && !NPCInAttackRange))
            {
                AttackPlayer();
            }
            else if ((!playerInSightRange && !playerInAttackRange) && (NPCInSightRange && !NPCInAttackRange))
            {
                ChaseNPC();
            }
            else if ((playerInSightRange && !playerInAttackRange) && (NPCInSightRange && !NPCInAttackRange))
            {
                ChaseNPC();
            }
            else if ((playerInSightRange && playerInAttackRange) && (NPCInSightRange && !NPCInAttackRange))
            {
                ChaseNPC();
            }
            else if ((!playerInSightRange && !playerInAttackRange) && (NPCInSightRange && NPCInAttackRange))
            {
                AttackNPC();
            }
            else if ((playerInSightRange && !playerInAttackRange) && (NPCInSightRange && NPCInAttackRange))
            {
                AttackNPC();
            }
            else if ((playerInSightRange && playerInAttackRange) && (NPCInSightRange && NPCInAttackRange))
            {
                AttackNPC();
            }
        }
        else
        {
            Invoke(nameof(DestroyEnemy), 2.0f);
            return;
        }
    }

    /// <summary>
    /// Enemy are just walking and standing still
    /// </summary>
    private void Patrolling()
    {
        if (!walkPointSet)
        {
            Invoke(nameof(SearchWalkPoint), 3.0f);
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
            agent.speed = 2.0f;
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    /// <summary>
    /// Finding where the enemy could walk to
    /// </summary>
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    /// <summary>
    /// Move towards player if player come too close
    /// </summary>
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        agent.speed = 3.5f;
    }

    /// <summary>
    /// Attacks player if player is within range
    /// </summary>
    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            player.GetComponent<NewBehaviourScript>().Damage(dmg);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ChaseNPC()
    {
        Companion = FindObjectOfType<Companion>().transform;
        agent.SetDestination(Companion.position);
        agent.speed = 3.5f;
    }

    /// <summary>
    /// Attacks NPC if NPC is within range
    /// </summary>
    private void AttackNPC()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(Companion);

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    /// <summary>
    /// Zombie takes damage
    /// </summary>
    public void Damage(int dmg)
    {
        health -= dmg;
        if (health <= 0)
        {
            agent.speed = 0.0f;
            Died();
        }
    }

    /// <summary>
    /// Zombie dies when health is at 0
    /// </summary>
    private void Died()
    {
        isDead = true;
        trigger.enabled = false;
        player.GetComponent<NewBehaviourScript>().isStuck = false;

        if (Companion != null)
        {
            Companion.GetComponent<Companion>().agent.speed = 3.0f;
        }
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
