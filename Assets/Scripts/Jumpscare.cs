using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumpscare : MonoBehaviour
{
    public GameObject jumpscare;
    public GameObject zombie;
    public AudioSource scare;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            jumpscare.GetComponent<Animator>().SetTrigger("isJumped");
            scare.Play();
            Invoke(nameof(DestroyJump), 1.0f);
        }
    }

    private void DestroyJump()
    {
        Destroy(jumpscare);
        Destroy(gameObject);
    }
}
