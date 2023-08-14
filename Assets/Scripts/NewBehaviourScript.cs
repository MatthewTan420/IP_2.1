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
    public float movementSpeed;
    public float sprintSpeed;
    Vector3 rotationInput = Vector3.zero;
    public float rotationSpeed;
    private bool sprint = false;
    private float curSpeed;
    private float curSprint;
    private bool PlayerGrounded = true;

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

    public float Health;
    public static float curHealth;
    public HP_Bar healthbar;
    private bool isDead = false;

    public static InventoryManager Instance;
    public List<Item> Items;
    public List<Item> NewItems;
    public List<Item> Items2;
    public List<Item> NewItems2;
    public Item Item;
    public float heal = 3.0f;
    
    public GameObject Inven;
    public GameObject UI;
    public GameObject Menu;
    public GameObject DeathMenu;
    public bool isMenu = false;
    public InventoryManager invenScript;

    public bool isStuck = false;
    public static bool isTeleport = false;
    public static bool isLight = false;
    public GameObject light;

    public bool isSwitched = false;

    public GameObject questCon;
    public TextMeshProUGUI questText;
    public GameObject dialCon;
    public TextMeshProUGUI dialText;

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
        if (isStuck == false && PlayerGrounded == true)
        {
            rb.AddForce(new Vector3(0, 5, 0), ForceMode.Impulse);
            PlayerGrounded = false;
        }
    }

    void OnSprint()
    {
        sprint = true;
    }
    void OnSprintDone()
    {
        sprint = false;
    }

    void OnHeal(InputValue value)
    {
        if (Items.Contains(Item))
        {
            if (curHealth <= Health)
            {
                curHealth += heal;
                Items.Remove(Item);
                Items2.Remove(Item);
                healthbar.SetHealth(curHealth);
                if (curHealth >= Health)
                {
                    curHealth = Health;
                    healthbar.SetHealth(curHealth);
                }
            }
            if (!Items.Contains(Item))
            {
                NewItems.Remove(Item);
                NewItems2.Remove(Item);
            }
        }
    }

    void OnInven(InputValue value)
    {
        if (isDead != true)
        {
            Inven.SetActive(true);
            UI.SetActive(false);
            isLock = false;
            invenScript.ListItems();
        }
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Menu")
        {
            Menu.SetActive(true);
            isLock = false;
            isTeleport = false;
        }
        else if (collision.gameObject.tag == "Untagged")
        {
            PlayerGrounded = true;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Teleport")
        {
            isTeleport = true;
        }
        else if (col.gameObject.tag == "Crawler")
        {
            isStuck = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Crawler")
        {
            isStuck = false;
        }
        else if (col.gameObject.tag == "Quest")
        {
            Destroy(col.gameObject);
        }
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
        else if (curHealth <= 0)
        {
            isDead = true;
            GetComponent<Animator>().SetTrigger("isDie");
        }
    }

    /// <summary>
    /// Awake is called when the game starts
    /// </summary>
    private void Awake()
    {
        isLock = true;

        NewItems2 = InventoryManager.NewItems2;
        Items2 = InventoryManager.Items2;
    }

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        curSpeed = movementSpeed;
        curSprint = sprintSpeed;

        if (isTeleport == true)
        {
            //curHealth = curHealth;
            isTeleport = false;
        }
        else
        {
            curHealth = Health;
        }
        healthbar.SetMaxHealth(Health);
        healthbar.SetHealth(curHealth);
    }

    /// <summary>
    /// Update is called before the first frame update
    /// </summary>
    void Update()
    {
        if (isStuck == true)
        {
            curSpeed = 0.0f;
            curSprint = 0.0f;
        }
        else
        {
            curSpeed = movementSpeed;
            curSprint = sprintSpeed;
        }

        if (isLight == true)
        {
            light.SetActive(true);
        }
        else
        {
            light.SetActive(false);
        }

        Items = InventoryManager.Instance.Items;
        NewItems = InventoryManager.Instance.NewItems;

        if (curHealth <= 0)
        {
            DeathMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            return;
        }

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

    /// <summary>
    /// Player movement.
    /// </summary>
    Vector3 forwarDir = transform.forward;
        forwarDir *= movementInput.y;

        Vector3 rightDir = transform.right;
        rightDir *= movementInput.x;

        if (sprint == true)
        {
            GetComponent<Rigidbody>().MovePosition(transform.position
            + (forwarDir + rightDir) * curSprint);
        }
        else
        {
            GetComponent<Rigidbody>().MovePosition(transform.position
                + (forwarDir + rightDir) * curSpeed);
        }

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