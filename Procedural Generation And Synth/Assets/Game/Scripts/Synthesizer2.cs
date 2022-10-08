using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Synthesizer2 : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    //[SerializeField] private OctaveCreator _octaveCreator;

    [ContextMenu("AudioData")]
    public void SetAudioData()
    {
        Debug.Log(Mathf.Pow(2f, 1f / 12f));

        float[] audioData = new float[clip.samples * clip.channels];
        clip.GetData(audioData, 0);
        AudioClip newClip = AudioClip.Create("Test", audioData.Length, clip.channels, clip.frequency * 2, false);
        newClip.SetData(audioData, 0);
        //for (int i = 0; i < audioData.Length; i += clip.channels)
        //{
        //    clip.frequency

        //    if (clip.channels == 2) { audioData[i + 1] = audioData[i]; }
        //}

        clip.SetData(audioData, 0);

        this.GetComponent<AudioSource>().clip = newClip;
        this.GetComponent<AudioSource>().Play();
    }

    [ContextMenu("Octaves")]
    public void CreateOctave()
    {
        //_octaveCreator.CreateOctaves();
    }
}
