using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Keys : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
{
    [SerializeField] private int _keyNumber = -1;

    private Synthesizer _synth = null;
    private KeyNote _note;

    private void Awake()
    {
        _synth = FindObjectOfType<Synthesizer>();
        _note = new KeyNote() { id = _keyNumber };
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (_keyNumber < 0) { return; }

        _note.onTime = Time.time;
        _note.NoteOn = true;
        if (!_synth.CurrentlyActiveKeys.ContainsKey(_keyNumber))
        {
            _synth.CurrentlyActiveKeys.Add(_keyNumber, _note);
        }
    }

    public void OnPointerUp(PointerEventData data)
    {
        if (_keyNumber < 0) { return; }

        _note.offTime = Time.time;
        _note.NoteOn = false;
    }
}
