/*
 * Author: 
 * Date: 
 * Description: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpMelee : MonoBehaviour
{
    public Melee meleeScript;
    public Rigidbody rb;
    public Collider coll;
    public Transform melee;
    private Transform player;
    NewBehaviourScript script;
    private Transform meleeContainer, fpsCam;
    private GameObject ammoCount;
    public GameObject pickUp;

    public float pickUpRange;
    public float dropForwardForce, dropUpwardForce;

    public bool equipped;
    public bool Active = true;
    public static bool slotFull;

    public Item Item;

    private void Start()
    {
        meleeScript.SetUp();
        player = FindObjectOfType<NewBehaviourScript>().transform;
        script = player.GetComponent<NewBehaviourScript>();
        meleeContainer = FindObjectOfType<meleeContainer>().transform;
        fpsCam = FindObjectOfType<Camera>().transform;

        if (!equipped)
        {
            meleeScript.enabled = false;
            rb.isKinematic = false;
            coll.isTrigger = false;
        }
        if (equipped)
        {
            meleeScript.enabled = true;
            rb.isKinematic = true;
            coll.isTrigger = true;
            slotFull = true;
        }
    }

    private void Update()
    {
        if (equipped)
        {
            pickUp.SetActive(false);
        }
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
    /// Pick up or drop the item
    /// </summary>
    void OnPickUp()
    {
        //Drop if equipped and "Q" is pressed
        if (equipped == true && Active == true)
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

    void OnEquip()
    {
        if (equipped)
        {
            melee.gameObject.SetActive(false);
            Active = false;
        }
    }

    void OnEquip2()
    {
        if (equipped && Active == false)
        {
            melee.gameObject.SetActive(true);
            Active = true;
        }
        else if (equipped && Active == true)
        {
            melee.gameObject.SetActive(false);
            Active = false;
        }
    }

    /// <summary>
    /// Pick up function
    /// </summary>
    private void PickUp()
    {
        equipped = true;
        slotFull = true;

        meleeScript.holdGun = true;


        InventoryManager.Instance.Add(Item);

        //Make weapon a child of the camera and move it to default position
        transform.SetParent(meleeContainer);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = Vector3.one;

        //Make Rigidbody kinematic and BoxCollider a trigger
        rb.isKinematic = true;
        coll.isTrigger = true;

        //Enable script
        meleeScript.enabled = true;
    }

    /// <summary>
    /// Drop function
    /// </summary>
    private void Drop()
    {
        equipped = false;
        slotFull = false;

        meleeScript.holdGun = false;

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
        meleeScript.enabled = false;
    }
}
