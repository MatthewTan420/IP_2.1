/*
 * Author: 
 * Date: 
 * Description: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public AudioSource enter;
    public AudioSource exit;

    /// <summary>
    /// This opens the door if user is near
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GetComponent<Animator>().SetTrigger("Enter");
            enter.Play();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GetComponent<Animator>().SetTrigger("Exit");
            exit.Play();
        }
    }
}
