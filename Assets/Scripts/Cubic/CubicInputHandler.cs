﻿using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CubicInputHandler : MonoBehaviour
{
    [SerializeField] private float _minForwardDistance = 30f;
    [SerializeField] private float _swipeSensitivity = 6f;
    [SerializeField] private Cubic _cubic;

    private PlayerInput _input;
    private Vector2 _previousPointerPosition;
    private bool _isCubicUnderPress;

    public event Action<Vector3> LineChanged;
    public event Action PressEscaped;
    public event Action PressSpeedReduced;

    private Vector2 CurrentPointerPosition => _input.Cubic.PointerPosition.ReadValue<Vector2>();

    private void Awake()
    {
        _input = new PlayerInput();

        _input.Cubic.KeyboardWASD.performed += OnKeyboardMoved;
        _input.Cubic.KeyboardArrow.performed += OnKeyboardMoved;
        _input.Cubic.PressSpeedReduce.performed += OnSpeedReduced;

        _input.Cubic.PointerPress.performed += _ => _previousPointerPosition = CurrentPointerPosition;
        _input.Cubic.PointerPress.canceled += OnPointerPressCanceled;
    }

    private void OnEnable()
    {
        _input.Cubic.Enable();
        _cubic.SteppedOnStand += OnCubicSteppedOnStand;
    }

    private void OnDisable()
    {
        _input.Cubic.Disable();
        _cubic.SteppedOnStand -= OnCubicSteppedOnStand;
    }

    private void OnKeyboardMoved(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector3>();

        if (direction == Vector3.zero)
        {
            return;
        }

        if (direction == Vector3.up)
        {
            PressEscaped?.Invoke();
        }
        else
        {
            LineChanged?.Invoke(direction);
        }
    }

    private void OnPointerPressCanceled(InputAction.CallbackContext context)
    {
        if (Application.isMobilePlatform == false)
        {
            return;
        }

        Vector2 currentPosition = CurrentPointerPosition;

        if (currentPosition == _previousPointerPosition)
        {
            return;
        }

        float distanceX = currentPosition.x - _previousPointerPosition.x;
        float distanceY = currentPosition.y - _previousPointerPosition.y;
        distanceX *= _swipeSensitivity;

        if (Mathf.Abs(distanceX) > Mathf.Abs(distanceY) && _isCubicUnderPress == false)
        {
            LineChanged?.Invoke(currentPosition.x > _previousPointerPosition.x ? Vector3.back : Vector3.forward);
        }
        else if (currentPosition.y > _previousPointerPosition.y && distanceY > _minForwardDistance && _isCubicUnderPress)
        {
            PressEscaped?.Invoke();
        }
    }

    private void OnSpeedReduced(InputAction.CallbackContext context)
    {
        PressSpeedReduced?.Invoke();
    }

    private void OnCubicSteppedOnStand(PressStand pressStand)
    {
        _isCubicUnderPress = true;
    }
}