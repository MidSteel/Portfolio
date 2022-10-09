using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ControlsEnums {Idle, Up, Down, Left, Right, Mouse, OptionsMenu }

//Very Basic Controller to move around.
public class Controls : MonoBehaviour
{
    [SerializeField] private InputActionAsset _action;
    [SerializeField] private Vector2 _mouseDir;

    public delegate void ActionPerformed(ControlsEnums control);
    public delegate void ActionCanceled(ControlsEnums control);
    public ActionPerformed onActionPerformed;
    public ActionCanceled onActionCanceld;

    public InputAction MouseAction { get; private set; }
    public InputActionAsset _Action { get { return _action; } }

    private void Awake()
    {
        _action.Enable();
        foreach (InputAction action in _action.actionMaps[0].actions)
        {
            action.performed += x => PerformAction((ControlsEnums)Enum.Parse(typeof(ControlsEnums),action.name));
            action.canceled += x => ActionOver((ControlsEnums)Enum.Parse(typeof(ControlsEnums), action.name));
        }
    }

    private void Start()
    {
        MouseAction = _action.FindAction("Mouse");
        GameManager manager = GameManager.instance;
        Camera cam = Camera.main;
    }

    public void PerformAction(ControlsEnums control)
    {
        onActionPerformed?.Invoke(control);
    }

    public void ActionOver(ControlsEnums control)
    {
        onActionCanceld?.Invoke(control);
    }
}
