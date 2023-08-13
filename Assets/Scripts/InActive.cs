using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InActive : MonoBehaviour
{
    public GameObject inactive;
    private Transform player;
    public float pickUpRange;
    public AudioSource sound;

    void Start()
    {
        player = FindObjectOfType<NewBehaviourScript>().transform;
    }

    void OnPickUp()
    {
        Vector3 distanceToPlayer = player.position - transform.position;
        if (distanceToPlayer.magnitude <= pickUpRange)
        {
            inactive.SetActive(false);
            sound.Play();
        }
    }
}
