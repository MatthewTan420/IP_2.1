/*
 * Author: 
 * Date: 
 * Description: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item Item;
    public Transform player;
    public float pickUpRange;

    void Awake()
    {
        
    }

    /// <summary>
    /// Player pick up items
    /// </summary>
    void PickUp()
    {
        InventoryManager.Instance.Add(Item);
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
        }
    }
}
