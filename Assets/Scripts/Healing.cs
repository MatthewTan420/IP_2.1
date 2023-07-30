using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Item> Items;
    public List<Item> NewItems;
    public Item Item;
    public float heal = 3.0f;
    public NewBehaviourScript script;

    void Update()
    {
        Items = InventoryManager.Instance.Items;
        NewItems = InventoryManager.Instance.NewItems;
    }

    void OnHeal()
    {
        if (Items.Contains(Item))
        {
            if (script.curHealth <= script.Health)
            {
                script.curHealth += heal;
                Items.Remove(Item);
                script.healthbar.SetHealth(script.curHealth);
                if (script.curHealth >= script.Health)
                {
                    script.curHealth = script.Health;
                    script.healthbar.SetHealth(script.curHealth);
                }
            }
            if (!Items.Contains(Item))
            {
                NewItems.Remove(Item);
            }
        }
    }
}
