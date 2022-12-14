using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshGenerator))]
public class MeshGeneratorEditor : Editor
{
    private MeshGenerator _target = null;
    private int _hoveringOverPointIndex = -1;
    private bool _dragPoint = false;
    private bool _attachToAnExistingPoint = false;

    public void OnSceneGUI()
    {
        _target = (MeshGenerator)target;
        Event guiEvent = Event.current;
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);
        float drawPlaneHeight = 0;
        float dstToDrawPlane = 5f;
        Vector3 mousePosition = (mouseRay.GetPoint(dstToDrawPlane));

        _hoveringOverPointIndex = CheckIfMouseIsHoveringOverPoints(mousePosition);

        if (guiEvent.type == EventType.KeyDown && guiEvent.keyCode == KeyCode.LeftShift) { _attachToAnExistingPoint = true; }
        if (guiEvent.type == EventType.KeyUp && guiEvent.keyCode == KeyCode.LeftShift) { _attachToAnExistingPoint = false; }

        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0)
        {
            if (_hoveringOverPointIndex == -1)
            {
                StopDraggingPoint();
                _target.VertPoints.Add(mousePosition);
                _target.TriangleIndex.Add(_target.GetNextPointIndex);
                if (_target.TriangleIndex.Count % 3 == 0) { _target.MeshGeneration(); }
            }
            else
            {
                if (_attachToAnExistingPoint)
                {
                    _target.TriangleIndex.Add(_hoveringOverPointIndex);
                    if (_target.TriangleIndex.Count % 3 == 0) { _target.MeshGeneration(); }
                }
                else
                {
                    _dragPoint = !_dragPoint;
                }
            }
        }

        for (int i = 0; i < _target.VertPoints.Count; i++)
        {
            if (i == _hoveringOverPointIndex) { Handles.color = Color.cyan; }
            else { Handles.color = Color.white; }
            Handles.DrawWireCube(_target.VertPoints[i], Vector3.one * 0.5f);
        }

        if (guiEvent.type == EventType.Layout)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        }

        if (_dragPoint)
        {
            StartDraggingPoint(mousePosition);
            _target.MeshGeneration();
        }
    }

    public void StartDraggingPoint(Vector3 mousePosition)
    {
        if (_hoveringOverPointIndex == -1) { return; }

        _target.VertPoints[_hoveringOverPointIndex] = mousePosition;
    }

    public void StopDraggingPoint()
    {
        _hoveringOverPointIndex = -1;
        _dragPoint = false;
    }

    public int CheckIfMouseIsHoveringOverPoints(Vector3 mousePos)
    {
        if (_hoveringOverPointIndex != -1 && _dragPoint) { return _hoveringOverPointIndex; }
        int mouseOverPointIndex = -1;
        for (int i = 0; i < _target.VertPoints.Count; i++)
        { 
            if (Vector3.Distance(mousePos, _target.VertPoints[i]) <= 0.5f)
            {
                mouseOverPointIndex = i;
                break;
            }
        }
        return mouseOverPointIndex;
    }
}
