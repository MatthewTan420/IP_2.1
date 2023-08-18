/*
 * Author: Matthew, Seth, Wee Kiat, Isabel
 * Date: 19/8/2023
 * Description: ItemPickUp
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    public Item Item;
    public Transform player;
    public float pickUpRange;
    public GameObject pickUp;

    void Awake()
    {
        pickUp.SetActive(false);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            pickUp.SetActive(true);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            pickUp.SetActive(false);
        }
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
    void OnInteract()
    {
        player = FindObjectOfType<NewBehaviourScript>().transform;
        Vector3 distanceToPlayer = player.position - transform.position;
        if (distanceToPlayer.magnitude <= pickUpRange)
        {
            PickUp();
        }
    }
}
