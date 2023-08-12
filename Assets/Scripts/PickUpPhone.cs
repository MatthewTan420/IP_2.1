using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpPhone : MonoBehaviour
{
    public Transform player;
    public float pickUpRange;

    /// <summary>
    /// Player pick up items
    /// </summary>
    void PickUp()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Pick up function
    /// </summary>
    void OnPickUp()
    {
        player = FindObjectOfType<NewBehaviourScript>().transform;
        Vector3 distanceToPlayer = player.position - transform.position;
        if (distanceToPlayer.magnitude <= pickUpRange)
        {
            PickUp();
            NewBehaviourScript.isLight = true;
        }
    }
}
