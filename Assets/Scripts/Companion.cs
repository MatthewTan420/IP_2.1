/*
 * Author: 
 * Date: 
 * Description: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Companion : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    private Transform enemy;
    private bool isReady = false;
    Vector3 dest;
    public float pickUpRange, sightRange;
    private bool enemyInSightRange;
    public LayerMask whatIsGround, whatIsEnemy;
    public int dmg;
    bool alreadyAttacked;

    void Awake()
    {
        enemy = FindObjectOfType<Enemy>().transform;
    }

    void Start()
    {
        player = FindObjectOfType<NewBehaviourScript>().transform;
    }
    /// <summary>
    /// Companions follow player if he wants it to
    /// </summary>
    void Update()
    {
        if (isReady == true)
        {
            enemyInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsEnemy);
            if (enemyInSightRange)
            {
                enemy = FindObjectOfType<Enemy>().transform;
                dest = enemy.position;
                agent.destination = dest;
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    GetComponent<Animator>().SetTrigger("isAtk");
                    GetComponent<Animator>().ResetTrigger("isFollow");
                    AttackNPC();
                }
                else
                {
                    GetComponent<Animator>().SetTrigger("isFollow");
                    GetComponent<Animator>().ResetTrigger("isAtk");
                }
            }
            else
            {
                dest = player.position;
                agent.destination = dest;
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    GetComponent<Animator>().SetTrigger("isIdle");
                    GetComponent<Animator>().ResetTrigger("isFollow");
                    GetComponent<Animator>().ResetTrigger("isAtk");
                }
                else
                {
                    GetComponent<Animator>().SetTrigger("isFollow");
                    GetComponent<Animator>().ResetTrigger("isIdle");
                    GetComponent<Animator>().ResetTrigger("isAtk");
                }
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Crawler")
        {
            agent.speed = 0.0f;
        }
    }

    /// <summary>
    /// When player wants the companion to follow them
    /// </summary>
    void OnComp()
    {
        Vector3 distanceToPlayer = player.position - transform.position;
        if (distanceToPlayer.magnitude <= pickUpRange)
        {
            isReady = true;
            GetComponent<Animator>().SetTrigger("isReady");
        }
    }

    private void AttackNPC()
    {
        if (!alreadyAttacked)
        {
            enemy.GetComponent<Enemy>().Damage(dmg);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), 2.0f);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
