using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    /// <summary>
    /// The amount of damage dealt when player stands in it
    /// </summary>
    public float damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<NewBehaviourScript>().Damage(damage * Time.deltaTime);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<NewBehaviourScript>().Damage(damage * Time.deltaTime);
        }
    }
}
