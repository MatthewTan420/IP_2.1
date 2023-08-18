/*
 * Author: Matthew, Seth, Wee Kiat, Isabel
 * Date: 19/8/2023
 * Description: IsActive
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsActive : MonoBehaviour
{
    public GameObject active;
    private Transform player;
    public float pickUpRange;

    void Start()
    {
        player = FindObjectOfType<NewBehaviourScript>().transform;
    }

    /// <summary>
    /// set up player in one of the levels
    /// </summary>
    void OnInteract()
    {
        Vector3 distanceToPlayer = player.position - transform.position;
        if (distanceToPlayer.magnitude <= pickUpRange)
        {
            active.SetActive(true);
        }
    }
}
