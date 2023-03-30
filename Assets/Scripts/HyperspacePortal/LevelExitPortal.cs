using DG.Tweening;
using System;
using UnityEngine;

public class LevelExitPortal : HyperspacePortal
{
    [SerializeField] private CubicMovement _cubicMovement;
    [SerializeField] private PistonMover _pistonMover;
    [SerializeField] private PressStand _pressStand;
    [SerializeField] private float _dragDistance = 5f;
    [SerializeField] private float _dragDuration = 3f;
    [SerializeField] private float _randomness = 100f;
    [SerializeField] private int _vibration = 30;
    [SerializeField] private Vector3 _strength = new Vector3(2, 0, 2);

    private const float Delay = 1f;
    private bool _isCubicLeft;

    public event Action SuckingIn;

    private void OnEnable()
    {
        _cubicMovement.CubicLeftPress += OnCubicLeftPress;
        _pistonMover.WorkCompleted += OnPistonWorkCompleted;
    }

    private void OnDisable()
    {
        _cubicMovement.CubicLeftPress -= OnCubicLeftPress;
        _pistonMover.WorkCompleted -= OnPistonWorkCompleted;
    }

    private void OnCubicLeftPress()
    {
        _isCubicLeft = true;
        Invoke(nameof(SuckIn), Delay);
    }

    private void SuckIn()
    {
        SuckingIn?.Invoke();
        TargetScale = Vector3.zero;
        Sequence dragSequence = DOTween.Sequence();

        Tween dragByX = CubicTransform.DOMoveX(CubicTransform.position.x + _dragDistance, _dragDuration).SetEase(Ease.Linear);
        Tween shaking = CubicTransform.DOShakeRotation(_dragDuration, strength: _strength, vibrato: _vibration, randomness: _randomness).SetEase(Ease.Linear);
        Tween flight = CubicTransform.DOMove(Center.position, FlightDuration).SetEase(Ease.Linear);
        Tween scaling = CubicTransform.DOScale(TargetScale, FlightDuration).SetEase(Ease.Linear);
        Tween rotation = CubicTransform.DORotate(Vector3.right * RotationAngle * RotationSpeed, FlightDuration, RotateMode.FastBeyond360).SetEase(Ease.Linear);

        dragSequence.Append(dragByX);
        dragSequence.Join(shaking);
        dragSequence.Append(flight);
        dragSequence.Join(scaling);
        dragSequence.Join(rotation);
    }

    private void OnPistonWorkCompleted()
    {
        if (_isCubicLeft == false)
        {
            Invoke(nameof(SuckIn), Delay);
        }
    }
}
