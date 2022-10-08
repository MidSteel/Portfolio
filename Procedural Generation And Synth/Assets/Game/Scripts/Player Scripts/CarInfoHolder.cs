using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInfoHolder : MonoBehaviour
{
    [SerializeField] private Transform _carBodyTransform = null;

    public Transform CarBodyTransform { get { return _carBodyTransform; } }
}
