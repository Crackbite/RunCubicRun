using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CubicInputHandler : MonoBehaviour
{
    [SerializeField] private float _minForwardDistance = 30f;

    private PlayerInput _input;
    private Vector2 _previousPointerPosition;

    public event Action<Vector3> LineChanged;
    public event Action PressEscaped;
    public event Action PressSpeedReduced;

    private Vector2 CurrentPointerPosition => _input.Cubic.PointerPosition.ReadValue<Vector2>();

    private void Awake()
    {
        _input = new PlayerInput();

        _input.Cubic.KeyboardMove.performed += OnKeyboardMoved;
        _input.Cubic.PressSpeedReduce.performed += OnSpeedReduced;

        _input.Cubic.PointerPress.performed += _ => { _previousPointerPosition = CurrentPointerPosition; };
        _input.Cubic.PointerPress.canceled += OnPointerPressCanceled;
    }

    private void OnEnable()
    {
        _input.Cubic.Enable();
    }

    private void OnDisable()
    {
        _input.Cubic.Disable();
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
        Vector2 currentPosition = CurrentPointerPosition;

        if (currentPosition == _previousPointerPosition)
        {
            return;
        }

        float distanceX = currentPosition.x - _previousPointerPosition.x;
        float distanceY = currentPosition.y - _previousPointerPosition.y;

        if (Mathf.Abs(distanceX) > Mathf.Abs(distanceY))
        {
            LineChanged?.Invoke(currentPosition.x > _previousPointerPosition.x ? Vector3.back : Vector3.forward);
        }
        else if (currentPosition.y > _previousPointerPosition.y && distanceY > _minForwardDistance)
        {
            PressEscaped?.Invoke();
        }
    }

    private void OnSpeedReduced(InputAction.CallbackContext context)
    {
        PressSpeedReduced?.Invoke();
    }
}