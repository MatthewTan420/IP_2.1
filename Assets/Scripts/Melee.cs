/*
 * Author: 
 * Date: 
 * Description: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Melee : MonoBehaviour
{
    public float range, timeBetweenShots;
    public int dmg;
    public bool holdGun;
    public AudioSource swing;

    bool shooting, readyToShoot;
    public PickUpMelee pickM;

    void Awake()
    {
        readyToShoot = true;
    }

    public void SetUp()
    {
        holdGun = false;
    }

    /// <summary>
    /// Player hit
    /// </summary>
    void OnFire()
    {
        if (pickM.Active == true)
        {
            if (holdGun == false)
            {
                return;
            }

            if (readyToShoot)
            {
                Shot();
            }
        }
    }

    /// <summary>
    /// Melee function, damages enemy if it was hit
    /// </summary>
    void Shot()
    {
        readyToShoot = false;
        swing.Play();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            if (hit.transform.tag == "Enemy")
            {
                hit.transform.GetComponent<Enemy>().Damage(dmg);
            }
            else if (hit.transform.tag == "Crawler")
            {
                hit.transform.GetComponent<Crawler>().Damage(dmg);
            }
        }
        Invoke(nameof(ResetShot), timeBetweenShots);

    }

    /// <summary>
    /// Fires one bullet at a time
    /// </summary>
    void ResetShot()
    {
        readyToShoot = true;
    }
}
