using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgorithmicMusic : MonoBehaviour
{
    public Scale cromaticScale = new Scale(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 });
    public Scale majorScale = new Scale(new int[] { 0, 2, 4, 5, 7, 9, 11 });
}

public struct Scale
{
    public int[] keyNumbers;

    public Scale(int[] keyNumbers)
    {
        this.keyNumbers = keyNumbers;
    }
}

public struct ToneScale
{
    public Scale scale;
    public int root;
}

public struct Note
{
    private enum NoteType { Normal, DotNote, Triplet} //Dot Note :- Note that has 1.5 times the assigned duration\\ Triplet three notes With Same duration and signature.

    public int frequency;
    public float duration;
    public Vector2 velocity;
}

public struct Melody
{
    public Scale scale;
    public float duration;
    public int signature;
    public int numberOfNotes;
    public Note[] melodyNotes;

    public void GenerateMelody()
    {

    }
}

public struct Rhythm
{
    public int normalNotesProbability;
    public int dotNotesProbability;
    public int trippletNotesProbability;
    public int rangeMinvalue;
    public int rangeMaxValue;
    public float[] rhythmicPattern;

    public void GenerateRhythm(int signature, int bpm)
    {

    }

    public void GenerateRhythmicPattern()
    {

    }
}