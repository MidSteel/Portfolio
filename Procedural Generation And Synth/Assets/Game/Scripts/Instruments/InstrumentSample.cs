using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstrumentSample : MonoBehaviour
{
    public AllInstrumentSamples sample;
    public SampleInfo[] instruments;

    public SampleInfo Ins;

    private KeyboardInputs input = null;
    private KeyCode currentKey;
    private AudioSource audioSource = null;

    private void Awake()
    {
        Ins.SetBaseFrequency();
        audioSource = this.GetComponent<AudioSource>();
        audioSource.clip = Ins.GetNote(3);
        audioSource.Play();
    }

    private void Start()
    {
        input = KeyboardInputs.instance;
    }

    public void OnGUI()
    {
        if (Event.current.type == EventType.KeyDown)
        {
            if (Event.current.keyCode != KeyCode.None)
            {
                currentKey = Event.current.keyCode; Debug.Log(currentKey);
            }
        }
    }

    public void Update()
    {
        if (currentKey != KeyCode.None)
        {
            int currentNumber = input.GetKeyInputNumber(currentKey);
            if (currentNumber > -1 && !audioSource.isPlaying)
            {
                audioSource.clip = Ins.GetNote(currentNumber);
                if (!audioSource.isPlaying) { audioSource.Play(); }
            }
        }
    }
}

[System.Serializable]
public class SampleInfo
{
    public string instrumentName;
    public int sampleNote;
    public AudioClip sampleAudio;
    public int sampleOctave;
    public float[] BaseFrequency;

    private float _nextKey = Mathf.Pow(2f, 1f / 12f);
    private float _startingFreq = 0f;
    private AudioClip _baseNoteClip;
    private float[] audioData;

    public void SetBaseFrequency()
    {
        audioData = new float[sampleAudio.samples * sampleAudio.channels];
        sampleAudio.GetData(audioData, 0);

        _startingFreq = sampleAudio.frequency;

        if (sampleNote > 1)
        {
            while (sampleNote > 1)
            {
                _startingFreq /= _nextKey;
            }
        }

        if (sampleOctave > 1) { _startingFreq /= (sampleOctave - 1); }
        _baseNoteClip = AudioClip.Create(instrumentName, sampleAudio.samples, sampleAudio.channels, (int)_startingFreq, false);
        _baseNoteClip.SetData(audioData, 0);
    }

    public AudioClip GetNote(int note)
    {
        float freq = _baseNoteClip.frequency;

        for (int i = 0; i < note; i++)
        {
            freq *= _nextKey;
        }

        AudioClip tempAudio = AudioClip.Create(instrumentName + " note", _baseNoteClip.samples, _baseNoteClip.channels, (int)freq, false);
        tempAudio.SetData(audioData, 0);
        return tempAudio;
    }
}
