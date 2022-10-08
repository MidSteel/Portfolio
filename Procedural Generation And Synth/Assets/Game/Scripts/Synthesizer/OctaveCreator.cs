using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Octave Creator", menuName = "Octave Creator")]
public class OctaveCreator : ScriptableObject
{
    public short keysInOctave;
    public short clipOctave;
    public short clipKey;
    public AudioClip originalClip;
    public List<float[]> audioDatas;

    public List<AudioClip> CreateOctaves()
    {
        audioDatas = new List<float[]>();
        List<AudioClip> baseOctaveClips = new List<AudioClip>();
        float divider = clipOctave * 2f;
        float[] audioData = new float[originalClip.samples * originalClip.channels];
        originalClip.GetData(audioData, 0);

        for (int i = 0; i < keysInOctave; i++)
        {
            if (i < clipKey)
            {
                AudioClip newClip = AudioClip.Create(i.ToString(), audioData.Length, originalClip.channels, originalClip.frequency / i, false);
                baseOctaveClips.Add(newClip);
                newClip.SetData(audioData, 0);
            }
            else if (i > clipKey)
            {
                AudioClip newClip = AudioClip.Create(i.ToString(), audioData.Length, originalClip.channels, originalClip.frequency / i, false);
                baseOctaveClips.Add(newClip);
                newClip.SetData(audioData, 0);
            }
            else { baseOctaveClips.Add(originalClip); }
        }

        return baseOctaveClips;
    }
}
