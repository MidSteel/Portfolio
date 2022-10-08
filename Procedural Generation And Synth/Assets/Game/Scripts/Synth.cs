using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Synth : MonoBehaviour
{

    [SerializeField] private Instrument _insrument;
    [SerializeField] private OctaveCreator _octaveCreator;

    private int _sampleRate = 44100;
    private KeyboardInputs _keys = null;
    private Dictionary<KeyCode, int> _currentlyActiveKeys = new Dictionary<KeyCode, int>();
    private float _increment = 0f;
    private float _phase;
    public List<AudioClip> _audioSamples;
    private float[] _originalClipData;
    private AudioSource _audioSource;

    private void Start()
    {
        _keys = KeyboardInputs.instance;
        _increment = (Mathf.PI * 2f) / _sampleRate;
        _audioSource = this.GetComponent<AudioSource>();
        _audioSamples = _octaveCreator.CreateOctaves();
        _originalClipData = new float[_octaveCreator.originalClip.samples * _octaveCreator.originalClip.channels];
        _octaveCreator.originalClip.GetData(_originalClipData, 0);
        //_originalClipData = new float[_octaveCreator.originalClip.length * _octaveCreator.originalClip.channels];
        _audioSource.clip = _audioSamples[2];
        _audioSource.Play();
    }

    private void OnGUI()
    {
        if (Event.current.type == EventType.KeyDown)
        {
            KeyCode key = Event.current.keyCode;
            int keyNumber = _keys.GetKeyInputNumber(key);
            if (!_currentlyActiveKeys.ContainsKey(key) && keyNumber > -1)
            {
                _currentlyActiveKeys.Add(key, keyNumber);
            }
        }
        else if (Event.current.type == EventType.KeyUp)
        {
            KeyCode key = Event.current.keyCode;

            if (_currentlyActiveKeys.ContainsKey(key)) { _currentlyActiveKeys.Remove(key); }
        }

        //if (_currentlyActiveKeys.Count > 0) { _audioSource.clip = _audioSamples[_currentlyActiveKeys.ElementAt(0).Value]; _audioSource.Play(); }
        //else { _audioSource.Stop(); }
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        //for (int i = 0; i < data.Length; i++)
        //{
        //    data[i] += _originalClipData[i];
        //}
    }
}
