using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "All Instrument samples List")]
public class AllInstrumentSamples : ScriptableObject
{
    public AudioClip clip;

    public void ChangeClip(AudioClip c)
    {
        clip = AudioClip.Create(c.name, c.samples, c.channels, c.frequency, false);
    }
}
