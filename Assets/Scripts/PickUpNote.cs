using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpNote : MonoBehaviour
{
    public Transform player;
    public float pickUpRange;
    //public GameObject pickUp;
    public GameObject note;
    NewBehaviourScript script;

    void Awake()
    {
        //pickUp.SetActive(false);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            //pickUp.SetActive(true);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            //pickUp.SetActive(false);
        }
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
            note.SetActive(true);
            Debug.Log("Gay");
        }
    }
    void Update()
    {
        script = FindObjectOfType<NewBehaviourScript>();
        note = script.noteOne;
    }
}
