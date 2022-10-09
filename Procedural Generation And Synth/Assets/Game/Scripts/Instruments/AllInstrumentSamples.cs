using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "All Instrument samples List")]
public class AllInstrumentSamples : ScriptableObject
{
    public AudioClip clip; //Audio Clip to use as an instrument.

    public void ChangeClip(AudioClip c)
    {
        clip = AudioClip.Create(c.name, c.samples, c.channels, c.frequency, false); //To Change Frequency of the clip to generate different notes.
    }
}
