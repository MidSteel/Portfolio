using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Instrument")]
public class Instrument : ScriptableObject
{
    [SerializeField] private string _instrumentName;
    [SerializeField] private Osc[] _oscilatorSettings;          //Settigns for Oscilators.
    [SerializeField] [Range(0f, 1f)] private float _lowPass;
    [SerializeField] [Range(0f, 1f)]private float _highPass;

    public Osc[] Oscilators { get { return _oscilatorSettings; } }
    public float LowPass { get { return _lowPass; } }
    public float HighPass { get { return _highPass; } }
}
