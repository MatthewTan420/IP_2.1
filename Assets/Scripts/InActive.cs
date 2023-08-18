/*
 * Author: Matthew, Seth, Wee Kiat, Isabel
 * Date: 19/8/2023
 * Description: InActive
 */

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

    /// <summary>
    /// set up player in one of the levels
    /// </summary>
    void OnInteract()
    {
        Vector3 distanceToPlayer = player.position - transform.position;
        if (distanceToPlayer.magnitude <= pickUpRange)
        {
            inactive.SetActive(false);
            sound.Play();
        }
    }
}
