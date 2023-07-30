/*
 * Author: 
 * Date: 
 * Description: 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Item> Items = new List<Item>();
    public List<Item> NewItems = new List<Item>();

    public Transform ItemContent;
    public GameObject InventoryItem;
    private int count;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// This is to add items in the list
    /// </summary>
    public void Add(Item item)
    {
        if (!NewItems.Contains(item))
        {
            NewItems.Add(item);
        }

        Items.Add(item);
        
    }

    /// <summary>
    /// This removes items from the list
    /// </summary>
    public void Remove(Item item)
    {
        Items.Remove(item);
        NewItems.Remove(item);
    }

    /// <summary>
    /// This is to update the inventory system, where it shows the item and how many of it
    /// </summary>
    public void ListItems()
    {
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in NewItems)
        {
            count = 0;
            foreach (var it in Items)
            {
                if (item == it)
                {
                    count += 1;
                }
            }

            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
            var itemNum = obj.transform.Find("ItemNum").GetComponent<TextMeshProUGUI>();
            itemName.text = item.itemName;
            itemNum.text = "" + count;
        }
    }
}
