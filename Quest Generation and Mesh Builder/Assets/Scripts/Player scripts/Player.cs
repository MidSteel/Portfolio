using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;

    private Controls _controls = null;
    private GameManager _manager = null;
    private UIManager _uiManager = null;
    private short _moveDirectionX = 0;
    private short _moveDirectionY = 0;
    private Vector2 _mouseDir = Vector2.zero;
    private EnemyKiller _enemyKiller = null;

    public EnemyKiller _EnemyKiller { get { return _enemyKiller; } }
    public PlayerInventory PlayerInventory { get; private set; }

    private void Start()
    {
        _enemyKiller = this.GetComponent<EnemyKiller>();
        PlayerInventory = this.GetComponent<PlayerInventory>();
        _controls = this.GetComponent<Controls>();
        _controls.onActionPerformed += Input;
        _controls.onActionCanceld += InputCanceled;
        _manager = GameManager.instance;
        _uiManager = UIManager.instance;
    }

    public void Input(ControlsEnums control)
    {
        switch(control)
        {
            case ControlsEnums.Up:
                InputProcess(control);
                break;
            case ControlsEnums.Down:
                InputProcess(control);
                break;
            case ControlsEnums.Left:
                InputProcess(control);
                break;
            case ControlsEnums.Right:
                InputProcess(control);
                break;
            case ControlsEnums.OptionsMenu:
                InputProcess(control);
                break;
        }
    }

    public void InputCanceled(ControlsEnums control)
    {
        _manager.onUpdate -= MovePlayer;
        
        if (control == ControlsEnums.Up || control == ControlsEnums.Down)
        {
            _moveDirectionX = 0;
        }

        if (control == ControlsEnums.Right || control == ControlsEnums.Left)
        {
            _moveDirectionY = 0;
        }
    }

    public void InputProcess(ControlsEnums control)
    {
        bool addToUpdate = false;

        if (control == ControlsEnums.Up) 
        {
            _moveDirectionX = 1;
            addToUpdate = true;
        }
        else if (control == ControlsEnums.Down)
        {
            _moveDirectionX = -1;
            addToUpdate = true;
        }

        if (control == ControlsEnums.Right)
        {
            _moveDirectionY = 1;
            addToUpdate = true;
        }
        else if (control == ControlsEnums.Left)
        {
            _moveDirectionY = -1;
            addToUpdate = true;
        }

        if (control == ControlsEnums.Mouse)
        {
            //Debug.Log(_controls.MouseAction.ReadValue<Vector2>());
        }

        if (control == ControlsEnums.OptionsMenu)
        {
            _uiManager.OpenCloseMenu();
        }

        if (addToUpdate) 
        {
            if (!_manager.DoesContainMethodInOnUpdate("MovePlayer"))
            {
                _manager.onUpdate += MovePlayer;
            }
        }
    }

    public void MovePlayer()
    {
        if (_moveDirectionX == 1)
        {
            this.transform.position += transform.forward * _speed * Time.deltaTime;
        }
        else if (_moveDirectionX == -1)
        {
            this.transform.position -= transform.forward * _speed * Time.deltaTime;
        }

        if (_moveDirectionY == -1)
        { 
            this.transform.position -= transform.right * _speed * Time.deltaTime;
        }
        else if (_moveDirectionY == 1)
        {
            this.transform.position += transform.right * _speed * Time.deltaTime;
        }
    }
}
