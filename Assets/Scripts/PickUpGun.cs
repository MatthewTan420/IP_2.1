/*
 * Author: 
 * Date: 
 * Description: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpGun : MonoBehaviour
{
    public Shoot gunScript;
    public Rigidbody rb;
    public Collider coll;
    private Transform player;
    private NewBehaviourScript script;
    private Transform gunContainer, fpsCam;
    private GameObject ammoCount;

    public float pickUpRange;
    public float dropForwardForce, dropUpwardForce;

    public bool equipped;
    public static bool slotFull;

    public Item Item;

    private void Start()
    {
        gunScript.SetUp();
        player = FindObjectOfType<NewBehaviourScript>().transform;
        script = player.GetComponent<NewBehaviourScript>();
        gunContainer = FindObjectOfType<gunContainer>().transform;
        ammoCount = script.ammoCon;
        fpsCam = FindObjectOfType<Camera>().transform;

        if (!equipped)
        {
            gunScript.enabled = false;
            rb.isKinematic = false;
            coll.isTrigger = false;
            ammoCount.SetActive(false);
        }
        if (equipped)
        {
            gunScript.enabled = true;
            rb.isKinematic = true;
            coll.isTrigger = true;
            ammoCount.SetActive(true);
            slotFull = true;
        }
    }

    /// <summary>
    /// Pick up or drop the item
    /// </summary>
    void OnPickUp()
    {
        //Drop if equipped and "Q" is pressed
        if (equipped == true) 
        {
            Drop();
        }
        else
        {
            //Check if player is in range and "E" is pressed
            Vector3 distanceToPlayer = player.position - transform.position;
            if (distanceToPlayer.magnitude <= pickUpRange && !slotFull)
            {
                PickUp();
            }
        }
    }

    /// <summary>
    /// Pick up function
    /// </summary>
    private void PickUp()
    {
        equipped = true;
        slotFull = true;

        gunScript.holdGun = true;
        ammoCount.SetActive(true);

        InventoryManager.Instance.Add(Item);

        //Make weapon a child of the camera and move it to default position
        transform.SetParent(gunContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;

        //Make Rigidbody kinematic and BoxCollider a trigger
        rb.isKinematic = true;
        coll.isTrigger = true;

        //Enable script
        gunScript.enabled = true;
    }

    /// <summary>
    /// Drop function
    /// </summary>
    private void Drop()
    {
        equipped = false;
        slotFull = false;

        gunScript.holdGun = false;
        ammoCount.SetActive(false);

        InventoryManager.Instance.Remove(Item);
        //Set parent to null
        transform.SetParent(null);

        //Make Rigidbody not kinematic and BoxCollider normal
        rb.isKinematic = false;
        coll.isTrigger = false;

        //Gun carries momentum of player
        rb.velocity = player.GetComponent<Rigidbody>().velocity;

        //AddForce
        rb.AddForce(fpsCam.forward * dropForwardForce, ForceMode.Impulse);
        rb.AddForce(fpsCam.up * dropUpwardForce, ForceMode.Impulse);
        //Add random rotation
        float random = Random.Range(-1f, 1f);
        rb.AddTorque(new Vector3(random, random, random) * 10);

        //Disable script
        gunScript.enabled = false;
    }
}
