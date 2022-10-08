using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private int _id = -1;

    public int Id { get { return _id; } set { _id = value; } }
}
