using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;
using System.Threading;

//[RequireComponent(typeof(AudioLowPassFilter), typeof(AudioHighPassFilter))]
public class Synthesizer : MonoBehaviour
{
    public float test;

    [SerializeField] private WaveForm _wave;
    [SerializeField] private float _frequency = 440f;
    [SerializeField][Range(0f, 1f)] private float _volume = 0.5f;
    [Header("=========Instrument=========")]
    [SerializeField] private Instrument _instrument;
    
    private float[] _baseFrequencies = null;

    private const float _sampleRate = 48000f;

    private float _increment = 0f;
    private float _phase = 0f;
    private Random random = new Random();
    private float time = 0f;
    private Dictionary<int, KeyNote> _currentlyActiveKeys = new Dictionary<int, KeyNote>();

    private List<int> _keysBeingReleased = new List<int>();
    private KeyboardInputs _inputs = null;
    private Queue<int> _removeNotesQueue = new Queue<int>();

    public Dictionary<int, KeyNote> CurrentlyActiveKeys { get { return _currentlyActiveKeys; } }

    private void Awake()
    {
        float keyDistance = Mathf.Pow(2f, (1f / 12f));
        float firstKey = 440f * Mathf.Pow(2f, (1f - 49f) / 12f);
        _baseFrequencies = new float[12];
        _baseFrequencies[0] = firstKey;
        
        for (int i = 1; i < 12; i++)
        {
            _baseFrequencies[i] = _baseFrequencies[i - 1] * keyDistance;
        }

        foreach (Osc oscilator in _instrument.Oscilators)
        {
            oscilator.OnStart(_baseFrequencies);
        }
    }

    private void Start()
    {
        _inputs = KeyboardInputs.instance;
    }

    private void Update()
    {
        time = Time.time;
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        _increment = (2f * Mathf.PI) / _sampleRate;

        for (int i = 0; i < data.Length; i += channels)
        {
            _phase += _increment;

            for (int osc = 0; osc < _instrument.Oscilators.Length; osc++)
            {
                for (int j = 0; j < _currentlyActiveKeys.Count; j++)
                {
                    KeyNote note = _currentlyActiveKeys.ElementAt(j).Value;
                    for (int wave = 0; wave < _instrument.Oscilators[osc].waveAmps.Length; wave++)
                    {
                        WaveAmps w = _instrument.Oscilators[osc].waveAmps[wave];
                        
                        float amplitude = _instrument.Oscilators[osc].envelope.GetAmplitude(time, note.onTime, note.offTime, note.NoteOn);
                        data[i] += Oscilator(_phase, w.wave, _instrument.Oscilators[osc].GetFrequency(note.id) * w.frequencyMultiplier, amplitude * w.waveAmplitude, w.LFOFrequency, w.LFOAmplitude);
                        
                        //if (i + 10 < data.Length - 1) { data[i + 10] += (data[i] * test); }
                    }
                }
            }

            if (channels == 2) { data[i + 1] = data[i]; }
            if (_phase > Mathf.PI * 2f) { _phase = 0f; }
        }
    }

    private void OnGUI()
    {
        if (Event.current != null && Event.current.type == EventType.KeyDown)
        {
            KeyCode key = Event.current.keyCode;

            if (key != KeyCode.None)
            {
                if (Input.GetKeyDown(key))
                {
                    int keyNumber = _inputs.GetKeyInputNumber(key);

                    if (!_currentlyActiveKeys.ContainsKey(keyNumber))
                    {
                        _currentlyActiveKeys.Add(keyNumber, new KeyNote() { id = keyNumber, onTime = time, NoteOn = true });
                    }
                    else
                    {
                        _currentlyActiveKeys[keyNumber].SetOnTime(time);
                        _currentlyActiveKeys[keyNumber].SetNoteState(true);
                    }
                }
            }
        }

        if (Event.current.type == EventType.KeyUp)
        {
            KeyCode curKey = Event.current.keyCode;
            int curKeyNum = _inputs.GetKeyInputNumber(curKey);

            if (_currentlyActiveKeys.ContainsKey(curKeyNum))
            {
                _currentlyActiveKeys[curKeyNum].SetOffTime(time);
                _currentlyActiveKeys[curKeyNum].SetNoteState(false);
            }
        }
    }

    private float GetFreq(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.V:
                return 493.88f;
            case KeyCode.D:
                return 415.30f;
            case KeyCode.S:
                return 369.99f;
            case KeyCode.P:
                return 329.66f;
            case KeyCode.M:
                return 659.25f;
            case KeyCode.J:
                return 622.25f;
            case KeyCode.H:
                return 554.39f;
            case KeyCode.C:
                return 440f;
            case KeyCode.Alpha0:
                return 311.13f;
            default:
                return 0f;
        }
    }

    public float Oscilator(float phase, WaveForm wave, float frequency, float amplitude, float LFOFrequency = 0f, float LFOAmplitude = 0f)
    {
        float sound = 0f;
        float freq = (phase * frequency) + LFOAmplitude * frequency * Mathf.Sin(LFOFrequency * phase);

        switch (wave) {
            case WaveForm.Sin:
                sound = Mathf.Sin (freq);
                break;
            case WaveForm.Square:
                sound = Mathf.Sign(Mathf.Sin(freq));
                break;
            case WaveForm.Triangle:
                sound = Mathf.Asin(Mathf.Sin(freq));
                break;
            case WaveForm.Saw:
                sound = 0.1f * ((frequency / (2f / Mathf.PI)) * ((phase / 6.28f) % (1f / (frequency / (2f * Mathf.PI)))));
                break;
            case WaveForm.Noise:
                sound = Mathf.Sin(random.Next());
                break;
        }

        return sound * _volume * amplitude;
    }
}

[Serializable]
public class Osc
{
    public WaveAmps[] waveAmps;
    public Envelope envelope;
    public float[] baseKeyFrequencies;

    [SerializeField] private int _octave;

    private float _octaveMultiplier;

    public int Octave { get { return _octave; } set { _octave = value; _octaveMultiplier = Mathf.Pow(2, _octave); } }

    public void OnStart(float[] baseKeys)
    {
        this.baseKeyFrequencies = new float[12];

        for (int i = 0; i < 12; i++)
        {
            this.baseKeyFrequencies[i] = baseKeys[i];
        }
        Octave = _octave;
    }

    public float GetFrequency(int keyNumber)
    {
        if (keyNumber < 0) { return 0f; }

        if (keyNumber < 12)
        {
            return baseKeyFrequencies[keyNumber] * _octaveMultiplier;
        }
        else
        {
            int mod = keyNumber % 12;
            int keyOctave = keyNumber / 12;
            float multiplier = Mathf.Pow(2, keyOctave + _octave);
            float multipliedFrequency = baseKeyFrequencies[mod] * (multiplier);
            return multipliedFrequency;
        }
    }
}
[Serializable]
public class KeyNote
{
    public int id;
    public float amplitude;
    public float onTime;
    public float offTime;
    public bool NoteOn { get; set; }

    public void SetOnTime(float time)
    {
        onTime = time;
    }

    public void SetOffTime(float time)
    {
        offTime = time;
    }

    public void SetNoteState(bool state)
    {
        NoteOn = state;
    }
}

[Serializable] public struct WaveAmps 
{
    public WaveForm wave; 
    public float waveAmplitude; 
    public float frequencyMultiplier;
    public float LFOFrequency;
    public float LFOAmplitude;
}

public enum Key { A, ASharp, B, C, CSharp, D, DSharp, E, F, FSharp, G, GSharp}

[Serializable]
public struct Envelope
{
    public float attackTime;
    public float decayTime;
    public float releaseTime;
    public float sustainAmplitude;
    public float startAmplitude;
    
    public bool ShouldTurnOff { get; private set; }
    
    private  float _triggerOnTime;
    private float _triggerOffTime;
    private bool _noteOn;
    private float _releaseAmplitude;

    private Thread _computationThread;

    public float GetAmplitude(float time, float onTime, float offTime = 0f, bool noteOn = true)
    {
        float amplitude = 0f;
        float lifeTime = time - onTime;
        Envelope env = this;
        if (noteOn)
        {
            if (lifeTime <= attackTime)
            {
                amplitude = (lifeTime / attackTime) * startAmplitude;
            }
            else if (lifeTime > attackTime && lifeTime <= (attackTime + decayTime))
            {
                amplitude = ((lifeTime - attackTime) / decayTime) * (sustainAmplitude - startAmplitude) + startAmplitude;
            }
            else if (lifeTime > (attackTime + decayTime))
            {
                amplitude = sustainAmplitude;
            }

        }
        else
        {
            if (lifeTime <= attackTime) { _releaseAmplitude = (lifeTime / attackTime) * startAmplitude; }
            else if (lifeTime > attackTime && lifeTime <= (attackTime + decayTime)) { _releaseAmplitude = ((lifeTime - attackTime) / decayTime) * (sustainAmplitude - startAmplitude) + startAmplitude; }
            else if (lifeTime > (attackTime + decayTime)) { _releaseAmplitude = sustainAmplitude; }

            amplitude = ((time - offTime) / releaseTime) * (0f - _releaseAmplitude) + _releaseAmplitude;
        }

        if (amplitude <= 0.0001f)
        {
            amplitude = 0f;
            ShouldTurnOff = true;
        }

        return amplitude;
    }

    public void NoteOn(float time)
    {
        _triggerOnTime = time;
        _noteOn = true;
    }

    public void NoteOff(float time)
    {
        _triggerOffTime = time;
        _noteOn = false;
    }
}
