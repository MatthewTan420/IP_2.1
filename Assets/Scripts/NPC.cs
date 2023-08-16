/*
 * Author: 
 * Date: 
 * Description: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    private string currentState;
    private string nextState;

    public NavMeshAgent agent;
    private Transform enemy;
    public LayerMask whatIsGround, whatIsEnemy;

    public float sightRange;
    public bool enemyInSightRange;
    private float displacementDist = 5.0f;

    private bool isDead = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void SwitchState()
    {
        StartCoroutine(currentState);
    }

    IEnumerator Idle()
    {
        bool isIdle = true;
        while (isIdle)
        {
            GetComponent<Animator>().SetTrigger("isIdle");
            GetComponent<Animator>().ResetTrigger("isRun");
            GetComponent<Animator>().ResetTrigger("isDead");

            if (enemyInSightRange)
            {
                nextState = "Chase";
                isIdle = false;
            }
            yield return new WaitForEndOfFrame();
        }
        SwitchState();
    }

    IEnumerator Chase()
    {
        GetComponent<Animator>().SetTrigger("isRun");
        GetComponent<Animator>().ResetTrigger("isIdle");
        GetComponent<Animator>().ResetTrigger("isDead");

        bool isChasing = true;

        while (isChasing)
        {
            enemy = FindObjectOfType<Enemy>().transform;

            Vector3 normDir = (enemy.position - transform.position).normalized;
            //normDir = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up) * normDir;
            MoveToPos(transform.position - (normDir * displacementDist));

            // some example of how the switching will work 
            if (!enemyInSightRange)
            {
                nextState = "Idle";
                isChasing = false;
            }
            yield return new WaitForEndOfFrame();
        }
        SwitchState();
    }

    void Start()
    {
        currentState = "Idle";
        nextState = currentState;
        SwitchState();
    }

    private void Update()
    {
        enemyInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsEnemy);

        if (nextState != currentState)
        {
            currentState = nextState;
        }

        if (isDead == true)
        {
            Invoke(nameof(DestroyEnemy), 5.0f);
            GetComponent<Animator>().SetTrigger("isDead");
            GetComponent<Animator>().ResetTrigger("isRun");
            GetComponent<Animator>().ResetTrigger("isIdle");
            return;
        }
    }

    private void MoveToPos(Vector3 pos)
    {
        agent.SetDestination(pos);
    }

    public void Die()
    {
        isDead = true;
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}