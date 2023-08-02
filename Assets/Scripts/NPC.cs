/*
 * Author: 
 * Date: 
 * Description: 
 */

using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public NavMeshAgent agent;
    private Transform enemy;
    public LayerMask whatIsGround, whatIsEnemy;

    public float sightRange;
    public bool enemyInSightRange;
    private float displacementDist = 5.0f;

    private bool isDead = false;

    private void Awake()
    {
        enemy = FindObjectOfType<Enemy>().transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (isDead == false)
        {
            enemyInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsEnemy);
            
            if (enemyInSightRange)
            {
                enemy = FindObjectOfType<Enemy>().transform;
                RunNPC();
            }
            else
            {
                //Debug.Log("Idle");
            }
        }
        else
        {
            //Debug.Log("Die");
            return;
        }
    }

    private void RunNPC()
    {
        Vector3 normDir = (enemy.position - transform.position).normalized;
        normDir = Quaternion.AngleAxis(Random.Range(-90, 90), Vector3.up) * normDir;
        MoveToPos(transform.position - (normDir * displacementDist));
    }

    private void MoveToPos(Vector3 pos)
    {
        agent.SetDestination(pos);
    }

    public void Die()
    {
        isDead = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
