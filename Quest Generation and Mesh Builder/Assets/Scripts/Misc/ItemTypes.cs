using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Types", menuName = "Misc/Item Types List")]
public class ItemTypes : ScriptableObject
{
    [SerializeField] private List<EntityType> _itemTypeList = new List<EntityType>();

    public List<EntityType> ItemTypeList { get { return _itemTypeList; } }

    public GameObject GetItemObject(string type)
    {
        foreach (EntityType t in _itemTypeList)
        {
            if (t.entityType == type) { return t.typeObject; }
        }

        return null;
    }

    public EntityType GetItem(int itemId)
    {
        foreach (EntityType t in _itemTypeList)
        {
            if (t.entityId == itemId) { return t; }
        }

        return new EntityType();
    }
}
