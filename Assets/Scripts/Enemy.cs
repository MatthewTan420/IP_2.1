/*
 * Author: Matthew, Seth, Wee Kiat, Isabel
 * Date: 19/8/2023
 * Description: Enemy Script
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class Enemy : MonoBehaviour
{
    private string currentState;
    private string nextState;

    public NavMeshAgent agent;
    public Transform player;
    private Transform NPC;
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

    public AudioSource idle;
    public AudioSource spot;
    public AudioSource death;
    public AudioSource atk;
    public AudioSource hit;
    public VisualEffect blood;

    private void SwitchState()
    {
        StartCoroutine(currentState);
    }

    /// <summary>
    /// Idle State
    /// </summary>
    IEnumerator Idle()
    {
        if (!isDead)
        {
            while (currentState == "Idle")
            {
                if (!walkPointSet)
                {
                    GetComponent<Animator>().SetTrigger("isIdle");
                    GetComponent<Animator>().ResetTrigger("isWalk");
                    GetComponent<Animator>().ResetTrigger("isRun");
                    GetComponent<Animator>().ResetTrigger("isAtk");
                    idle.Play();
                    Invoke(nameof(SearchWalkPoint), 3.0f);
                }

                if (walkPointSet)
                {
                    agent.SetDestination(walkPoint);
                    agent.speed = 2.0f;
                    GetComponent<Animator>().SetTrigger("isWalk");
                    GetComponent<Animator>().ResetTrigger("isRun");
                    GetComponent<Animator>().ResetTrigger("isIdle");
                    GetComponent<Animator>().ResetTrigger("isAtk");
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

    /// <summary>
    /// Chase Player State
    /// </summary>
    IEnumerator Chase()
    {
        if (!isDead)
        {
            agent.speed = 3.5f;
            GetComponent<Animator>().SetTrigger("isRun");
            GetComponent<Animator>().ResetTrigger("isWalk");
            GetComponent<Animator>().ResetTrigger("isIdle");
            GetComponent<Animator>().ResetTrigger("isAtk");

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

    /// <summary>
    /// Atk Player State
    /// </summary>
    IEnumerator Atk()
    {
        if (!isDead)
        {
            GetComponent<Animator>().SetTrigger("isAtk");
            GetComponent<Animator>().ResetTrigger("isRun");
            GetComponent<Animator>().ResetTrigger("isWalk");
            GetComponent<Animator>().ResetTrigger("isIdle");
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

    /// <summary>
    /// Chase NPC State
    /// </summary>
    IEnumerator ChaseN()
    {
        if (!isDead)
        {
            agent.speed = 3.5f;

            GetComponent<Animator>().SetTrigger("isRun");
            GetComponent<Animator>().ResetTrigger("isWalk");
            GetComponent<Animator>().ResetTrigger("isIdle");
            GetComponent<Animator>().ResetTrigger("isAtk");

            bool isChasing = true;

            while (isChasing)
            {
                NPC = FindObjectOfType<NPC>().transform;
                agent.SetDestination(NPC.position);

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

    /// <summary>
    /// Atk NPC State
    /// </summary>
    IEnumerator AtkN()
    {
        if (!isDead)
        {
            GetComponent<Animator>().SetTrigger("isAtk");
            //transform.LookAt(NPC);

            bool isAtk = true;

            while (isAtk)
            {
                NPC isScript = FindObjectOfType<NPC>();
                if (isScript != null)
                {
                    NPC = isScript.transform;
                }
                else
                {
                    nextState = "Idle";
                    isAtk = false;
                }

                agent.SetDestination(transform.position);
                if (!alreadyAttacked)
                {
                    NPC.GetComponent<NPC>().Die();
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

    public void Damage(int dmg)
    {
        health -= dmg;
        if (health > 0)
        {
            hit.Play();
            blood.Play();
        }
        if (health <= 0)
        {
            agent.speed = 0.0f;
            Died();
            blood.Play();
        }
    }

    private void Died()
    {
        isDead = true;
        GetComponent<Animator>().SetTrigger("isDie");
        GetComponent<Animator>().ResetTrigger("isRun");
        GetComponent<Animator>().ResetTrigger("isWalk");
        GetComponent<Animator>().ResetTrigger("isIdle");
        GetComponent<Animator>().ResetTrigger("isAtk");
        death.Play();
        Invoke(nameof(DestroyEnemy), 5.0f);
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
