using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialDoor : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Item> Items;
    public List<Item> NewItems;
    public List<Item> Items2;
    public List<Item> NewItems2;
    public Item Item;

    private Transform player;
    public float pickUpRange;
    public GameObject pickUp;

    public bool isLock = true;

    public AudioSource enter;
    public AudioSource exit;
    public AudioSource unlock;
    public AudioSource locked;

    private void Awake()
    {
        NewItems2 = InventoryManager.NewItems2;
        Items2 = InventoryManager.Items2;
        //player = FindObjectOfType<NewBehaviourScript>().transform;
        pickUp.SetActive(false);
    }

    private void Update()
    {
        Items = InventoryManager.Instance.Items;
        NewItems = InventoryManager.Instance.NewItems;
        player = FindObjectOfType<NewBehaviourScript>().transform;

        if (isLock == false)
        {
            pickUp.SetActive(false);
        }
    }

    void OnInteract()
    {
        Vector3 distanceToPlayer = player.position - transform.position;
        if (distanceToPlayer.magnitude <= pickUpRange)
        {
            if (Items.Contains(Item))
            {
                Items.Remove(Item);
                Items2.Remove(Item);
                isLock = false;
                unlock.Play();

                if (!Items.Contains(Item))
                {
                    NewItems.Remove(Item);
                    NewItems2.Remove(Item);
                }
            }

            if (isLock == true)
            {
                locked.Play();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && isLock == false)
        {
            GetComponent<Animator>().SetTrigger("Enter");
            enter.Play();
        }

        if (other.gameObject.tag == "Player")
        {
            pickUp.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && isLock == false)
        {
            GetComponent<Animator>().SetTrigger("Exit");
            exit.Play();
        }

        if (other.gameObject.tag == "Player")
        {
            pickUp.SetActive(false);
        }
    }
}
