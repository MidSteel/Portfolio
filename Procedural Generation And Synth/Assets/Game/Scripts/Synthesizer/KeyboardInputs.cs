using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInputs : MonoBehaviour
{
    [SerializeField] private List<KeyCode> _inputKeys = new List<KeyCode>();

    private Dictionary<KeyCode, int> _keyNumberDictionary = new Dictionary<KeyCode, int>();

    public static KeyboardInputs instance = null;

    private void Awake()
    {
        instance = this;

        for (int i = 0; i < _inputKeys.Count; i++)
        {
            _keyNumberDictionary[_inputKeys[i]] = i;
        }
    }

    public int GetKeyInputNumber(KeyCode key)
    {
        if (_keyNumberDictionary.ContainsKey(key)) { return _keyNumberDictionary[key]; }

        return -1;
    }
}