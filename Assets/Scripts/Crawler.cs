/*
 * Author: 
 * Date: 
 * Description: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Crawler : MonoBehaviour
{
    private string currentState;
    private string nextState;

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

    public AudioSource death;
    public AudioSource atk;
    public AudioSource hit;


    private void SwitchState()
    {
        StartCoroutine(currentState);
    }

    IEnumerator Idle()
    {
        if (!isDead)
        {
            while (currentState == "Idle")
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


                if ((playerInSightRange && !playerInAttackRange) && (!NPCInSightRange && !NPCInAttackRange))
                {
                    nextState = "Chase";
                }
                else if ((!playerInSightRange && !playerInAttackRange) && (NPCInSightRange && !NPCInAttackRange))
                {
                    nextState = "ChaseN";
                }
                else if ((playerInSightRange && !playerInAttackRange) && (NPCInSightRange && !NPCInAttackRange))
                {
                    nextState = "ChaseN";
                }
                else if ((playerInSightRange && playerInAttackRange) && (NPCInSightRange && !NPCInAttackRange))
                {
                    nextState = "ChaseN";
                }
                yield return new WaitForEndOfFrame();
            }
            SwitchState();
        }
    }

    IEnumerator Chase()
    {
        if (!isDead)
        {
            agent.speed = 3.5f;

            bool isChasing = true;

            while (isChasing)
            {
                agent.SetDestination(player.position);

                // some example of how the switching will work 
                if ((!playerInSightRange && !playerInAttackRange) && (!NPCInSightRange && !NPCInAttackRange))
                {
                    nextState = "Idle";
                    isChasing = false;
                }
                else if ((playerInSightRange && playerInAttackRange) && (!NPCInSightRange && !NPCInAttackRange))
                {
                    nextState = "Atk";
                    isChasing = false;
                }
                else if ((playerInSightRange && !playerInAttackRange) && (NPCInSightRange && !NPCInAttackRange))
                {
                    nextState = "ChaseN";
                    isChasing = false;
                }

                yield return new WaitForEndOfFrame();
            }
            SwitchState();
        }
    }

    IEnumerator Atk()
    {
        if (!isDead)
        {
            transform.LookAt(player);

            bool isAtk = true;

            while (isAtk)
            {
                agent.SetDestination(transform.position);
                if (!alreadyAttacked)
                {
                    player.GetComponent<NewBehaviourScript>().Damage(dmg);
                    alreadyAttacked = true;
                    atk.Play();
                    Invoke(nameof(ResetAttack), timeBetweenAttacks);
                }

                if ((playerInSightRange && !playerInAttackRange) && (!NPCInSightRange && !NPCInAttackRange))
                {
                    nextState = "Chase";
                    isAtk = false;
                }
                yield return new WaitForEndOfFrame();
            }
            SwitchState();
        }
    }

    IEnumerator ChaseN()
    {
        if (!isDead)
        {
            agent.speed = 3.5f;

            bool isChasing = true;

            while (isChasing)
            {
                Companion = FindObjectOfType<Companion>().transform;
                agent.SetDestination(Companion.position);

                // some example of how the switching will work 
                if ((!playerInSightRange && !playerInAttackRange) && (NPCInSightRange && NPCInAttackRange))
                {
                    nextState = "AtkN";
                    isChasing = false;
                }
                else if ((playerInSightRange && !playerInAttackRange) && (NPCInSightRange && NPCInAttackRange))
                {
                    nextState = "AtkN";
                    isChasing = false;
                }
                else if ((playerInSightRange && playerInAttackRange) && (NPCInSightRange && NPCInAttackRange))
                {
                    nextState = "AtkN";
                    isChasing = false;
                }
                else if ((!playerInSightRange && !playerInAttackRange) && (!NPCInSightRange && !NPCInAttackRange))
                {
                    nextState = "Idle";
                    isChasing = false;
                }

                yield return new WaitForEndOfFrame();
            }
            SwitchState();
        }
    }

    IEnumerator AtkN()
    {
        if (!isDead)
        {
            transform.LookAt(Companion);

            bool isAtk = true;

            while (isAtk)
            {
                agent.SetDestination(transform.position);
                if (!alreadyAttacked)
                {
                    alreadyAttacked = true;
                    atk.Play();
                    Invoke(nameof(ResetAttack), timeBetweenAttacks);
                }

                if ((!playerInSightRange && !playerInAttackRange) && (!NPCInSightRange && !NPCInAttackRange))
                {
                    nextState = "Idle";
                    isAtk = false;
                }
                yield return new WaitForEndOfFrame();
            }
            SwitchState();
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
        hit.Play();
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
        death.Play();
        if (Companion != null)
        {
            Companion.GetComponent<Companion>().agent.speed = 3.0f;
        }
        Invoke(nameof(DestroyEnemy), 3.0f);
        return;
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    void Start()
    {
        currentState = "Idle";
        nextState = currentState;
        SwitchState();
    }

    private void Update()
    {
        player = FindObjectOfType<NewBehaviourScript>().transform;

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        NPCInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsNPC);
        NPCInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsNPC);

        if (nextState != currentState)
        {
            currentState = nextState;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
