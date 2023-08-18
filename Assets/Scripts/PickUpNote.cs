using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpNote : MonoBehaviour
{
    public Transform player;
    public float pickUpRange;
    //public GameObject pickUp;
    public GameObject note;
    private bool isNote = false;
    NewBehaviourScript script;
    public int noteNum;

    void Awake()
    {
        //pickUp.SetActive(false);
    }

    void Start()
    {
        script = FindObjectOfType<NewBehaviourScript>();
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
    void OnInteract()
    {
        if (isNote == false)
        {
            player = FindObjectOfType<NewBehaviourScript>().transform;
            Vector3 distanceToPlayer = player.position - transform.position;
            if (distanceToPlayer.magnitude <= pickUpRange)
            {
                note.SetActive(true);
                isNote = true;
            }
        }
    }

    void OnPickUp()
    {
        if (isNote == true)
        {
            note.SetActive(false);
            isNote = false;
        }
    }
    void Update()
    {
        if (noteNum == 1)
        {
            note = script.noteOne;
        }
        else if (noteNum == 2)
        {
            note = script.noteTwo;
        }
        else if (noteNum == 4)
        {
            note = script.noteFour;
        }
        else if (noteNum == 5)
        {
            note = script.noteFive;
        }
        else if (noteNum == 6)
        {
            note = script.noteSix;
        }
    }
}
