using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicMaker : MonoBehaviour
{
    [SerializeField] private int _octaves = 2;
    [SerializeField] private float _startingFreq;
    //<Temp>
    [SerializeField] private KeyCode[] Keycodes = null;
    private Dictionary<KeyCode, float> keyFrequencies = new Dictionary<KeyCode, float>();
    //</Temp>
    [Header("Envolope====================================")] [SerializeField] private EnvelopeADSR _envelope = new EnvelopeADSR();
    private MusicNotesGenerator _musicNoteGenerator = null;

    public float[] fractals = null;
    public int fractalIndex = 0;
    public float timer = Mathf.Infinity;
    public float time = 0f;

    public int index = 0;
    float[,] mapValues = null;
    float[] frequncies;
    float[] amplitudes;

    public EnvelopeADSR Envelope { get { return _envelope; } }

    private void Awake()
    {
        fractals = new float[1000];
        float z = 3;
        float c = 2;

        for (int i = 0; i < 1000; i++)
        {
            fractals[i] = z;
            z = (z * z) + c;
        }

        _musicNoteGenerator = this.GetComponent<MusicNotesGenerator>();
        _musicNoteGenerator.Envelope = _envelope;
    }

    private void Start()
    {
        _musicNoteGenerator.Frequencies = new float [_octaves * 12];
        float freq = _startingFreq;
        int index = 0;

        for (int i = 0; i < _octaves; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                _musicNoteGenerator.Frequencies[index] = freq; 
                keyFrequencies.Add(Keycodes[index], freq);
                freq *= MusicNotesGenerator.RatioBetweenNoteFrequncies;
                index++;
            }
        }
    }

}

[System.Serializable]
public struct EnvelopeADSR
{
    public float attackTime;
    public float decayTime;
    public float sustainAmplitude;
    public float releaseTime;
    public float startAmplitude;
    public float triggerOnTime;
    public float triggerOffTime;
    public bool noteOn;

    public float GetAmplitude(float time)
    {
        float amplitude = 0f;
        float lifeTime = time - triggerOnTime;

        if (noteOn)
        {
            if (lifeTime <= attackTime)
            {
                amplitude = (lifeTime / attackTime) * startAmplitude;
            }
            if (lifeTime > attackTime && lifeTime <= (decayTime + attackTime))
            {
                amplitude = ((lifeTime - attackTime) / decayTime) * (sustainAmplitude - startAmplitude) + startAmplitude;
            }
            if (lifeTime > (attackTime + decayTime))
            {
                amplitude = sustainAmplitude;
            }
        }
        else
        {
            amplitude = (time - triggerOffTime);
        }

        if (amplitude <= 0.0001f)
        {
            amplitude = 0f;
        }

        return amplitude;
    }

    public void NoteOn(float timeOn)
    {
        triggerOnTime = timeOn;
        noteOn = true;
    }

    public void NoteOff(float timeOff)
    {
        triggerOffTime = timeOff;
        noteOn = false;
    }
}