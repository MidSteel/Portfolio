using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Test : MonoBehaviour
{
    public const float nextKeyValue = 1.05946309436f;

    [SerializeField] private float _multiplier = 2f;
    [SerializeField] private float initialNum = 0.001f;
    [SerializeField] private float constatnt = 1f;
    [SerializeField] private float ratio = 0.2f;
    [SerializeField] private float totalIterations = 100;
    //[SerializeField] private List<float> notes = new List<float>();
    [SerializeField] private List<Vector2> notes = new  List<Vector2>();
    private MusicNotesGenerator _noteGenerator = null;
    private int _noteIndex = 0;
    private float _noteChangeTimer = 0f;
    private float _noteChangeTime = 0f;

    [SerializeField] private Vector2[] testNotes = null;
    [SerializeField] private float[] testFreq = null;
    [SerializeField] private float _testStartFreq = 0f;
    [SerializeField] private Vector2 _testMinMaxTime = Vector2.zero;

    private Synthesizer _synth = null;
    private KeyNote _note = new KeyNote();

    private void Awake()
    {
        _synth = FindObjectOfType<Synthesizer>();
        _noteGenerator = this.GetComponent<MusicNotesGenerator>();
        testFreq = new float[10];
        float startIndex = _testStartFreq;

        for (int i = 0; i < 10; i++)
        {
            testFreq[i] = startIndex;
            startIndex *= nextKeyValue;
        }

        GenerateNotes();
    }

    private void GenerateNotes()
    {
        testNotes = new Vector2[50];

        for (int i = 0; i < 50; i++)
        {
            testNotes[i] = new Vector2(Random.Range(0, 10), -1);
        }

        for (int i = 1; i < 50; i +=2)
        {
            testNotes[i].y = Random.Range(_testMinMaxTime.x, _testMinMaxTime.y);
        }

        //for (int i = 3; i < 16; i += 4)
        //{
        //    testNotes[i].z = Random.Range(0, 10);
        //}

        for (int i = 0; i < 50; i++)
        {
            int index = i;

            if (testNotes[i].y == -1)
            {
                while (index < 16 && testNotes[index].y == -1)
                {
                    index++;
                }

                testNotes[i].y = (testNotes[index].y != -1) ? testNotes[index].y : 0;
            }

            index = i;

            //if (testNotes[i].z == -1) 
            //{
            //    while (index < 16 && testNotes[index].z == -1)
            //    {
            //        index++;
            //    }

            //    testNotes[i].z = (testNotes[index].z != -1) ? testNotes[index].z : 0;
            //}   
        }

        foreach (Vector2 note in testNotes)
        {
            notes.Add(new Vector2(note.x, note.y));
        }
    }

    private void Update()
    {
        _noteChangeTimer += Time.deltaTime;

        if (_noteChangeTimer > _noteChangeTime)
        {
            _noteChangeTimer = 0f;
            //_noteChangeTime = Random.Range(_testMinMaxTime.x, _testMinMaxTime.y);

            _noteChangeTime = notes[_noteIndex].y;

            if (_synth.CurrentlyActiveKeys.ContainsKey(_note.id)) { _synth.CurrentlyActiveKeys.Remove(_note.id); }
            _note.id = (int)notes[_noteIndex].x;
            _note.NoteOn = false;

            if (!_synth.CurrentlyActiveKeys.ContainsKey(_note.id)) { _synth.CurrentlyActiveKeys.Add(_note.id, _note); }
            _note.onTime = Time.time;
            _note.NoteOn = true;
            //_noteGenerator.Frequency = (testFreq[(int)notes[_noteIndex]]);

            //_noteGenerator.Frequency = this.GetComponent<MusicMaker>().Envelope.GetAmplitude(Time.deltaTime) * testFreq[(int)notes[_noteIndex]];

            _noteIndex = (_noteIndex + 1) < notes.Count ? _noteIndex + 1 : 0;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Sup", this);
    }
}
