using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumpscare : MonoBehaviour
{
    public GameObject jumpscare;
    public GameObject zombie;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            jumpscare.GetComponent<Animator>().SetTrigger("isJumped");
            Invoke(nameof(DestroyJump), 1.5f);
        }
    }

    private void DestroyJump()
    {
        Destroy(jumpscare);
        Destroy(gameObject);
    }
}
