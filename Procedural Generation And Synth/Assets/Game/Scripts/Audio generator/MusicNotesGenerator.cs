using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WaveForm { Sin, Square, Triangle, Saw, Noise}

[RequireComponent(typeof(AudioSource))]
public class MusicNotesGenerator : MonoBehaviour
{
    public const float RatioBetweenNoteFrequncies = 1.05946309436f;

    [SerializeField] private WaveForm _waveForm = WaveForm.Sin;
    [SerializeField] private float _frequency = 440f;
    [SerializeField] private float _gain = 0f;
    [SerializeField] [Range(0f, 1.0f)] private float _volume = 0.1f;
    [SerializeField] private float _noiseRatio = 0.5f;
    [SerializeField] [Range(-1f, 1f)] private float _offset = 0f;

    private float _increment;
    private float _phase;
    private float _sampleRate = 48000f;
    private float[] _frequncies;
    private int _thisFreq;

    public float Frequency { get { return _frequency; } set { _frequency = value; } }
    public float Gain { get { return _gain; } set { _gain = value; } }
    public float Increment { get { return _increment; } set { _increment = value; } }
    public float Phase { get { return _phase; } set { _phase = value; } }
    public float SampleRate { get { return _sampleRate; } set { _sampleRate = value; } }
    public float[] Frequencies { get { return _frequncies; } set { _frequncies = value; } }
    public int ThisFreq { get { return _thisFreq; } set { _thisFreq = value; } }
    public float Volume { get { return _volume; } }
    public EnvelopeADSR Envelope { get; set; }


    private void Update()
    {
        _gain = _volume;
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        _increment =  2f * Mathf.PI / _sampleRate;

        for (int i = 0; i < data.Length; i += channels)
        {
            _phase += _increment;
            data[i] = Oscilator(_waveForm, _phase, _frequency);

            if (channels == 2)
            {
                data[i + 1] = data[i];
            }
            if (_phase > Mathf.PI * 2)
            {
                _phase = 0f;
            }
        }
    }

    private float Oscilator(WaveForm wave, float phase, float frequency)
    {
        float note = 0f;

        switch (wave)
        {
            case WaveForm.Sin:
                note =  Mathf.Sin(phase * frequency) ;
                break;
            case WaveForm.Square:
                note = (Mathf.Sin(phase * frequency) > 0f) ? 1f : -1f;
                break;
            case WaveForm.Triangle:
                note = Mathf.Asin(Mathf.Sin(phase * frequency)) * 2f / Mathf.PI;
                break;
        }

        return _gain * (Envelope.GetAmplitude(phase) * note + Envelope.GetAmplitude(Phase) * note);
    }
}
