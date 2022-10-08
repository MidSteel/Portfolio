using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private ItemTypes _itemTypes = null;
    [SerializeField] private List<int> _inventoryItems = new List<int>();

    public delegate void OnItemAdded(int id);
    public OnItemAdded onItemAdded;

    public void AddItemToInventory(int id)
    {
        _inventoryItems.Add(id);
        onItemAdded?.Invoke(id);
    }

    public int ContainsItem(int id)
    {
        int itemsWithId = 0;

        foreach (int itemid in _inventoryItems)
        {
            if (itemid == id) { itemsWithId++; }
        }

        return itemsWithId;
    }
}
