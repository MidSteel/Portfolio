using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Oscilator : MonoBehaviour
{
    private const int _SampleRate = 48000;
    private const short _BitsPerSample = 16;

    [SerializeField] private float _gain = 0f;
    [SerializeField] private WaveForm waveForm = WaveForm.Sin;
    [SerializeField] private Vector2 _minMaxTime = Vector2.zero;

    private float _timer = 0f;
    private float _time = 0f;
    [SerializeField] private float[] wave = null;
    private float[] wave2 = null;
    private float[] wave3 = null;
    private float freq = 0f;
    private int freqIndex = 0;

    private float[] _baseFreq = null;

    private List<KeyCode> curremtlyActiveKeys = new List<KeyCode>();

    private float freq2 = 0f;

    private void Awake()
    {
        //float nextKey = Mathf.Pow(2f, 1 / 12f);
        //_baseFreq = new float[12];
        //float f = 440f;
        //for (int i = 0; i < 12; i++)
        //{
        //    f *= nextKey;
        //    _baseFreq[i] = f;
        //}

        //GenerateMelody();
        //_time = Random.Range(_minMaxTime.x, _minMaxTime.y);

        //keyBoolDictionary.Add(KeyCode.V, false);
        //keyBoolDictionary.Add(KeyCode.D, false);
        //keyBoolDictionary.Add(KeyCode.S, false);
        //keyBoolDictionary.Add(KeyCode.P, false);
        //keyBoolDictionary.Add(KeyCode.M, false);
        //keyBoolDictionary.Add(KeyCode.J, false);
        //keyBoolDictionary.Add(KeyCode.H, false);
        //keyBoolDictionary.Add(KeyCode.C, false);
        //keyBoolDictionary.Add(KeyCode.Alpha0, false);
    }

    //private KeyCode curKey;

    private void OnGUI()
    {
        if (Event.current != null && Event.current.type == EventType.KeyDown)
        {
            KeyCode key = Event.current.keyCode;
            if (key != KeyCode.None)
            {
                if (!curremtlyActiveKeys.Contains(key)) curremtlyActiveKeys.Add(key);
            }
        }

        if (Event.current.type == EventType.KeyUp)
        {
            KeyCode curKey = Event.current.keyCode;

            if (curremtlyActiveKeys.Contains(curKey)) { curremtlyActiveKeys.Remove(curKey); }
        }
    }

    private void Update()
    {
        //if (_timer > _time)
        //{
        //    _time = Random.Range(_minMaxTime.x, _minMaxTime.y);
        //    freqIndex = (freqIndex + 1 >= wave.Length) ? 0 : freqIndex + 1;
        //    freq = wave[freqIndex];
        //    _timer = 0f;
        //}

        //_timer += Time.deltaTime;
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

    private float phase;
    private float increment;

    private void OnAudioFilterRead(float[] data, int channels)
    {
        increment = Mathf.PI * 2f / _SampleRate;
        for (int i = 0; i < data.Length; i += channels)
        {
            phase += increment;

            for (int j = 0; j < curremtlyActiveKeys.Count; j++)
            {
                //if (curremtlyActiveKeys[j] != KeyCode.None)
                //{
                float sawFreq = GetFreq(curremtlyActiveKeys[j]) * 0.4f;
                //data[i] += /*0.2f **/ ((sawFreq / (2f / Mathf.PI)) * /*Mathf.PI **/ (phase % (1f / sawFreq / (2f * Mathf.PI))));
                //data[i] += 0.05f * (sawFreq / (2f / Mathf.PI)) /** Mathf.PI **/ * (phase % (1f / (sawFreq / (2f * Mathf.PI))));
                //data[i] += 2f * Mathf.Sin(phase * GetFreq(curremtlyActiveKeys[j]) * 2f);
                //data[i] += Mathf.Sin(phase * (GetFreq(curremtlyActiveKeys[j])) * .9f);
                //data[i] += /*0.7f*/ 0.3f * Mathf.Sign(Mathf.Sin((GetFreq(curremtlyActiveKeys[j]) * .2f) * phase));
                //data[i] += 0.5f * Mathf.Sign(Mathf.Sin((GetFreq(curremtlyActiveKeys[j]) * 2f) * phase));
                data[i] += 2f * Mathf.Asin(Mathf.Sin((GetFreq(curremtlyActiveKeys[j]) ) * phase));
                data[i] += .5f * Mathf.Asin(Mathf.Sin((GetFreq(curremtlyActiveKeys[j]) * 0.5f) * phase));
                data[i] += .5f * Mathf.Asin(Mathf.Sin((GetFreq(curremtlyActiveKeys[j]) * 0.65f) * phase));
                //data[i] += 2f * Mathf.Asin(Mathf.Sin((GetFreq(curremtlyActiveKeys[j]) * .7f /*0.6f*/) * phase));
                //}




            }

            data[i] *= _gain;
            //data[i] = (Mathf.Sign(Mathf.Sin(phase * freq) /*+ Mathf.Sin((phase * wave2[freqIndex]))*/ /*+ Mathf.Sin(phase * wave3[freqIndex] *//** 0.3f*/) ) /*+ Mathf.Sin(phase * (freq * 0.5f))*//*)*/* _gain;
            if (channels == 2) { data[i + 1] = data[i]; }
            if (phase > Mathf.PI * 2) { phase = 0f; }
        }

        //increment = 2f * Mathf.PI / _SampleRate;

        //for (int i = 0; i < data.Length; i += channels)
        //{
        //    _phase += _increment;
        //    data[i] = Oscilator(_waveForm, _phase, _frequency);

        //    if (channels == 2)
        //    {
        //        data[i + 1] = data[i];
        //    }
        //    if (_phase > Mathf.PI * 2)
        //    {
        //        _phase = 0f;
        //    }
        //}
    }

    private void GenerateMelody()
    {
        wave = new float[Random.Range(30, 60)];
        wave2 = wave3 = new float[wave.Length];
        float nextKey = Mathf.Pow(2f, 1 / 12f);
        int newFreq = 0;
        int newFreq2 = 0;
        int newFreq3 = 0;
        int octave = 4;

        for (int i = 0; i < wave.Length; i++)
        {
            if (i > 0)
            {
                int rF = Random.Range(0, 6);
                int IF = Random.Range(0, 3);

                if (rF <= 2)
                {
                    newFreq = Random.Range(0, 12);
                    newFreq2 = Random.Range(0, 12);
                    newFreq3 = Random.Range(0, 12);

                }
                else if (rF <= 5)
                {
                    newFreq ++;
                    newFreq2 ++;
                    newFreq3 ++;
                    if (newFreq > 11) { newFreq = 0; }
                    if (newFreq2 > 11) { newFreq2 = 0; }
                    if (newFreq3 > 11) { newFreq3 = 0; }
                }
                else
                {
                    newFreq --;
                    newFreq2 --;
                    newFreq3 --;
                    if (newFreq < 0) { newFreq = 11; }
                    if (newFreq2 < 0) { newFreq2 = 11; }
                    if (newFreq3 < 0) { newFreq3 = 11; }
                }
            }
            else
            {
                newFreq = Random.Range(0, 12);
                newFreq2 = Random.Range(0, 12);
                newFreq3 = Random.Range(0, 12);
            }

            wave[i] = _baseFreq[newFreq] + 12 * octave;
            wave2[i] = _baseFreq[newFreq2] + 12 * (octave / 2);
            wave3[i] = _baseFreq[newFreq3] + 12 * (octave + 1);
        }
    }
}
