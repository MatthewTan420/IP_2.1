using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item/Create New Item")]

public class Item : ScriptableObject
{
    /// <summary>
    /// Item variables
    /// </summary>

    public int id;
    public string itemName;
    public int value;
}
