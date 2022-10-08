using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBody : MonoBehaviour
{
    [SerializeField] private Transform[] _wheelsTransforms = null;

    public Transform[] WheelsTransforms { get { return _wheelsTransforms; } }
}
