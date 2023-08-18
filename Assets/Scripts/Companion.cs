/*
 * Author: Matthew, Seth, Wee Kiat, Isabel
 * Date: 19/8/2023
 * Description: Companion Script
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Companion : MonoBehaviour
{
    private string currentState;
    private string nextState;

    public UnityEngine.AI.NavMeshAgent agent;
    public Transform player;
    private Transform enemy;
    public bool isReady = false;
    Vector3 dest;
    public float pickUpRange, sightRange;
    private bool enemyInSightRange;
    public LayerMask whatIsGround, whatIsEnemy;
    public int dmg;
    bool alreadyAttacked;
    public bool isStuck = false;

    public AudioSource woof;
    public AudioSource growl;
    public AudioSource whine;
    float timerVal = 0;
    float timerVal1 = 6;

    private void SwitchState()
    {
        StartCoroutine(currentState);
    }

    void Awake()
    {
        enemy = FindObjectOfType<Enemy>().transform;
    }

    void Start()
    {
        player = FindObjectOfType<NewBehaviourScript>().transform;
        currentState = "Idle";
        nextState = currentState;
        SwitchState();
    }

    /// <summary>
    /// Idle State
    /// </summary>
    IEnumerator Idle()
    {
        GetComponent<Animator>().SetTrigger("isIdle");
        GetComponent<Animator>().ResetTrigger("isFollow");
        GetComponent<Animator>().ResetTrigger("isAtk");

        bool isIdle = true;
        while (isIdle)
        {
            if (isReady)
            {
                nextState = "Follow";
                isIdle = false;
            }
            yield return new WaitForEndOfFrame();
        }
        SwitchState();
    }

    /// <summary>
    /// Follow State
    /// </summary>
    IEnumerator Follow()
    {
        bool isFollow = true;

        while (isFollow)
        {
            agent.SetDestination(player.position);
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

            if (enemyInSightRange)
            {
                nextState = "Chase";
                isFollow = false;
            }
            yield return new WaitForEndOfFrame();
        }
        SwitchState();
    }

    /// <summary>
    /// Chase Zombie State
    /// </summary>
    IEnumerator Chase()
    {
        GetComponent<Animator>().SetTrigger("isFollow");
        GetComponent<Animator>().ResetTrigger("isIdle");
        GetComponent<Animator>().ResetTrigger("isAtk");

        bool isChase = true;

        while (isChase)
        {
            enemy = FindObjectOfType<Enemy>().transform;
            dest = enemy.position;
            agent.destination = dest;

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                nextState = "Atk";
                isChase = false;
            }
            yield return new WaitForEndOfFrame();
        }
        SwitchState();
    }
    /// <summary>
    /// Attack Zombie State
    /// </summary>
    IEnumerator Atk()
    {
        GetComponent<Animator>().SetTrigger("isAtk");
        GetComponent<Animator>().ResetTrigger("isIdle");
        GetComponent<Animator>().ResetTrigger("isFollow");

        bool isAtk = true;

        while (isAtk)
        {
            Enemy isScript = FindObjectOfType<Enemy>();
            if (isScript != null)
            {
                enemy = isScript.transform;
            }
            else
            {
                nextState = "Follow";
                isAtk = false;
            }
            //attack zombie if dog is close enough
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!alreadyAttacked)
                {
                    enemy.GetComponent<Enemy>().Damage(dmg);
                    alreadyAttacked = true;
                    Invoke(nameof(ResetAttack), 2.0f);
                }
            }
            else
            {
                nextState = "Chase";
                isAtk = false;
            }

            if (!enemyInSightRange)
            {
                nextState = "Follow";
                isAtk = false;
            }
            yield return new WaitForEndOfFrame();
        }
        SwitchState();
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    /// <summary>
    /// To get the dog to follow player
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

    /// <summary>
    /// Dog stops if crawler is near
    /// </summary>
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Crawler")
        {
            agent.speed = 0.0f;
            isStuck = true;
            whine.Play();
        }
    }

    /// <summary>
    /// Audio
    /// </summary>
    private void Woof(float sec)
    {
        timerVal += Time.deltaTime;
        if (timerVal > sec)
        {
            woof.Play();
            timerVal -= sec;
        }
    }

    /// <summary>
    /// Audio if near enemy
    /// </summary>
    private void Growl(float sec)
    {
        timerVal1 += Time.deltaTime;
        if (timerVal1 > sec)
        {
            growl.Play();
            timerVal1 -= sec;
        }
    }

    private void Update()
    {
        enemyInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsEnemy);

        if (!isStuck)
        {
            whine.Stop();
        }

        if (enemyInSightRange)
        {
            Growl(7);
        }
        else
        {
            Woof(8);
        }

        if (nextState != currentState)
        {
            currentState = nextState;
        }
    }
}
