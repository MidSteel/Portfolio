using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarBuilder : MonoBehaviour
{
    private enum SelectionType { Next, Previous}
    private enum PartType { Body, Wheel}

    [SerializeField] private Transform _onScreenPos = null;
    [SerializeField] private Transform _offScreenPos = null;
    [SerializeField] private GameObject[] _carBodies = null;
    [SerializeField] private GameObject[] _wheels = null;
    [SerializeField] private Button _nextButton = null;
    [SerializeField] private Button _prevButton = null;
    [SerializeField] private Button _selectionDoneButton = null;

    private PartType _currentPartType = PartType.Body;
    private int _currentCarBody = -1;
    private int _currentWheelType = -1;
    private GameObject[] _instantiatedCarBodies = null;
    private GameObject[] _instantiatedWheels = null;

    private CarInfoHolder _carInfoHolder = null;
    private CarBody _carBodyObject = null;
    private List<GameObject> _carWheelObjs = new List<GameObject>();
    private GameObject _currentlySelectedObj;

    private void Awake()
    {
        InstantiateParts();
        _carInfoHolder = FindObjectOfType<CarInfoHolder>();
        _nextButton.onClick.AddListener(() => Select(SelectionType.Next));
        _prevButton.onClick.AddListener(() => Select(SelectionType.Previous));
        _selectionDoneButton.onClick.AddListener(SelectionDone);
    }

    private void Select(SelectionType SelectionType)
    {
        if (_currentPartType == PartType.Body) { SelectCarBody(SelectionType); }
        else { SelectTire(SelectionType); }
    }

    public void SelectionDone()
    {
        var values = System.Enum.GetValues(typeof(PartType));

        if ((int)_currentPartType < values.Length - 1)
        {
            _currentPartType = (PartType)values.GetValue(((int)_currentPartType) + 1);
        }
    }

    private void SelectTire(SelectionType selectionType)
    {
        if (_carBodyObject == null) { return; }

        for (int i = 0; i < _carWheelObjs.Count; i++)
        {
            Destroy(_carWheelObjs[i].gameObject);
        }

        if (selectionType == SelectionType.Next) { _currentWheelType = _currentWheelType < _wheels.Length - 1 ? _currentWheelType + 1 : 0; }
        else { _currentWheelType = _currentWheelType > 0 ? _currentWheelType - 1 : _wheels.Length - 1; }

        _carWheelObjs.Clear();
        for (int i = 0; i < _carBodyObject.WheelsTransforms.Length; i++)
        {
            _carWheelObjs.Add(Instantiate(_wheels[_currentWheelType], _carBodyObject.WheelsTransforms[i]));
            _carWheelObjs[i].transform.localPosition = Vector3.zero;
        }
    }

    private void SelectCarBody(SelectionType selectionType)
    {
        if (_carBodyObject != null) { _carBodyObject.transform.position = _offScreenPos.position; }

        if (selectionType == SelectionType.Next) { _currentCarBody = _currentCarBody < _carBodies.Length - 1 ? _currentCarBody + 1 : 0; }
        else { _currentCarBody = _currentCarBody > 0 ? _currentCarBody - 1 : _carBodies.Length - 1; }

        _carBodyObject = _instantiatedCarBodies[_currentCarBody].GetComponent<CarBody>();
        _carBodyObject.transform.position = _onScreenPos.position;
    }

    private void InstantiateParts()
    {
        if (_offScreenPos == null) { return; }

        _instantiatedCarBodies = new GameObject[_carBodies.Length];
        _instantiatedWheels = new GameObject[_wheels.Length];

        for (int i = 0; i < _carBodies.Length; i++)
        {
            _instantiatedCarBodies[i] = Instantiate(_carBodies[i], _offScreenPos);
        }
    }
}
