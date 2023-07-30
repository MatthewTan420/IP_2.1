/*
 * Author: 
 * Date: 
 * Description: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Shoot : MonoBehaviour
{
    public float range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletPerTap;
    int bulletsLeft, bulletsShot;
    public int dmg;
    public bool holdGun;

    bool shooting, readyToShoot, reloading;

    private TextMeshProUGUI Ammo;
    public ParticleSystem MuzzleFlash;

    private NewBehaviourScript script;
    private Transform player;

    void Awake()
    {
        player = FindObjectOfType<NewBehaviourScript>().transform;
        script = player.GetComponent<NewBehaviourScript>();
        Ammo = script.ammoCount;

        bulletsLeft = magazineSize;
        readyToShoot = true;
        Ammo.text = bulletsLeft + "/" + magazineSize;
    }

    public void SetUp()
    {
        holdGun = false;
    }

    /// <summary>
    /// Player shooting
    /// </summary>
    void OnFire()
    {
        if (holdGun == false)
        {
            return;
        }

        if ((readyToShoot && reloading == false) && bulletsLeft > 0)
        {
            Shot();
        }
        
    }

    /// <summary>
    /// Shooting function, damages enemy if it was shot
    /// </summary>
    void Shot()
    {
        readyToShoot = false;
        MuzzleFlash.Play();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            if (hit.transform.tag == "Enemy")
            {
                hit.transform.GetComponent<Enemy>().Damage(dmg);
            }
        }
        bulletsLeft -= 1;
        Ammo.text = bulletsLeft + "/" + magazineSize;
        Invoke(nameof(ResetShot), timeBetweenShots);

    }

    /// <summary>
    /// Fires one bullet at a time
    /// </summary>
    void ResetShot()
    {
        readyToShoot = true;
    }

    /// <summary>
    /// Reloads weapon
    /// </summary>
    void OnReload(InputValue value)
    {
        reloading = true;
        Ammo.text = "Reloading...";
        Invoke(nameof(ReloadFinish), reloadTime);
    }

    void ReloadFinish()
    {
        bulletsLeft = magazineSize;
        Ammo.text = bulletsLeft + "/" + magazineSize;
        reloading = false;
    }
}
