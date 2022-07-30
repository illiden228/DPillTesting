using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : BaseDisposable
{
    private InputAction _inputAction;
    private Vector2 _moveDirection;

    public Vector2 ClampedMoveDirection => _moveDirection.normalized;

    public PlayerInput()
    {
        _inputAction = new InputAction(binding: "<Gamepad>/rightStick");

        _inputAction.performed += OnInputAction;
        _inputAction.started += OnInputAction;
        _inputAction.canceled += OnInputAction;
        _inputAction.Enable();
    }

    protected override void OnDispose()
    {
        _inputAction.Disable();
    }

    private void OnInputAction(InputAction.CallbackContext context)
    {
        _moveDirection = context.ReadValue<Vector2>();
    }
}
