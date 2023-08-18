/*
 * Author: 
 * Date: 
 * Description: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    /// <summary>
    /// The amount of damage dealt when player stands in it
    /// </summary>
    public float damage;

    /// <summary>
    /// Dmg dealt if user stays
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<NewBehaviourScript>().Damage(damage * Time.deltaTime);
        }
    }
    /// <summary>
    /// Stops the player from getting dmg
    /// </summary>
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<NewBehaviourScript>().Damage(damage * Time.deltaTime);
        }
    }
}
