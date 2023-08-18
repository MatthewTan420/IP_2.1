/*
 * Author: Matthew, Seth, Wee Kiat, Isabel
 * Date: 19/8/2023
 * Description: Jumpscare
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumpscare : MonoBehaviour
{
    public GameObject jumpscare;
    public GameObject zombie;
    public AudioSource scare;

    /// <summary>
    /// This activates jumpscare
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            jumpscare.GetComponent<Animator>().SetTrigger("isJumped");
            scare.Play();
            Invoke(nameof(DestroyJump), 1.0f);
        }
    }

    /// <summary>
    /// This destroys jumpscare
    /// </summary>
    private void DestroyJump()
    {
        Destroy(jumpscare);
        Destroy(gameObject);
    }
}
