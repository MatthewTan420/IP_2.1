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

public class NewBehaviourScript : MonoBehaviour
{
    float test = 0;

    Vector3 movementInput = Vector3.zero;
    public float movementSpeed = 0.0f;
    Vector3 rotationInput = Vector3.zero;
    public float rotationSpeed = 0.0f;

    public Transform camera;
    public Rigidbody rb;

    public bool isEquip;
    public bool isEquip2;
    private bool isLock;

    GameObject gun;
    public GameObject gunCon;
    public GameObject ammo;
    public GameObject ammoCon;
    public TextMeshProUGUI ammoCount;
    public GameObject melee;
    Shoot gunScript;
    PickUpGun pickScript;
    public Melee meleeScript;

    public float Health;
    public float curHealth;
    public HP_Bar healthbar;

    public GameObject Inven;
    public GameObject UI;
    public InventoryManager invenScript;

    /// <summary>
    /// Controls the player actions and movements
    /// </summary>
    void OnLook(InputValue value)
    {
        rotationInput.y = value.Get<Vector2>().x;
        rotationInput.x = -value.Get<Vector2>().y;
    }
    void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }
    void OnJump(InputValue value)
    {
        rb.AddForce(new Vector3(0, 5, 0), ForceMode.Impulse);
    }

    void OnEquip(InputValue value)
    {
        if (isEquip == true)
        {
            isEquip = false;
            isEquip2 = false;
        }
        else if (isEquip == false)
        {
            isEquip = true;
        }
    }

    void OnEquip2(InputValue value)
    {
        if (isEquip2 == true)
        {
            isEquip2 = false;
            isEquip = false;
        }
        else if (isEquip2 == false)
        {
            isEquip2 = true;
        }
    }

    void OnInven(InputValue value)
    {
        Inven.SetActive(true);
        UI.SetActive(false);
        isLock = false;
        invenScript.ListItems();
    }

    public void Lock()
    {
        isLock = true;
    }

    public void unLock()
    {
        isLock = false;
    }

    /// <summary>
    /// Player takes damage.
    /// </summary>
    public void Damage(float dmg)
    {
        
        if (curHealth > 0)
        {
            curHealth -= dmg;
            healthbar.SetHealth(curHealth);
        }
    }

    /// <summary>
    /// Awake is called when the game starts
    /// </summary>
    private void Awake()
    {
        gun = FindObjectOfType<Shoot>().gameObject;
        gunScript = gun.GetComponent<Shoot>();
        pickScript = gun.GetComponent<PickUpGun>();
        isLock = true;
    }

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        curHealth = Health;
        healthbar.SetMaxHealth(Health);
    }

    /// <summary>
    /// Update is called before the first frame update
    /// </summary>
    void Update()
    {
        
        if (isLock == true)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1.0f;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
        }
        
        if (isEquip == true)
        {
            gunCon.SetActive(true);
            ammo.SetActive(true);
            gunScript.enabled = true;
            melee.SetActive(false);
            meleeScript.enabled = false;
        }
        if (isEquip == false)
        {
            gunCon.SetActive(false);
            ammo.SetActive(false);
            gunScript.enabled = false;
        }

        if (isEquip2 == true)
        {
            melee.SetActive(true);
            meleeScript.enabled = true;
            gunCon.SetActive(false);
            ammo.SetActive(false);
            gunScript.enabled = false;
        }
        if (isEquip2 == false)
        {
            melee.SetActive(false);
            meleeScript.enabled = false;
        }

        /// <summary>
        /// Player movement.
        /// </summary>
        Vector3 forwarDir = transform.forward;
        forwarDir *= movementInput.y;

        Vector3 rightDir = transform.right;
        rightDir *= movementInput.x;

        GetComponent<Rigidbody>().MovePosition(transform.position
            + (forwarDir + rightDir) * movementSpeed);

        if (isLock == true)
        {
            var headRot = camera.rotation.eulerAngles
            + new Vector3(rotationInput.x, 0, 0) * rotationSpeed;
            test += rotationInput.x;

            if (test < 300.0f && test > -300.0f)
            {
                camera.rotation = Quaternion.Euler(headRot);
            }

            transform.rotation = Quaternion.Euler(
                transform.rotation.eulerAngles
                + new Vector3(0, rotationInput.y, 0) * rotationSpeed);
        }
    }
}