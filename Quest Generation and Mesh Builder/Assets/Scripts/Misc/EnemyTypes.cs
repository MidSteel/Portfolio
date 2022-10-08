using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Types", menuName = "Misc/Enemy Types List")]
public class EnemyTypes : ScriptableObject
{
    [SerializeField] private List<EntityType> _enemyTypeList = new List<EntityType>();

    public List<EntityType> EnemyTypeList { get { return _enemyTypeList; } }

    public GameObject GetEnemyObject(string type)
    {
        foreach (EntityType t in _enemyTypeList)
        {
            if (t.entityType == type) { return t.typeObject; }
        }

        return null;
    }
    
    public EntityType GetEnemy(int enemyId)
    {
        foreach (EntityType t in _enemyTypeList)
        {
            if (t.entityId == enemyId) { return t; }
        }

        return new EntityType();
    }
}

[System.Serializable]
public struct EntityType
{
    public int entityId;
    public string entityType;
    public GameObject typeObject;
}